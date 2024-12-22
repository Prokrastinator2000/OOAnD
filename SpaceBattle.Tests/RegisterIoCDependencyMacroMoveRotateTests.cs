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
    public void Test_Register_MacroMoveRotate_Failure()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Move");
        Assert.Throws<Exception>(() => strategy.Resolve(new object[0]));
    }
    [Fact]
    public void TTest_Register_MacroMoveRotate_Success()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
        var cmd3 = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command1",
                            (object[] args) => cmd1.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command.Move",
                            (object[] args) => cmd2.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command.Rotate",
                            (object[] args) => cmd3.Object).Execute();

        var RegisterMacroCmd = new RegisterIoCDependencyMacroCommand();
        RegisterMacroCmd.Execute();

        IEnumerable<string> MacroTestDependencies = new List<string> { "Command.Move" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Move",
                            (object[] args) => MacroTestDependencies).Execute();
        var strategy = new CreateMacroCommandStrategy("Macro.Move");
        var macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Never);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Never);

        MacroTestDependencies = new List<string> { "Command.Rotate" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Rotate",
                            (object[] args) => MacroTestDependencies).Execute();
        strategy = new CreateMacroCommandStrategy("Macro.Rotate");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Never);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Once);

    }
}
