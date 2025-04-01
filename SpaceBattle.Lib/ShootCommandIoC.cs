using App;
namespace SpaceBattle.Lib;
public class ShootCommandIoC : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Commands.ShootCommand",
            (object[] args) => new ShootCommand((IShooting)args[0])
        ).Execute();
    }
}
