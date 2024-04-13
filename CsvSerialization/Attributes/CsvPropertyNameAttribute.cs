namespace CsvSerialization;

/// <summary>
/// Attribute indicating that property has specified name in csv format.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CsvPropertyNameAttribute : Attribute
{
    /// <summary>
    /// The name of the property in csv format.
    /// </summary>
    public string PropertyName { get; private init; }
    
    /// <summary>
    /// Creates an instance of <see cref="CsvPropertyNameAttribute"/>
    /// </summary>
    /// <param name="csvPropertyName">The name of the property in csv format.</param>
    public CsvPropertyNameAttribute(string csvPropertyName)
    {
        ArgumentNullException.ThrowIfNull(csvPropertyName, nameof(csvPropertyName));
        if (csvPropertyName.Length < 1)
            throw new ArgumentException("Name must be not empty.");
        PropertyName = csvPropertyName;
    }
}