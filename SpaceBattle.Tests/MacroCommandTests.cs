using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class MacroCommandTests
{
    [Fact]
    public void Execute_AllCommandsExecuted()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
        ICommand[] list = [cmd1.Object, cmd2.Object];
        var MacroCommand = new MacroCommand(list);
        MacroCommand.Execute();

        cmd1.Verify(x => x.Execute());
        cmd2.Verify(x => x.Execute());
    }

    [Fact]
    public void Execute_Exception()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();

        cmd1.Setup(x => x.Execute()).Throws(new Exception());

        ICommand[] list = [cmd1.Object, cmd2.Object];

        var MacroCommand = new MacroCommand(list);

        Assert.Throws<Exception>(() => MacroCommand.Execute());
        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Never);
    }
}
