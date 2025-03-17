using App;

namespace SpaceBattle.Lib;

public class AddGameObjectCommand : ICommand
{
    private readonly object AddedObject;
    private readonly string uuid;
    public AddGameObjectCommand(string uuid, object AddedObject)
    {
        this.uuid = uuid;
        this.AddedObject = AddedObject;
    }
    public void Execute()
    {
        Ioc.Resolve<IDictionary<string, object>>("Game.Object.Repository").Add(uuid, AddedObject);
    }
}
