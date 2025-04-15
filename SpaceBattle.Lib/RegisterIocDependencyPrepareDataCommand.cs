using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionPrepareData : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.PrepareData",
                (object[] arg) => new PrepareDataCommand((ICollisionDataGenerator)arg[0])).Execute();
    }
}
