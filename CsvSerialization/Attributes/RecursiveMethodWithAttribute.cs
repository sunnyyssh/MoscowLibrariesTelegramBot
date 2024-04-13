namespace CsvSerialization;

/// <summary>
/// Indicates that this method has recursive calls.
/// </summary>
internal class RecursiveMethodWithAttribute : Attribute
{
    /// <summary>
    /// Indicates that this method has recursive calls.
    /// </summary>
    /// <param name="methodNames">Methods it is recursive with.</param>
    public RecursiveMethodWithAttribute(params string[] methodNames)
    {
        
    }
}