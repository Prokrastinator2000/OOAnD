using App;
namespace SpaceBattle.Lib;
public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        var CreateMacro = new CreateMacroCommandStrategy("Macro.Move");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Move",
                    (object[] args) => CreateMacro.Resolve(args)).Execute();

        CreateMacro = new CreateMacroCommandStrategy("Macro.Rotate");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Rotate",
                    (object[] args) => CreateMacro.Resolve(args)).Execute();
    }
}
