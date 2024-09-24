namespace brandportal_dotnet.Models;

public interface IFilterInput
{
    /// <summary>
    /// Filter
    /// </summary>
    public ColumnFilter[] Filters { get; }
}