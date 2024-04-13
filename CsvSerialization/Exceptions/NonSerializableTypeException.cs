namespace CsvSerialization;

public class NonSerializableTypeException : SerializationException
{
    public NonSerializableTypeException(Type type) 
        : base($"{type.FullName} is not supported for csv serialization. " +
               $"Check if that type is primitive or marked with CsvTypeAttribute")
    {
        
    }
}