using Amazon.S3;
using Amazon.S3.Model;
using EduCrm.Modules.Account.Application.Repositories;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Storage;

public sealed class DigitalOceanSpacesReceiptStorage : IReceiptStorage
{
    private readonly IAmazonS3 _s3;
    private readonly DigitalOceanSpacesOptions _options;

    public DigitalOceanSpacesReceiptStorage(IAmazonS3 s3, IOptions<DigitalOceanSpacesOptions> options)
    {
        _s3 = s3;
        _options = options.Value;
    }

    public async Task UploadAsync(string objectKey, Stream content, string contentType, CancellationToken ct)
    {
        var request = new PutObjectRequest
        {
            BucketName = _options.Bucket,
            Key = objectKey,
            InputStream = content,
            ContentType = contentType,
            CannedACL = S3CannedACL.Private,
            DisablePayloadSigning = true
        };

        await _s3.PutObjectAsync(request, ct);
    }

    public async Task<Stream> DownloadAsync(string objectKey, CancellationToken ct)
    {
        var response = await _s3.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _options.Bucket,
            Key = objectKey
        }, ct);

        return response.ResponseStream;
    }
}
