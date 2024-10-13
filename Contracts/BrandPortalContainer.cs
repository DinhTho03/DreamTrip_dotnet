using brandportal_dotnet.Common;
using Volo.Abp.BlobStoring;

namespace brandportal_dotnet.Contracts;

[BlobContainerName(BlobContainerNameConst.Default)]
public class BrandPortalContainer : IBlobContainerName;

[BlobContainerName(BlobContainerNameConst.Report)]
public class BrandPortalReportContainer : IBlobContainerName;

public interface IBlobContainerName;