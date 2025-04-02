using App;

namespace SpaceBattle.Lib;

public class RemoveGameObjectCommand : ICommand
{
    private readonly string uuid;
    public RemoveGameObjectCommand(string uuid)
    {
        this.uuid = uuid;
    }
    public void Execute()
    {
        Ioc.Resolve<IDictionary<string, object>>("Game.Object.Repository").Remove(uuid);
    }
}
