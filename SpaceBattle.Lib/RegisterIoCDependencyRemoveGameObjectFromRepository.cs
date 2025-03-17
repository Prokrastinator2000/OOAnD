using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyRemoveGameObjectFromRepository : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
           "IoC.Register",
           "Game.Object.Remove",
           (object[] args) => new RemoveGameObjectCommand((string)args[0])
       ).Execute();
    }
}
