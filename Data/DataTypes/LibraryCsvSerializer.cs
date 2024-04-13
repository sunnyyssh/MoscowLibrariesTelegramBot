using CsvSerialization;

namespace Data;

public class LibraryCsvSerializer : ICsvSerializer<Library>
{
    private static readonly string[] Titles =
    {
        "ID",
        "LibraryName",
        "AdmArea",
        "District",
        "Address",
        "NumberOfAccessPoints",
        "WiFiName",
        "CoverageArea",
        "FunctionFlag",
        "AccessFlag",
        "Password",
        "Latitude_WGS84",
        "Longitude_WGS84",
        "global_id",
        "geodata_center",
        "geoarea"
    };

    private string TitleLine => string.Join(';', Titles);

    public string GetTitleLine()
    {
        return TitleLine;
    }

    public bool CheckTitleLine(string titleLine)
    {
        return titleLine == TitleLine;
    }

    public string Serialize(Library data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        
        string serialized = CsvSerializer.Serialize(data);

        return serialized;
    }

    public Library? Deserialize(string source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        try
        {
            return CsvSerializer.Deserialize<Library>(source);
        }
        catch (DeserializationException e)
        {
            return null;
        }
    }
}