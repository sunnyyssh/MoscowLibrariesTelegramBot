using System.Reflection;

namespace CsvSerialization;

/// <summary>
/// Attribute indicating that this type can be converted to csv in another type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class CsvTypeAttribute : Attribute
{
    private readonly Type _type;
    
    /// <summary>
    /// Creates an instance <see cref="CsvTypeAttribute"/>.
    /// </summary>
    /// <param name="thisType">The type this attribute is used for.</param>
    public CsvTypeAttribute(Type thisType)
    {
        _type = thisType;
        //throw new NotImplementedException();
    }
    //
    // internal int CsvPropertiesCount => SortedCsvProperties.Length;
    //
    // internal PropertyHandler[] SortedCsvProperties
    // {
    //     get
    //     {
    //         if (_csvProperties is not null)
    //             return _csvProperties;
    //         _csvProperties = CsvSerializer.GetSortedCsvProperties(_type);
    //         return _csvProperties;
    //         throw new NotImplementedException();
    //     }
    // }
}