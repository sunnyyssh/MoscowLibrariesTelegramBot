using System.Text.Encodings.Web;
using System.Text.Json;

namespace Data;

public class JsonProcessing<TData> : IDataProcessing<TData>
{
    private JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    
    public Stream Write(TData data)
    {
        var stream = new MemoryStream();

        JsonSerializer.Serialize(stream, data, _options);
        stream.Position = 0;

        return stream;
    }

    public async Task<Stream> WriteAsync(TData data)
    {
        var stream = new MemoryStream();

        await JsonSerializer.SerializeAsync(stream, data, _options);
        stream.Position = 0;

        return stream;
    }

    public TData? Read(Stream source)
    {
        return JsonSerializer.Deserialize<TData>(source, _options);
    }

    public async Task<TData?> ReadAsync(Stream source)
    {
        return await JsonSerializer.DeserializeAsync<TData>(source, _options);
    }
}