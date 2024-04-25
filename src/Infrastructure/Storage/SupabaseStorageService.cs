using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Entities;
using Supabase.Storage;
using Client = Supabase.Client;
using FileOptions = Supabase.Storage.FileOptions;

namespace BevMan.Infrastructure.Storage;

public class SupabaseStorageService : IStorageService
{
    private readonly Client _supabaseClient;

    public SupabaseStorageService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task DeleteFileAsync(string bucket, string path, CancellationToken cancellationToken)
    {
        await _supabaseClient.Storage.From(bucket).Remove(path);
    }

    public async Task<(string Path, string PublicUrl)> UploadFileAsync(Blob blob, string bucket, string path,
        CancellationToken cancellationToken)
    {
        Bucket? existingBucket = await _supabaseClient.Storage.GetBucket(bucket);
        if (existingBucket is null)
        {
            await _supabaseClient.Storage.CreateBucket(bucket, new BucketUpsertOptions { Public = true });
        }

        await using Stream stream = blob.Content();
        await using MemoryStream memoryStream = new();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        string resultPath = await _supabaseClient.Storage.From(bucket)
            .Upload(memoryStream.ToArray(), path,
                new FileOptions { Upsert = true });
        string publicUrl = _supabaseClient.Storage.From(bucket).GetPublicUrl(resultPath);
        return (resultPath, publicUrl);
    }
}
