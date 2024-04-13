using System.Reflection;
using System.Runtime.CompilerServices;

namespace CsvSerialization;

internal static class SerializingHelper
{
    [RecursiveMethodWith(nameof(GetSortedCsvValues))]
    internal static string[] GetCsvValues(Type type, object csvObj)
    {
        PropertyInfo[] bareProperties = type.GetProperties();
        List<(int, string)> csvValues = new List<(int, string)>();

        foreach (PropertyInfo property in bareProperties)
        {
            if (TryGetAttribute(property, out CsvIncludeAtAttribute? includeAtAttribute))
            {
                if (!IsPrimitiveType(property.PropertyType))
                    throw new PrimitiveTypeExpectedException(property.PropertyType);
                int pos = includeAtAttribute!.Position;
                object propValue = property.GetValue(csvObj)!;
                string converted = ConvertPrimitive(property.PropertyType, propValue);
                csvValues.Add((pos, converted));
                continue;
            }

            if (TryGetAttribute(property, out CsvIncludeCsvTypeAtAttribute? csvTypeAtAttribute))
            {
                object propValue = property.GetValue(csvObj)!;
                var embedded = GetSortedCsvValues(property.PropertyType, propValue, csvTypeAtAttribute!.Positions);
                csvValues.AddRange(embedded);
            }
        }
        
        var sortedEnumerable =
            (from it in csvValues
                orderby it.Item1
                select it).ToArray();

        if (!CheckCorrectPositions(sortedEnumerable.Select(x => x.Item1)))
            throw new PositionsSerializationException("Positions have gapes or have not zero first position.");
        
        return sortedEnumerable.Select(x => x.Item2).ToArray();
    }

    [RecursiveMethodWith(nameof(GetCsvValues))]
    private static (int, string)[] GetSortedCsvValues(Type csvType, object csvObj, int[] positions)
    {
        if (!TryGetAttribute(csvType, out CsvTypeAttribute? csvTypeAttribute))
        {
            throw new NonSerializableTypeException(csvType);
        }
        string[] sortedValues = GetCsvValues(csvType, csvObj);
        if (sortedValues.Length != positions.Length)
        {
            throw new PositionsSerializationException(
                "Positions count marked in serializable property doesn't match embedded type positions count.");
        }
        (int, string)[] result = new (int, string)[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            result[i] = (positions[i], sortedValues[i]);
        }

        return result;
    }

    private static readonly Type[] PrimitiveTypes =
    {
        typeof(int),
        typeof(long),
        typeof(byte),
        typeof(DateTime),
        typeof(string),
        typeof(double),
        typeof(float),
    };
    
    internal static bool IsPrimitiveType(Type type)
    {
        return PrimitiveTypes.Any(x => x == type);
    }

    private static string ConvertPrimitive(Type type, object? obj)
    {
        return obj?.ToString() ?? "";
    }
    
    [RecursiveMethodWith(nameof(GetSortedSubTitles))]
    internal static string[] GetTitles(Type type)
    {
        PropertyInfo[] bareProperties = type.GetProperties();
        List<(int, string)> csvTitles = new List<(int, string)>();

        foreach (PropertyInfo bareProperty in bareProperties)
        {
            if (TryGetAttribute(bareProperty, out CsvIncludeAtAttribute? includeAtAttribute))
            {
                csvTitles.Add((includeAtAttribute!.Position, GetCsvPropertyName(bareProperty)));
                continue;
            }

            if (TryGetAttribute(bareProperty, out CsvIncludeCsvTypeAtAttribute? typeAtAttribute))
            {
                var embedded = GetSortedSubTitles(bareProperty.PropertyType, typeAtAttribute!.Positions);
                csvTitles.AddRange(embedded);
            }
        }

        var sortedEnumerable =
            (from it in csvTitles
            orderby it.Item1
            select it).ToArray();

        if (!CheckCorrectPositions(sortedEnumerable.Select(x => x.Item1)))
            throw new PositionsSerializationException("Positions have gapes or have not zero first position.");
        
        return sortedEnumerable.Select(x => x.Item2).ToArray();
    }

    private static bool CheckCorrectPositions(IEnumerable<int> pos)
    {
        int curr = 0;
        foreach (int i in pos)
        {
            if (i != curr)
            {
                return false;
            }

            curr++;
        }

        return true;
    }
    

    [RecursiveMethodWith(nameof(GetTitles))]
    private static (int, string)[] GetSortedSubTitles(Type csvType, int[] positions)
    {
        if (!TryGetAttribute(csvType, out CsvTypeAttribute? csvTypeAttribute))
        {
            throw new NonSerializableTypeException(csvType);
        }
        string[] sortedTitles = GetTitles(csvType);
        if (sortedTitles.Length != positions.Length)
        {
            throw new PositionsSerializationException(
                "Positions count marked in serializable property doesn't match embedded type positions count.");
        }
        (int, string)[] result = new (int, string)[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            result[i] = (positions[i], sortedTitles[i]);
        }

        return result;
    }
    
    private static string GetCsvPropertyName(PropertyInfo includedProperty)
    {
        if (TryGetAttribute(includedProperty, out CsvPropertyNameAttribute? nameAttribute))
        {
            return nameAttribute!.PropertyName;
        }

        return includedProperty.Name;
    }
    

    internal static string GetCsvString(IEnumerable<string> stringMembers)
    {
        return string.Join(';', stringMembers.Select(x => string.Concat("\"", x, "\"")));
    }
    
    
    internal static bool TryGetAttribute<TAttribute>(MemberInfo member, out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = member.GetCustomAttribute<TAttribute>();
        return attribute is not null;
    }
    
    internal static bool CollectionHasNull<T>(IEnumerable<T> collection)
        => !typeof(T).IsValueType && collection.Any(notnull => notnull is null);

}