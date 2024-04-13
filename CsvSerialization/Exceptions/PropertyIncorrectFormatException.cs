namespace CsvSerialization;

public class PropertyIncorrectFormatException : DeserializationException
{
    public PropertyIncorrectFormatException(string csvValue, string propertyName)
        : base($"Can't parse \"{csvValue}\" to {propertyName} property.")
    {
        
    }
}