using brandportal_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace brandportal_dotnet.shared;

public static class LinqExtensions
{
    public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, IPagingInput pagingInput)
        => source.Page(pagingInput.PageIndex, pagingInput.PageSize);
    
    public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, IPagingInput pagingInput)
        => source.Page(pagingInput.PageIndex, pagingInput.PageSize);
    
    public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
    {
        if (pageSize > 0)
        {
            var skip = Math.Max(0, (page - 1) * pageSize);
            return source.Skip(skip).Take(pageSize);
        }

        return source;
    }
    
    public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
    {
        if (pageSize > 0)
        {
            var skip = Math.Max(0, (page - 1) * pageSize);
            return source.Skip(skip).Take(pageSize);
        }

        return source;
    }

    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, IFilterInput filterInput) =>
     source.Where(filterInput.Filters);

    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ColumnFilter[] filters) =>
        !filters.Any()
            ? source
            : filters.Aggregate(source, (current, item) => current.Where(ConvertToCamelCase(item.Key), item.Comparison, item.Value));

    private static IEnumerable<T> Where<T>(this IEnumerable<T> source, string propertyName, string comparison, string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !typeof(T).HasProperty(propertyName))
        {
            return source;
        }
        return source.Where(PredicateBuilder.Build<T>(propertyName, comparison, value).Compile());
    }


    private static string ConvertToPascalCase(string input)
    {
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        var words = input.Split('_');
        for (var i = 0; i < words.Length; i++)
        {
            words[i] = textInfo.ToTitleCase(words[i]);
        }
        return string.Join("", words);
    }
    
    private static string ConvertToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Convert the first character to uppercase
        var firstChar = char.ToUpper(input[0]);
        // Combine the first character with the rest of the string
        var result = firstChar + input[1..];

        return result;
    }
    
    private static bool HasProperty(this Type obj, string propertyName) =>
        !string.IsNullOrEmpty(propertyName) && obj.GetProperty(propertyName) != null;

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ISortInput sortInput) => source.OrderBy(
        !string.IsNullOrEmpty(sortInput.Active) && !string.IsNullOrEmpty(sortInput.Direction)
            ?
            [
                new ColumnOrder
                {
                    Key = sortInput.Active,
                    Direction = sortInput.Direction,
                }
            ]
            : Array.Empty<ColumnOrder>());

    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, ISortInput sortInput) => source.OrderBy(
        !string.IsNullOrEmpty(sortInput.Active) && !string.IsNullOrEmpty(sortInput.Direction)
            ?
            [
                new ColumnOrder
                {
                    Key = sortInput.Active,
                    Direction = sortInput.Direction,
                }
            ]
            : Array.Empty<ColumnOrder>());

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string? direction, string? key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return source;
        }
        
        var sortExpression = BuildExpression<T>(key);

        return direction == "asc" ? source.OrderBy(sortExpression) : source.OrderByDescending(sortExpression);
    }
    
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, ColumnOrder[] sorts)
    {
        if (!sorts.Any())
        {
            return source;
        }

        var sort = sorts[0];
        var sortExpression = BuildExpression<T>(sort.Key).Compile();
        var orderSource = sort.Direction == "asc"
            ? source.OrderBy(sortExpression)
            : source.OrderByDescending(sortExpression);

        foreach (var column in sorts.Take(1..))
        {
            var columnExpression = BuildExpression<T>(sort.Key).Compile();
            orderSource = column.Direction == "asc"
                ? orderSource.ThenBy(columnExpression)
                : orderSource.ThenByDescending(columnExpression);
        }
        return orderSource;
    }
    
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ColumnOrder[] sorts)
    {
        if (!sorts.Any())
        {
            return source;
        }

        var sort = sorts[0];
        var sortExpression = BuildExpression<T>(sort.Key);
        var orderSource = sort.Direction == "asc"
            ? source.OrderBy(sortExpression)
            : source.OrderByDescending(sortExpression);

        foreach (var column in sorts.Take(1..))
        {
            var columnExpression = BuildExpression<T>(sort.Key);
            orderSource = column.Direction == "asc"
                ? orderSource.ThenBy(columnExpression)
                : orderSource.ThenByDescending(columnExpression);
        }
        return orderSource;
    }
    
    private static Expression<Func<T, object>> BuildExpression<T>(string propToOrder)
    {
        var param = Expression.Parameter(typeof(T));
        var memberAccess = Expression.Property(param, propToOrder);        
        var convertedMemberAccess = Expression.Convert(memberAccess, typeof(object));
        return Expression.Lambda<Func<T, object>>(convertedMemberAccess, param);
    }
}