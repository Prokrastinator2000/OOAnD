using App;

namespace SpaceBattle.Lib;

public class AddGameObjectCommand : ICommand
{
    private readonly object AddedObject;
    private readonly string uuid;
    private readonly IDictionary<string, object> repository;
    public AddGameObjectCommand(string uuid, object AddedObject, IDictionary<string, object> repository)
    {
        this.uuid = uuid;
        this.AddedObject = AddedObject;
        this.repository = repository;
    }
    public void Execute()
    {
        repository.Add(uuid, AddedObject);
    }
}
