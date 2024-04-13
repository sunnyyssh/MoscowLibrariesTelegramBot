using System.Diagnostics.CodeAnalysis;

namespace Data;

public interface IFilter<TData>
{
    bool TryFilterBy(IEnumerable<TData> input, string? by, 
        [NotNullWhen(true)] out IEnumerable<TData>? output);
}