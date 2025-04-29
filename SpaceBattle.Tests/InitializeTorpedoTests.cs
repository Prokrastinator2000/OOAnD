using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib.Tests;
public class ShootCommandTest
{
    public ShootCommandTest()
    {
        new InitCommand().Execute();
    }
    [Fact]
    public void ShootCommand()
    {
        var cmdMove = new Mock<ICommand>();
        var MockAdd = new Mock<ICommand>();

        var initializetorpedoRegister = new InitializeTorpedoIoC();
        initializetorpedoRegister.Execute();

        var ShootCommandRegister = new ShootCommandIoC();
        ShootCommandRegister.Execute();

        var IShootingMock = new Mock<IShooting>();
        IShootingMock.SetupGet(m => m.Position).Returns(new Vec(new int[] { 12, 5 }));
        IShootingMock.SetupGet(m => m.Velocity).Returns(new Vec(new int[] { -2, 3 }));
        IShootingMock.SetupGet(m => m.Impulse).Returns(new Vec(new int[] { -3, 5 }));

        var Torpedo = new Dictionary<string, object> { { "Position", new Vec(new int[] { 0, 0 }) }, { "Velocity", new Vec(new int[] { 0, 0 }) }, { "id", "123" } };

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Objects.GetTorpedo", (object[] args) => Torpedo).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => cmdMove.Object).Execute();

        var ShootCommand = Ioc.Resolve<ICommand>("Game.Commands.ShootCommand", IShootingMock.Object);
        ShootCommand.Execute();

        Assert.Equal(new Vec(new int[] { 7, 13 }), Torpedo["Position"]);
        Assert.Equal(new Vec(new int[] { -2, 3 }), Torpedo["Velocity"]);
        cmdMove.Verify(cmd => cmd.Execute(), Times.Exactly(1));

    }
}
