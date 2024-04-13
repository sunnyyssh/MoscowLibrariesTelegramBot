namespace CsvSerialization;

public class WrongCsvTitlesException : DeserializationException
{
    public WrongCsvTitlesException(string wrongTitle) 
        : base($"\"{wrongTitle}\" is incorrect title.")
    {
        
    }

    public WrongCsvTitlesException()
        : base("Titles were incorrect.")
    {
        
    }
}