namespace CsvSerialization;

public class CsvIncludeCsvTypeAtAttribute : Attribute
{
    public int[] Positions { get; private init; }
    
    public CsvIncludeCsvTypeAtAttribute(params int[] positions)
    {
        Positions = positions;
    }
}