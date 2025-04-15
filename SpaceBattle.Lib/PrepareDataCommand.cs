using App;

namespace SpaceBattle.Lib;

public class PrepareDataCommand : ICommand
{
    private readonly ICollisionDataGenerator dataGenerator;
    public PrepareDataCommand(ICollisionDataGenerator dataGenerator)
    {
        this.dataGenerator = dataGenerator;
    }

    public void Execute()
    {
        var collisionName = Ioc.Resolve<string>("Collision.GetCollisionName", dataGenerator.FirstShape, dataGenerator.SecondShape);
        var collisionDataSaveCommand = Ioc.Resolve<ICommand>("Commands.SaveData", collisionName, dataGenerator.GenerateCollisionData());
        collisionDataSaveCommand.Execute();
    }
}
