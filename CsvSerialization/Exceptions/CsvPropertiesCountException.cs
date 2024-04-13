namespace CsvSerialization;

public class CsvPropertiesCountException : DeserializationException
{
    public CsvPropertiesCountException() 
        : base("A csv-format string's properies count doesn't match type properties count")
    {
        
    }
}