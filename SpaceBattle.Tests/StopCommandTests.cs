using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class StopCommandTests
    {
        public StopCommandTests()
        {
            new InitCommand().Execute();
            var scope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", scope).Execute();
        }

        [Fact]
        public void StopCommand_Should_Inject_Cancel_Command()
        {

            var mockInjectable = new Mock<ICommandInjectable>();
            var mockCancel = new Mock<ICommand>();
            var mockState = new Dictionary<string, object>();

            mockState.Add("TestAction", mockInjectable.Object);

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Object", (object[] args) => mockState).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Injectable", (object[] args) => mockInjectable.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Empty", (object[] args) => mockCancel.Object).Execute();

            var order = new Mock<IDictionary<string, object>>();
            order.Setup(m => m["Action"]).Returns("TestAction");
            order.Setup(m => m["Key"]).Returns("Test");

            var stopAction = new StopCommand(order.Object);
            stopAction.Execute();

            mockInjectable.Verify(i => i.Inject(mockCancel.Object), Times.Once);
        }
    }
}
