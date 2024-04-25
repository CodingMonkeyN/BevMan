using Microsoft.AspNetCore.Http;

namespace BevMan.Domain.Entities;

public class Blob
{
    public Blob(IFormFile file) : this(file, new Dictionary<string, string>())
    {
    }

    public Blob(IFormFile file, IDictionary<string, string> metadata) : this(
        $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}",
        file.FileName,
        file.OpenReadStream,
        file.ContentType,
        metadata
    )
    {
    }

    public Blob(string name, string fileName, Func<Stream> content, string contentType) : this(
        name,
        fileName,
        content,
        contentType,
        new Dictionary<string, string>()
    )
    {
    }

    private Blob(string name, string fileName, Func<Stream> content, string contentType,
        IDictionary<string, string> metadata)
    {
        Dictionary<string, string> mergedMetadata = new(metadata) { { "FileName", fileName } };
        Name = name;
        FileName = fileName;
        Content = content;
        ContentType = contentType;
        Metadata = mergedMetadata;
    }

    public string Name { get; }
    public string FileName { get; }
    public Func<Stream> Content { get; }
    public string ContentType { get; }
    public IDictionary<string, string> Metadata { get; }
}
