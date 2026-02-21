using Amazon.SimpleEmail;

namespace TaskManagement.Infrastructure.AWS;

public sealed class SESService
{
    public SESService(IAmazonSimpleEmailService client)
    {
        Client = client;
    }

    public IAmazonSimpleEmailService Client { get; }
}