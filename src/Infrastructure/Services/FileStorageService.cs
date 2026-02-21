using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public sealed class FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public FileStorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:S3:BucketName"] ?? throw new InvalidOperationException("AWS S3 bucket name is not configured.");
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
    {
        string key = $"{Guid.NewGuid()}/{fileName}";

        PutObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
        return key;
    }

    public async Task<Stream> DownloadFileAsync(string fileKey, CancellationToken cancellationToken)
    {
        GetObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = fileKey
        };

        GetObjectResponse response = await _s3Client.GetObjectAsync(request, cancellationToken);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken)
    {
        DeleteObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = fileKey
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }
}