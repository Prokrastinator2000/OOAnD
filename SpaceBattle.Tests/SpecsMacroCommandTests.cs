using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class MacroCommandTests
{
    [Fact]
    public void Test_Resolve_MacroCommand_Success()
    {
        IEnumerable<string> CmdNames = new List<string> { "Command1", "Command2", "Command.Move", "Commands.Rotate" };

        var cmd1 = new Mock<Command1>();
        var cmd2 = new Mock<Command2>();
        var cmd3 = new Mock<CommandMove>();
        var cmd4 = new Mock<CommandRotate>();

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

        ICommand[] list = [cmd1.Object, cmd2.Object, cmd3.Object, cmd4.Object];
        var SourceMacroCommand = new MacroCommand(list);

        var strategy = new CreateMacroCommandStrategy("Macro.Test");

        var macroCommand = strategy.Resolve();

        macroCommand.Execute();

        cmd1.Verify(x => x.Execute());
        cmd2.Verify(x => x.Execute());
        cmd3.Verify(x => x.Execute());
        cmd4.Verify(x => x.Execute());
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Extended");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve());
    }
}
