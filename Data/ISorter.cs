using System.Diagnostics.CodeAnalysis;

namespace Data;

public interface ISorter<TData>
{
     IEnumerable<TData> Sort(IEnumerable<TData> input);
}