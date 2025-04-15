using System.Text.Json;
using App;
namespace SpaceBattle.Lib;

public class WriteToFileCommand : ICommand
{
    private readonly object objectToWrite;
    private readonly string filePath;

    public WriteToFileCommand(string filePath, object objectToWrite)
    {
        this.objectToWrite = objectToWrite;
        this.filePath = filePath;
    }

    public void Execute()
    {
        var jsonString = JsonSerializer.Serialize(objectToWrite);
        File.WriteAllText(filePath, jsonString);
    }
}
