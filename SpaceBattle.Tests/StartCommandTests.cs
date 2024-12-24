using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class StartCommandTest
    {
        [Fact]
        public void StartCommandExecutionTest()
        {
            
            new InitCommand().Execute();
            var scope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", scope).Execute();

            var mockAction = new Mock<ICommand>();
            var mockConfigurable = new Mock<ICommand>().As<ICommandInjectable>();
            var mockReceiver = new Mock<ICommandReceiver>();
            var mockSender = new Mock<ICommand>();
            var mockState = new Mock<IDictionary<string, object>>();

            Ioc.Resolve<ICommand>("IoC.Register", "Macro.MoveAction", (object[] args) => mockAction.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Macro", (object[] args) => mockAction.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.CommandInjectable", (object[] args) => mockConfigurable.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Queue", (object[] args) => mockReceiver.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Send", (object[] args) => mockSender.Object).Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Object", (object[] args) => mockState.Object).Execute();

            var order = new Mock<IDictionary<string, object>>();
            order.Setup(o => o["Action"]).Returns("MoveAction");
            order.Setup(o => o["Args"]).Returns(new object[] { });
            order.Setup(o => o["Key"]).Returns("someKey");

            
            var command = new StartCommand(order.Object);
            command.Execute();

            
            mockConfigurable.Verify(c => c.Inject(It.IsAny<ICommand>()), Times.Once);
            mockSender.Verify(c => c.Execute(), Times.Once);
        }
    }
}
