using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyActionsStopTest
    {
        public RegisterIoCDependencyActionsStopTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void CorrectlyResolvesStopCommand()
        {
            
            var mockOrder = new Mock<IDictionary<string, object>>();
            mockOrder.Setup(o => o["Action"]).Returns("SomeAction");
            mockOrder.Setup(o => o["Key"]).Returns("someKey");

            var registerIoCDependencyActionsStop = new RegisterIoCDependencyActionsStop();

            
            registerIoCDependencyActionsStop.Execute();
            var resolvedCommand = Ioc.Resolve<ICommand>("Actions.Stop", mockOrder.Object);

            
            Assert.IsType<StopCommand>(resolvedCommand);
        }
    }
}
