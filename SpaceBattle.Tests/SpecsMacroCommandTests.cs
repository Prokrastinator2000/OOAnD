using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class SpecsMacroCommandTests
{
    public SpecsMacroCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    [Fact]
    public void Test_Resolve_MacroCommand_Success()
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

        var registerMc = new RegisterIoCDependencyMacroCommand();
        registerMc.Execute();

        IEnumerable<string> MacroTestDependencies = new List<string> { "Command1", "Command.Move", "Command.Rotate" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Test",
                            (object[] args) => MacroTestDependencies).Execute();

        var strategy = new CreateMacroCommandStrategy("Macro.Test");
        var macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Once);

        MacroTestDependencies = new List<string> { "Command.Move" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Move",
                            (object[] args) => MacroTestDependencies).Execute();
        strategy = new CreateMacroCommandStrategy("Macro.Move");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Exactly(2));
        cmd3.Verify(c => c.Execute(), Times.Once);

        MacroTestDependencies = new List<string> { "Command.Rotate" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Rotate",
                            (object[] args) => MacroTestDependencies).Execute();
        strategy = new CreateMacroCommandStrategy("Macro.Rotate");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Exactly(2));
        cmd3.Verify(c => c.Execute(), Times.Exactly(2));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        IEnumerable<string> MacroTestDependencies = new List<string> { "Command1", "Command.Move", "Command.Rotate", "Command3" };
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Specs.Macro.Extended",
                            (object[] args) => MacroTestDependencies).Execute();

        var strategy = new CreateMacroCommandStrategy("Macro.Extended");

        Assert.Throws<Exception>(() => strategy.Resolve(new object[0]));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_UnknownStrategy()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Ex");

        Assert.Throws<Exception>(() => strategy.Resolve(new object[0]));
    }
}
