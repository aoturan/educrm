namespace EduCrm.Modules.Account.Application.Repositories;

public interface IReceiptStorage
{
    Task UploadAsync(
        string objectKey,
        Stream content,
        string contentType,
        CancellationToken ct);

    Task<Stream> DownloadAsync(string objectKey, CancellationToken ct);
}

public sealed class DigitalOceanSpacesOptions
{
    public string ServiceUrl { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public string AccessKeyId { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
}
