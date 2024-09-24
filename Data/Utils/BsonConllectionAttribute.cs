namespace brandportal_dotnet.Data.Utils;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]

public class BsonConllectionAttribute : Attribute
{
    public string CollectionName { get;  }
    public BsonConllectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
        
    
}