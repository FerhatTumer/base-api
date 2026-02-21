using Amazon.S3;

namespace TaskManagement.Infrastructure.AWS;

public sealed class S3Service
{
    public S3Service(IAmazonS3 client)
    {
        Client = client;
    }

    public IAmazonS3 Client { get; }
}