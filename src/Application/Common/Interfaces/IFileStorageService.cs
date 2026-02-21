namespace TaskManagement.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);

    Task<Stream> DownloadFileAsync(string fileKey, CancellationToken cancellationToken);

    Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken);
}