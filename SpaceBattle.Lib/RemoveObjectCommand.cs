using App;

namespace SpaceBattle.Lib;

public class RemoveGameObjectCommand : ICommand
{
    private readonly string uuid;
    private readonly IDictionary<string, object> repository;
    public RemoveGameObjectCommand(string uuid, IDictionary<string, object> repository)
    {
        this.uuid = uuid;

        this.repository = repository;
    }
    public void Execute()
    {
        repository.Remove(uuid);
    }
}
