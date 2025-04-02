using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyAddGameObjectToRepository : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Object.Add",
            (object[] args) =>
            new AddGameObjectCommand((string)args[0], args[1])).Execute();
    }
}
