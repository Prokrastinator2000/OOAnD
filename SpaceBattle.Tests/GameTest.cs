using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib;

public class GameTests
{

    public GameTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    [Fact]
    public void CorrectCommandExecution()
    {
        var mockQueue = new Mock<IQueue>();
        var mockCmd = new Mock<ICommand>();
        var mockFlag = new Mock<IStopFlag>();

        var RegCreateGame = new RegisterIoCDependencyCreateGame();
        RegCreateGame.Execute();

        mockQueue.SetupSequence(q => q.Take())
         .Returns(mockCmd.Object)
         .Returns(mockCmd.Object);
        mockFlag.SetupSequence(q => q.GetStopFlag())
         .Returns(true)
         .Returns(false);

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] _) => mockQueue.Object
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.StopFlag",
            (object[] _) => mockFlag.Object
        ).Execute();

        var iocScope = Ioc.Resolve<object>("IoC.Scope.Current");

        var game = Ioc.Resolve<ICommand>("Commands.CreateGame", iocScope);

        game.Execute();

        mockCmd.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void ExceptionThrowsTest()
    {
        var mockQueue = new Mock<IQueue>();
        var mockCmd = new Mock<ICommand>();
        var mockFlag = new Mock<IStopFlag>();

        var RegCreateGame = new RegisterIoCDependencyCreateGame();
        RegCreateGame.Execute();

        // mockQueue.SetupSequence(q => q.Take())
        //  .Returns(mockCmd.Object)
        //  .Returns(mockCmd.Object);
        mockFlag.SetupSequence(q => q.GetStopFlag())
         .Returns(true)
         .Returns(false);

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] _) => mockQueue.Object
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.StopFlag",
            (object[] _) => mockFlag.Object
        ).Execute();

        var exceptionCounter = 0;
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandle",
            (object[] obj) =>
            {
                exceptionCounter++;
                return new object();
            }
        ).Execute();

        var iocScope = Ioc.Resolve<object>("IoC.Scope.Current");

        var game = Ioc.Resolve<ICommand>("Commands.CreateGame", iocScope);

        game.Execute();

        Assert.Equal(1, exceptionCounter);
    }
}
