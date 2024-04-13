namespace Data;

public interface IDataProcessing<TData>
{
    Stream Write(TData data);
    
    Task<Stream> WriteAsync(TData data);

    TData? Read(Stream source);

    Task<TData?> ReadAsync(Stream source);
}