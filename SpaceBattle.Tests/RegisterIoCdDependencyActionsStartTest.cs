using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyActionsStartTest
    {
        public RegisterIoCDependencyActionsStartTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void CorrectlyResolvesStartCommand()
        {

            var mockOrder = new Mock<IDictionary<string, object>>();
            mockOrder.Setup(o => o["Action"]).Returns("SomeAction");
            mockOrder.Setup(o => o["Args"]).Returns(new object[] { });
            mockOrder.Setup(o => o["Key"]).Returns("someKey");

            var registerIoCDependencyActionsStart = new RegisterIoCDependencyActionsStart();

            registerIoCDependencyActionsStart.Execute();
            var resolvedCommand = Ioc.Resolve<ICommand>("Actions.Start", mockOrder.Object);

            Assert.IsType<StartCommand>(resolvedCommand);
        }
    }
}
