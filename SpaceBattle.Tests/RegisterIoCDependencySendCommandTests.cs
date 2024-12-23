using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencySendCommandTests
{
    public RegisterIoCDependencySendCommandTests()
    {
        new InitCommand().Execute();
        var IoCScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", IoCScope).Execute();
    }
    [Fact]
    public void ExecuteTest()
    {
        var rec = new Mock<ICommandReceiver>();
        var cmd = new Mock<ICommand>();

        var registerSend = new RegisterIoCDependencySendCommand();
        registerSend.Execute();

        var sendcommand = Ioc.Resolve<ICommand>("Commands.Send", cmd.Object, rec.Object);

        Assert.IsType<SendCommand>(sendcommand);

    }
}
