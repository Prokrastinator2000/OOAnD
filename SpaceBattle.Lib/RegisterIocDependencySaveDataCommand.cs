using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionDataSaveCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.SaveData",
                (object[] arg) => new SaveDataCommand((string)arg[0], (IList<int[]>)arg[1])).Execute();
    }
}
