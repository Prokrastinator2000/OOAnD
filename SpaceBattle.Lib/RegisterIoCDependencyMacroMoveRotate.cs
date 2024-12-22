using App;
namespace SpaceBattle.Lib;
public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        var CreateMacroMove = new CreateMacroCommandStrategy("Macro.Move");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Move",
                    (object[] args) => CreateMacroMove.Resolve(args)).Execute();

        CreateMacroMove = new CreateMacroCommandStrategy("Macro.Rotate");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Rotate",
                    (object[] args) => CreateMacroMove.Resolve(args)).Execute();
    }
}
