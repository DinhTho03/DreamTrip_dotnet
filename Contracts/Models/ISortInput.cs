namespace brandportal_dotnet.Models;

public interface ISortInput
{
    /// <summary>
    /// Sort direction: asc, desc
    /// </summary>
    public string? Direction { get; }
    
    /// <summary>
    /// Sort active column
    /// </summary>
    public string? Active { get; }
}