namespace CsvSerialization;

public class ParameterlessConstructorNotFoundException : DeserializationException
{
    public ParameterlessConstructorNotFoundException(Type type)
        : base($"{type.Name} doesn't have parameterless constructor.")
    {
        
    }
}