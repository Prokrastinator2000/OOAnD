using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyGameObjectRepository : ICommand
{
    public void Execute()
    {
        var gameItems = new Dictionary<string, object>();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Object.Repository",
            (object[] _) => gameItems
        ).Execute();
    }
}
