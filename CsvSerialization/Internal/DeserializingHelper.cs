using System.Reflection;

namespace CsvSerialization;

internal static class DeserializingHelper
{
    [RecursiveMethodWith(nameof(SetProperties))]
    internal static void SetProperties(object obj, Type type, string[] bareCsvData)
    {
        PropertyInfo[] bareProperties = type.GetProperties();
        foreach (PropertyInfo property in bareProperties)
        {
            if (SerializingHelper.TryGetAttribute(property, out CsvIncludeAtAttribute? includeAtAttribute))
            {
                int position = includeAtAttribute!.Position;
                if (!SerializingHelper.IsPrimitiveType(property.PropertyType))
                    throw new PrimitiveTypeExpectedException(property.PropertyType);
                if (position >= bareCsvData.Length)
                    throw new CsvPropertiesCountException();
                if (position < 0)
                    throw new PositionsSerializationException();
                SetPrimitiveProperty(obj, property, bareCsvData[position]);
                continue;
            }

            if (SerializingHelper.TryGetAttribute(property, out CsvIncludeCsvTypeAtAttribute? csvTypeAtAttribute))
            {
                Type propType = property.PropertyType;
                if (!SerializingHelper.TryGetAttribute(propType, out CsvTypeAttribute _))
                    throw new NonSerializableTypeException(propType);
                int[] positions = csvTypeAtAttribute!.Positions;
                foreach (int position in positions)
                {
                    if (position >= bareCsvData.Length)
                        throw new CsvPropertiesCountException();
                    if (position < 0)
                        throw new PositionsSerializationException();
                }

                ConstructorInfo? constructor = propType.GetConstructor(Array.Empty<Type>());
                if (constructor is null)
                    throw new ParameterlessConstructorNotFoundException(propType);
                object propInstance = constructor.Invoke(null);
                string[] embeddedCsvData = positions.Select(i => bareCsvData[i]).ToArray();
                SetProperties(propInstance, propType, embeddedCsvData);
                property.SetValue(obj, propInstance);
            }
            
        }
    }

    private static void SetPrimitiveProperty(object obj, PropertyInfo property, string notParsed)
    {
        object value;
        try
        {
            value = ParsePrimitiveValue(property.PropertyType, notParsed)!;
        }
        catch (FormatException e)
        {
            throw new PropertyIncorrectFormatException(notParsed, property.Name);
        }
        property.SetValue(obj, value);
    }

    private static object? ParsePrimitiveValue(Type type, string csvValue)
    {
        if (type == typeof(int))
            return int.Parse(csvValue);
        if (type == typeof(long))
            return long.Parse(csvValue);
        if (type == typeof(byte))
            return byte.Parse(csvValue);
        if (type == typeof(DateTime))
            return DateTime.Parse(csvValue);
        if (type == typeof(string))
            return csvValue;
        if (type == typeof(double))
            return double.Parse(csvValue.Replace('.', ','));
        if (type == typeof(float))
            return float.Parse(csvValue.Replace('.', ','));
        return null;
    }
    
    internal static void ThrowIfTitlesIncorrect(Type csvType, string titlesString)
    {
        var splittedTitles = SplitCsvString(titlesString);
        var mustBeTitles = SerializingHelper.GetTitles(csvType);
        if (splittedTitles.Length != mustBeTitles.Length)
        {
            throw new WrongCsvTitlesException();
        }
        for (int i = 0; i < splittedTitles.Length; i++)
        {
            if (splittedTitles[i] != mustBeTitles[i])
            {
                throw new WrongCsvTitlesException(splittedTitles[i]);
            }
        }
    }
    
    internal static string[] SplitCsvString(string csvString)
    {
        csvString = csvString.Trim(';', ' ');
        bool isOpenedBracket = false;
        int lastSemicolon = -1;
        bool wasBrackets = false;
        List<string> splitted = new List<string>();
        for (int i = 0; i < csvString.Length; i++)
        {
            if (csvString[i] == '"')
            {
                isOpenedBracket = !isOpenedBracket;
                wasBrackets = !isOpenedBracket;
                continue;
            }
            if (csvString[i] == ';' && !isOpenedBracket)
            {
                Range range = new Range(lastSemicolon + (wasBrackets ? 2 : 1), i - (wasBrackets ? 1 : 0));
                splitted.Add(csvString[range]);
                lastSemicolon = i;
                wasBrackets = false;
            }
        }
        Range nrange = new Range(lastSemicolon + (wasBrackets ? 2 : 1), csvString.Length - (wasBrackets ? 1 : 0));
        splitted.Add(csvString[nrange]);

        return splitted.ToArray();
    }
}