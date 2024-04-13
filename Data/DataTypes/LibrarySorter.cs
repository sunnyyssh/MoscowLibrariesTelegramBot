using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Data;

partial class Library
{
    public static readonly ReadOnlyDictionary<string, ISorter<Library>> Sorters = new(
        new Dictionary<string, ISorter<Library>>()
        {
            { "LibraryName", new LibrarySorter(lib => lib.LibraryName) },
            { "CoverageArea", new LibrarySorter(lib => lib.CoverageArea, true) }
        });
    
    private class LibrarySorter : ISorter<Library>
    {
        private readonly Func<Library, IComparable?> _selector;
        
        private readonly bool _descending;

        public IEnumerable<Library> Sort(IEnumerable<Library> input)
        {
            return _descending ? input.OrderByDescending(_selector) : input.OrderBy(_selector);
        }

        public LibrarySorter(Func<Library, IComparable?> selector, bool descending = false)
        {
            _selector = selector;
            _descending = descending;
        }
    }
}