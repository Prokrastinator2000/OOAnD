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
            {
                var uuid = Ioc.Resolve<string>("Uuid.Generate");

                ((IDictionary<string, object>)Ioc.Resolve<object>("Game.Object.Repository")).Add(uuid, args[0]);

                return uuid;
            }
        ).Execute();
    }
}
