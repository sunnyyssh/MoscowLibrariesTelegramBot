namespace CsvSerialization;

public class PrimitiveTypeExpectedException : SerializationException
{
    public PrimitiveTypeExpectedException(Type type) 
        : base($"{type.Name} is not primitive type but is marked with {nameof(CsvIncludeAtAttribute)}. Maybe it is embedded csv type.")
    {
        
    }
}