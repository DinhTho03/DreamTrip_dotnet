using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace brandportal_dotnet.shared;

public static class PredicateBuilder
{
    /// <summary>
    /// Build predicate filter
    /// </summary>
    /// <typeparam name="T">Type of query</typeparam>
    /// <param name="propertyName">Property name</param>
    /// <param name="comparison">Comparison</param>
    /// <param name="value">Value</param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Build<T>(string propertyName, string comparison, string value)
    {
        // any name
        const string parameterName = "x";
        var parameter = Expression.Parameter(typeof(T), parameterName);
        var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
        var body = MakeComparison(left, comparison, value);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression MakeComparison(Expression left, string comparison, string value)
    {
        return comparison switch
        {
            "==" => MakeBinary(ExpressionType.Equal, left, value),
            "!=" => MakeBinary(ExpressionType.NotEqual, left, value),
            ">" => MakeBinary(ExpressionType.GreaterThan, left, value),
            ">=" => MakeBinary(ExpressionType.GreaterThanOrEqual, left, value),
            "<" => MakeBinary(ExpressionType.LessThan, left, value),
            "<=" => MakeBinary(ExpressionType.LessThanOrEqual, left, value),
            "On" => MakeDate(left, value),
            "Contains" or "StartsWith" or "EndsWith" => MakeString(left, comparison, value),
            "In" => MakeList(left, value.Split(',')),
            "NotIn" => MakeNotIn(left, value.Split(',')),
            _ => throw new NotSupportedException($"Comparison {comparison} not supported"),
        };
    }
    
    private static Expression MakeList(Expression left, IEnumerable<string> codes)
    {
        var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
        var valueQueryType = left.Type;
        var constants = codes.Select(c =>
        {
            Expression expr;
            if (valueType.IsEnum)
            {
                if (Enum.TryParse(valueType, c, out var val))
                {
                    expr = Expression.Constant(val, valueQueryType);
                }
                else
                {
                    Enum.TryParse(valueType, Int16.MinValue.ToString(), out val);
                    expr = Expression.Constant(val, valueQueryType);
                }
            }
            else if (valueType == typeof(Guid) && Guid.TryParse(c, out var guidVal))
            {
                expr = Expression.Convert(Expression.Constant(guidVal), valueQueryType);
            }
            else if (valueType != typeof(string))
            {
                expr = Expression.Convert(Expression.Constant(Convert.ChangeType(c, valueType)), valueQueryType);
            }
            else
            {
                expr = Expression.Constant(c, valueQueryType);
            }

            return expr;
        }).ToList();

        var containsMethod = typeof(Enumerable).GetMethods()
            .Where(m => m.Name == "Contains")
            .Single(m => m.GetParameters().Length == 2)
            .MakeGenericMethod(valueType);

        if (Nullable.GetUnderlyingType(left.Type) != null)
        {
            var nonNullableType = Nullable.GetUnderlyingType(left.Type);
            var nonNullableLeft = Expression.Convert(left, nonNullableType);
            var arrayExpression = Expression.NewArrayInit(nonNullableType,
                constants.Select(c => Expression.Convert(c, nonNullableType)));
            var exprContains = Expression.Call(containsMethod, arrayExpression, nonNullableLeft);
            return exprContains;
        }
        else
        {
            var arrayExpression = Expression.NewArrayInit(valueType, constants);
            var exprContains = Expression.Call(containsMethod, arrayExpression, left);
            return exprContains;
        }
    }
    
    private static Expression MakeNotIn(Expression left, IEnumerable<string> codes)
    {
        var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
        var valueQueryType = left.Type;
        var constants = codes.Select(c =>
        {
            Expression expr;
            if (valueType.IsEnum)
            {
                if (Enum.TryParse(valueType, c, out var val))
                {
                    expr = Expression.Constant(val, valueQueryType);
                }
                else
                {
                    Enum.TryParse(valueType, Int16.MinValue.ToString(), out val);
                    expr = Expression.Constant(val, valueQueryType);
                }
            }
            else if (valueType == typeof(Guid) && Guid.TryParse(c, out var guidVal))
            {
                expr = Expression.Convert(Expression.Constant(guidVal), valueQueryType);
            }
            else if (valueType != typeof(string))
            {
                expr = Expression.Convert(Expression.Constant(Convert.ChangeType(c, valueType)), valueQueryType);
            }
            else
            {
                expr = Expression.Constant(c, valueQueryType);
            }

            return expr;
        }).ToList();

        var containsMethod = typeof(Enumerable).GetMethods()
            .Where(m => m.Name == "Contains")
            .Single(m => m.GetParameters().Length == 2)
            .MakeGenericMethod(valueType);

        if (Nullable.GetUnderlyingType(left.Type) != null)
        {
            var nonNullableType = Nullable.GetUnderlyingType(left.Type);
            var nonNullableLeft = Expression.Convert(left, nonNullableType);
            var arrayExpression = Expression.NewArrayInit(nonNullableType,
                constants.Select(c => Expression.Convert(c, nonNullableType)));
            var exprNotContains = Expression.Not(Expression.Call(containsMethod, arrayExpression, nonNullableLeft));
            return exprNotContains;
        }
        else
        {
            var arrayExpression = Expression.NewArrayInit(valueType, constants);
            var exprNotContains = Expression.Not(Expression.Call(containsMethod, arrayExpression, left));
            return exprNotContains;
        }
    }

    private static Expression MakeString(Expression left, string comparison, string value)
    {
        var nullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(object)));
        return Expression.AndAlso(nullCheck,
            Expression.Call(MakeString(left), comparison, Type.EmptyTypes,
                Expression.Constant(value.ToLower(), typeof(string))));
    }

    private static Expression MakeString(Expression source)
    {
        return source.Type != typeof(string) ? source : Expression.Call(source, "ToLower", Type.EmptyTypes);
    }

    private static Expression MakeBinary(ExpressionType type, Expression left, string value)
    {
        object? typedValue = value;
        if (left.Type != typeof(string))
        {
            if (string.IsNullOrEmpty(value))
            {
                typedValue = null;
                if (Nullable.GetUnderlyingType(left.Type) == null)
                {
                    left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
            }
            else
            {
                var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                    valueType == typeof(Guid) ? Guid.Parse(value) :
                    valueType == typeof(DateOnly) ? DateOnly.Parse(DateTime.Parse(value).ToString("dd/MM/yyyy")) :
                    valueType == typeof(TimeSpan) ? TimeSpan.Parse(value) :

                    Convert.ChangeType(value, valueType, CultureInfo.InvariantCulture);
            }
        }

        var right = Expression.Constant(typedValue, left.Type);
        return Expression.MakeBinary(type, left, right);
    }

    private static Expression MakeDate(Expression left, string value)
    {
        if (!DateTime.TryParse(value, out var filterDate))
        {
            throw new ArgumentException("");
        }

        var utcDateTime = filterDate.Date.ToUniversalTime();
        var type = typeof(DateTime);
        if (Nullable.GetUnderlyingType(left.Type) != null)
        {
            type = typeof(DateTime?);
        }

        Expression startDateExpression = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, left,
            Expression.Constant(utcDateTime, type));
        Expression endDateExpression = Expression.MakeBinary(ExpressionType.LessThan, left,
            Expression.Constant(utcDateTime.AddDays(1), type));
        return Expression.And(startDateExpression, endDateExpression);
    }
}