using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace TaskManagement.Infrastructure.AWS;

public interface ISecretsManagerService
{
    Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken);
}

public sealed class SecretsManagerService : ISecretsManagerService
{
    private readonly IAmazonSecretsManager _secretsManagerClient;

    public SecretsManagerService(IAmazonSecretsManager secretsManagerClient)
    {
        _secretsManagerClient = secretsManagerClient;
    }

    public async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken)
    {
        GetSecretValueRequest request = new()
        {
            SecretId = secretName
        };

        GetSecretValueResponse response = await _secretsManagerClient.GetSecretValueAsync(request, cancellationToken);
        return response.SecretString;
    }
}