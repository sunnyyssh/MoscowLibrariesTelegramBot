using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Data;

partial class Library
{
    public static readonly ReadOnlyDictionary<string, IFilter<Library>> Filter = new(
        new Dictionary<string, IFilter<Library>>()
        {
            { "AdmArea", new LibraryFilter(lib => lib.AdmArea)},
            { "WiFiName", new LibraryFilter(lib => lib.WiFiName)},
            { "FunctionFlag;AccessFlag", new LibraryFilter(lib => $"{lib.FunctionFlag};{lib.AccessFlag}")}
        });

    private delegate bool TryParseDelegate<T>(string? input, [NotNullWhen(true)] out T? value);
    
    private class LibraryFilter : IFilter<Library>
    {
        private readonly Func<Library, object?> _selector;
        
        private readonly TryParseDelegate<object>? _tryParse;

        public bool TryFilterBy(IEnumerable<Library> input, string? by,
            [NotNullWhen(true)] out IEnumerable<Library>? output)
        {
            output = null;

            object? obj;
            if (_tryParse is not null)
            {
                if (!_tryParse(by, out obj))
                {
                    return false;
                }
            }
            else
            {
                obj = by;
            }

            output = input.Where(lib => _selector(lib) == obj);
            return true;
        }

        public LibraryFilter(Func<Library, object?> selector, TryParseDelegate<object>? tryParse = null)
        {
            _selector = selector;
            _tryParse = tryParse;
        }
    }
}