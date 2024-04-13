using System.Diagnostics.CodeAnalysis;

namespace TelegramInteraction;

public sealed class ApiKey
{
    public string Key { get; }

    public override string ToString() => Key;

    public static bool TryLoadFrom(string? path, [NotNullWhen(true)] out ApiKey? apiKey)
    {
        apiKey = null;
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        
        try
        {
            var key = File.ReadAllText(path).Trim();
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            apiKey = new ApiKey(key);
            return true;
        }
        catch (IOException e)
        {
            return false;
        }
    }

    public bool TryLoadTo(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        
        try
        {
            File.WriteAllText(path, Key);
        }
        catch (IOException e)
        {
            return false;
        }

        return true;
    }
    
    public ApiKey(string key)
    {
        Key = key;
    }
}