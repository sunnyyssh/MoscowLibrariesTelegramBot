namespace Data;

public interface ICsvSerializer<TDataElement>
{
    string GetTitleLine();

    bool CheckTitleLine(string titleLine);
    
    string Serialize(TDataElement data);

    TDataElement? Deserialize(string source);
}