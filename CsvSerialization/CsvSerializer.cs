namespace CsvSerialization;

// It fully copied from my past homework.
// https://edu.hse.ru/pluginfile.php/3100598/assignsubmission_file/submission_files/1059098/19_Bagaviev_Control_Homework_3.zip?forcedownload=1
public static class CsvSerializer 
{
    public static string[] Serialize<T>(T[] objects)
    {
        ArgumentNullException.ThrowIfNull(objects, nameof(objects));
        if (SerializingHelper.CollectionHasNull(objects))
        {
            throw new ArgumentException("The collection has null value.");
        }


        List<string> result = new List<string>();
        Type type = typeof(T);
        var titles = SerializingHelper.GetTitles(type);
        var titleString = SerializingHelper.GetCsvString(titles);
        result.Add(titleString);

        foreach (T obj in objects)
        {
            var csvValues = SerializingHelper.GetCsvValues(type, obj!);
            string csvString = SerializingHelper.GetCsvString(csvValues);
            result.Add(csvString);
        }

        return result.ToArray();
    }

    public static string Serialize<T>(T obj)
    {
        Type type = typeof(T);
        
        var csvValues = SerializingHelper.GetCsvValues(type, obj!);
        string csvString = SerializingHelper.GetCsvString(csvValues);

        return csvString;
    }
    
    public static T[] Deserialize<T>(string[] csvStrings, bool firstStringIsTitle = true)
        where T : new()
    {
        ArgumentNullException.ThrowIfNull(csvStrings, nameof(csvStrings));
        if (SerializingHelper.CollectionHasNull(csvStrings))
            throw new ArgumentException("String array had null reference.", nameof(csvStrings));

        if (csvStrings.Length == 0)
            return Array.Empty<T>();
        
        Type type = typeof(T);
        if (firstStringIsTitle)
            DeserializingHelper.ThrowIfTitlesIncorrect(type, csvStrings[0]);

        return csvStrings.Skip(firstStringIsTitle ? 1 : 0).Select(Deserialize<T>).ToArray();

    }

    public static T Deserialize<T>(string csvString)
        where T : new()
    {
        ArgumentNullException.ThrowIfNull(csvString, nameof(csvString));
        string[] splitted = DeserializingHelper.SplitCsvString(csvString);
        T result = new T();
        DeserializingHelper.SetProperties(result, typeof(T), splitted);
        return result;
    }
}