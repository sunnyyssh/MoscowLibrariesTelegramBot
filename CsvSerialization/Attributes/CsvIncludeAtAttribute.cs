namespace CsvSerialization;

/// <summary>
/// Attribute indicating that property is included at specified position in csv table. 
/// </summary>
public class CsvIncludeAtAttribute : Attribute
{
    /// <summary>
    /// The position number where property stands at.
    /// </summary>
    public int Position { get; private init; }
    
    /// <summary>
    /// Initializes <see cref="CsvIncludeAtAttribute"/>.
    /// </summary>
    /// <param name="position">The position in csv table. The indexing starts with 0 and has no skips.</param>
    public CsvIncludeAtAttribute(int position)
    {
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Index must be more than or equal to zero.");
        }

        Position = position;
    }
}