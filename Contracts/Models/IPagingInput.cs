namespace brandportal_dotnet.Models;

public interface IPagingInput
{
    /// <summary>
    /// Page index
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }
}