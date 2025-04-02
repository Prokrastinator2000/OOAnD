using App;
namespace SpaceBattle.Lib;
public class InitializeTorpedoIoC : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Commands.InitializeTorpedo",
            (object[] args) => new InitializeTorpedoCommand((IDictionary<string, object>)args[0], (IShooting)args[1])
        ).Execute();
    }
}
