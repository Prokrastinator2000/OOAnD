using App;
using App.Scopes;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RegisterDependencyAdapterRegisterTests
    {
        private readonly object _scope;

        public RegisterDependencyAdapterRegisterTests()
        {
            new InitCommand().Execute();
            _scope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", _scope).Execute();

            // Регистрируем зависимости в родительском скоупе
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Reflection.GenerateAdapterCode", (object[] args) =>
            {
                var builder = new AdapterBuilder((Type)args[0], (Type)args[1]);
                foreach (var prop in ((Type)args[0]).GetProperties())
                {
                    builder.CreateProperty(prop);
                }

                return builder.Build();
            }).Execute();
        }

        [Fact]
        public void Execute_ShouldRegisterGameAdapterRegisterStrategy()
        {
            // Arrange
            var strategy = new RegisterDependencyAdapterRegister();

            // Act
            strategy.Execute();

            // Assert - проверяем через вызов стратегии
            var registerFunc = Ioc.Resolve<Func<object[], object>>("Game.Adapter.Register");
            Assert.NotNull(registerFunc);
        }

        [Fact]
        public void GameAdapterRegisterStrategy_ShouldGenerateValidCode()
        {
            // Arrange

            new RegisterDependencyAdapterRegister().Execute();

            // Act
            var code = Ioc.Resolve<string>("Game.Adapter.Register",
                typeof(IMoving),
                typeof(Dictionary<string, object>));

            // Assert
            Assert.Contains("class IMovingAdapter : IMoving", code);
            Assert.Contains("public Vec Position", code);
            Assert.Contains("public Vec Velocity", code);

        }

        [Fact]
        public void GameAdapterRegisterStrategy_ShouldCompileSuccessfully()
        {

            new RegisterDependencyAdapterRegister().Execute();

            // Act
            var code = Ioc.Resolve<string>("Game.Adapter.Register",
                typeof(IMoving),
                typeof(Dictionary<string, object>));

            var compilationResult = Compiler.CompileGeneratedCode(
                code,
                typeof(IMoving),
                typeof(Dictionary<string, object>));

            // Assert
            var adapterType = compilationResult.GetType("SpaceBattle.Lib.IMovingAdapter");
            Assert.NotNull(adapterType);
            Assert.True(typeof(IMoving).IsAssignableFrom(adapterType));

        }
    }
}
