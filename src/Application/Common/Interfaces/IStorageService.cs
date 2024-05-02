using BevMan.Domain.Entities;

namespace BevMan.Application.Common.Interfaces;

public interface IStorageService
{
    public Task<string> UploadFileAsync(Blob blob, string bucket, string path,
        CancellationToken cancellationToken);

    public Task DeleteFileAsync(string bucket, string path, CancellationToken cancellationToken);
}
