using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class InjectableCommandTests
{
    [Fact]
    public void InjectableCommandTest()
    {
        var cmd = new Mock<ICommand>();
        var inject = new InjectableCommand();
        inject.Inject(cmd.Object);
        inject.Execute();
        cmd.Verify(i => i.Execute());
    }

    [Fact]
    public void CommandHaventInjected()
    {
        var inject = new InjectableCommand();
        Assert.Throws<InvalidOperationException>(() => inject.Execute());
    }
}
