using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyMacroMoveRotateTests
{
    public RegisterIoCDependencyMacroMoveRotateTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Test_Register_MacroMoveRotate_Success()
    {
        var cmdMove = new Mock<ICommand>();
        var cmdRotate = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register", "Command.Move", (object[] args) => cmdMove.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Command.Rotate", (object[] args) => cmdRotate.Object).Execute();

        var RegisterMacroCmd = new RegisterIoCDependencyMacroCommand();
        RegisterMacroCmd.Execute();

        var registerMacroCmd = new RegisterIoCDependencyMacroMoveRotate();
        registerMacroCmd.Execute();

        var macroDependenciesMove = new List<string> { "Command.Move" };
        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Macro.Move", (object[] args) => macroDependenciesMove).Execute();

        var strategyMove = new CreateMacroCommandStrategy("Macro.Move");
        var macroCommandMove = strategyMove.Resolve(new object[0]);
        macroCommandMove.Execute();

        cmdMove.Verify(c => c.Execute(), Times.Once);
        cmdRotate.Verify(c => c.Execute(), Times.Never);

        var macroDependenciesRotate = new List<string> { "Command.Rotate" };
        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Macro.Rotate", (object[] args) => macroDependenciesRotate).Execute();

        var strategyRotate = new CreateMacroCommandStrategy("Macro.Rotate");
        var macroCommandRotate = strategyRotate.Resolve(new object[0]);
        macroCommandRotate.Execute();

        cmdMove.Verify(c => c.Execute(), Times.Once);
        cmdRotate.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Test_Register_MacroMoveRotate_Failure()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Move");
        Assert.Throws<Exception>(() => strategy.Resolve(new object[0]));
    }

    [Fact]
    public void Test_Register_MacroMoveRotate_CommandNotFound()
    {
        var cmdMove = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register", "Command.Move", (object[] args) => cmdMove.Object).Execute();

        var registerMacroCmd = new RegisterIoCDependencyMacroMoveRotate();
        registerMacroCmd.Execute();

        var macroDependencies = new List<string> { "Command.Rotate" };
        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Macro.Rotate", (object[] args) => macroDependencies).Execute();

        var strategyRotate = new CreateMacroCommandStrategy("Macro.Rotate");
        Assert.Throws<Exception>(() => strategyRotate.Resolve(new object[0]));
    }
}
