using App;
namespace SpaceBattle.Lib;
public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        RegisterIoCDepedencyMacroByStrategy("Move");
        RegisterIoCDepedencyMacroByStrategy("Rotate");
    }
    private void RegisterIoCDepedencyMacroByStrategy(string StrategyName)
    {
        var CreateMacroMove = new CreateMacroCommandStrategy(StrategyName);
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro." + StrategyName,
                    (object[] args) => CreateMacroMove.Resolve(args)).Execute();

    }
}
