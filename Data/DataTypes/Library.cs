using System.Text.Json.Serialization;
using CsvSerialization;

namespace Data;

[CsvType(typeof(Library))]
[JsonSerializable(typeof(Library))]
public partial class Library
{
    private readonly string? _libraryName;
    private readonly string? _admArea;
    private readonly string? _district;
    private readonly string? _address;
    private readonly string? _wiFiName;
    private readonly string? _functionFlag;
    private readonly string? _accessFlag;
    private readonly string? _password;
    private readonly string? _geoDataCenter;
    private readonly string? _geoArea;

    [JsonPropertyName("ID")]
    [CsvPropertyName("ID")]
    [CsvIncludeAt(0)]
    public int Id { get; init; }

    [JsonPropertyName("LibraryName")]
    [CsvPropertyName("LibraryName")]
    [CsvIncludeAt(1)]
    public string? LibraryName
    {
        get => _libraryName;
        init => _libraryName = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("AdmArea")]
    [CsvPropertyName("AdmArea")]
    [CsvIncludeAt(2)]
    public string? AdmArea
    {
        get => _admArea;
        init => _admArea = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("District")]
    [CsvPropertyName("District")]
    [CsvIncludeAt(3)]
    public string? District
    {
        get => _district;
        init => _district = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("Address")]
    [CsvPropertyName("Address")]
    [CsvIncludeAt(4)]
    public string? Address
    {
        get => _address;
        init => _address = string.IsNullOrEmpty(value) ? null : value;
    }
    
    [JsonPropertyName("NumberOfAccessPoints")]
    [CsvPropertyName("NumberOfAccessPoints")]
    [CsvIncludeAt(5)]
    public int NumberOfAccessPoints { get; init; }

    [JsonPropertyName("WiFiName")]
    [CsvPropertyName("WiFiName")]
    [CsvIncludeAt(6)]
    public string? WiFiName
    {
        get => _wiFiName;
        init => _wiFiName = string.IsNullOrEmpty(value) ? null : value;
    }
    
    [JsonPropertyName("CoverageArea")]
    [CsvPropertyName("CoverageArea")]
    [CsvIncludeAt(7)]
    public double CoverageArea { get; init; }

    [JsonPropertyName("FunctionFlag")]
    [CsvPropertyName("FunctionFlag")]
    [CsvIncludeAt(8)]
    public string? FunctionFlag
    {
        get => _functionFlag;
        init => _functionFlag = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("AccessFlag")]
    [CsvPropertyName("AccessFlag")]
    [CsvIncludeAt(9)]
    public string? AccessFlag
    {
        get => _accessFlag;
        init => _accessFlag = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("Password")]
    [CsvPropertyName("Password")]
    [CsvIncludeAt(10)]
    public string? Password
    {
        get => _password;
        init => _password = string.IsNullOrEmpty(value) ? null : value;
    }
    
    [JsonPropertyName("Latitude_WGS84")]
    [CsvPropertyName("Latitude_WGS84")]
    [CsvIncludeAt(11)]
    public double Latitude { get; init; }
    
    [JsonPropertyName("Longitude_WGS84")]
    [CsvPropertyName("Longitude_WGS84")]
    [CsvIncludeAt(12)]
    public double Longitude { get; init; }
    
    [JsonPropertyName("global_id")]
    [CsvPropertyName("global_id")]
    [CsvIncludeAt(13)]
    public long GlobalId { get; init; }

    [JsonPropertyName("geodata_center")]
    [CsvPropertyName("geodata_center")]
    [CsvIncludeAt(14)]
    public string? GeoDataCenter
    {
        get => _geoDataCenter;
        init => _geoDataCenter = string.IsNullOrEmpty(value) ? null : value;
    }

    [JsonPropertyName("geoarea")]
    [CsvPropertyName("geoarea")]
    [CsvIncludeAt(15)]
    public string? GeoArea
    {
        get => _geoArea;
        init => _geoArea = string.IsNullOrEmpty(value) ? null : value;
    }
}