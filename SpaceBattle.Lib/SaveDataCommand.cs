using App;

namespace SpaceBattle.Lib;

public class SaveDataCommand : ICommand
{
    private readonly IList<int[]> collisionData;
    private readonly string collisionName;

    public SaveDataCommand(string collisionName, IList<int[]> collisionData)
    {
        this.collisionData = collisionData;
        this.collisionName = collisionName;
    }
    public void Execute()
    {
        var pathForCollisionFile = Ioc.Resolve<string>("Data.CollisionFilesPath");
        Ioc.Resolve<ICommand>("Commands.WriteToFile", pathForCollisionFile + collisionName, collisionData).Execute();
        Ioc.Resolve<ICommand>("Collision.LoadDataToMemory", collisionName, collisionData).Execute();
    }
}
