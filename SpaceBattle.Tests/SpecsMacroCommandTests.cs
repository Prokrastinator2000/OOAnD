using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class SpecsMacroCommandTests
{
    [Fact]
    public void Test_Resolve_MacroCommand_Success()
    {
        IEnumerable<string> CmdNames = new List<string> { "Command1", "Command2", "Command.Move", "Commands.Rotate" };

        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
        var cmd3 = new Mock<ICommand>();
        var cmd4 = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command1",
                            (object[] args) => cmd1.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command2",
                            (object[] args) => cmd2.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Move",
                            (object[] args) => cmd3.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Rotate",
                            (object[] args) => cmd4.Object).Execute();

        var strategy = new CreateMacroCommandStrategy("Macro.Test");
        var macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Once);
        cmd4.Verify(c => c.Execute(), Times.Once);

        strategy = new CreateMacroCommandStrategy("Specs.Move");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Exactly(2));
        cmd4.Verify(c => c.Execute(), Times.Once);

        strategy = new CreateMacroCommandStrategy("Specs.Rotate");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Exactly(2));
        cmd4.Verify(c => c.Execute(), Times.Exactly(2));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Extended");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(new object[0]));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_UnknownStrategy()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Ex");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(new object[0]));
    }
}
