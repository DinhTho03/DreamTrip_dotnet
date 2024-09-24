namespace brandportal_dotnet.Models;

public class LookupDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}

public class LookupDto<T>
{
    public T Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}