using System.Data.Common;
using System.Text;

namespace Data;

public class CsvProcessing<TDataElement> : IDataProcessing<TDataElement[]>
{
    private readonly ICsvSerializer<TDataElement> _serializer;

    public Stream Write(TDataElement[] data)
    {
        var oneLine = string.Join(Environment.NewLine,
            Enumerable.Empty<string>().Append(_serializer.GetTitleLine())
                .Concat(data.Select(_serializer.Serialize)));

        var serialized = Encoding.UTF8.GetBytes(oneLine);
        
        var stream = new MemoryStream(serialized);
        stream.Position = 0;
        
        return stream;
    }

    public Task<Stream> WriteAsync(TDataElement[] data)
    {
        return Task.Run(() => Write(data));
    }

    public TDataElement[]? Read(Stream source)
    {
        StreamReader reader = new StreamReader(source, Encoding.UTF8);

        var titles = reader.ReadLine();
        
        if (titles is null || _serializer.CheckTitleLine(titles))
        {
            return null;
        }

        var deserializedList = new List<TDataElement>();
        var currentLine = reader.ReadLine();

        while (currentLine is not null)
        {
            var deserialized = _serializer.Deserialize(currentLine);
            if (deserialized is not null)
            {
                deserializedList.Add(deserialized);
            }
            
            currentLine = reader.ReadLine();
        }

        return deserializedList.ToArray();
    }

    public async Task<TDataElement[]?> ReadAsync(Stream source)
    {
        StreamReader reader = new StreamReader(source, Encoding.UTF8);

        var titles = await reader.ReadLineAsync();
        
        if (titles is null || _serializer.CheckTitleLine(titles))
        {
            return null;
        }

        var deserializedList = new List<TDataElement>();
        var currentLine = await reader.ReadLineAsync();

        while (currentLine is not null)
        {
            var deserialized = _serializer.Deserialize(currentLine);
            if (deserialized is not null)
            {
                deserializedList.Add(deserialized);
            }
            
            currentLine = await reader.ReadLineAsync();
        }

        return deserializedList.ToArray();
    }

    public CsvProcessing(ICsvSerializer<TDataElement> serializer)
    {
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));
        
        _serializer = serializer;
    }
}