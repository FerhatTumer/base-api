using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly string _senderEmail;

    public EmailService(IAmazonSimpleEmailService sesClient, IConfiguration configuration)
    {
        _sesClient = sesClient;
        _senderEmail = configuration["AWS:SES:SenderEmail"] ?? throw new InvalidOperationException("AWS SES sender email is not configured.");
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        SendEmailRequest sendRequest = new()
        {
            Source = _senderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { to }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(body)
                }
            }
        };

        await _sesClient.SendEmailAsync(sendRequest, cancellationToken);
    }
}