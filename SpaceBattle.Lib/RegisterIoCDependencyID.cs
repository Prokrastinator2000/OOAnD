using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyUuidGenerate : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Object.id.Generate",
            (object[] args) => Guid.NewGuid().ToString()
        ).Execute();
    }
}
