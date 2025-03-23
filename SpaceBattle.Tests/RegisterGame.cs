using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCreateGameTest
{
    [Fact]
    public void CorrectResolveTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        var mockQueue = new Mock<IQueue>();
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] _) => mockQueue.Object).Execute();

        var registerIoCDependencyCreateGame = new RegisterIoCDependencyCreateGame();
        registerIoCDependencyCreateGame.Execute();
        var res = Ioc.Resolve<ICommand>("Commands.CreateGame", iocScope);

        Assert.IsType<Game>(res);
    }
}
