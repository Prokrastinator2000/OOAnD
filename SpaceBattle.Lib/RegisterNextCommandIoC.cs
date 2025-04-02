using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyNextCommand : ICommand
{
    public void Execute()
    {
        var q = Ioc.Resolve<IQueue>("Game.Queue");

        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.GetNextCommand",
                (object[] _) => q.Take()).Execute();
    }
}
