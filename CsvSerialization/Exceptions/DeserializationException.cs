namespace CsvSerialization;

public class DeserializationException : SerializationException
{
    public DeserializationException()
    {
        
    }

    public DeserializationException(string message) : base(message)
    {
        
    }
}