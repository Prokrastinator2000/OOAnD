using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RegisterIoCDependencyRotateCommandTests
    {
        public RegisterIoCDependencyRotateCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_Should_Register_RotateCommand_Dependency()
        {
            var mock_reg_rotate = new Mock<RegisterIoCDependencyRotateCommand>();
            var reg_rotate = mock_reg_rotate.Object;
            reg_rotate.Execute();

            var mock_rotating = new Mock<IRotating>();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IRotatingObject", (object[] args) => mock_rotating.Object).Execute();

            var res = Ioc.Resolve<ICommand>("Commands.Rotate");

            Assert.IsType<RotateCommand>(res);
        }
    }
}
