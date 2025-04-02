using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCreateGame : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.CreateGame",
                (object[] obj) => new Game(obj[0])).Execute();
    }
}
