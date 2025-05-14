using App;
using App.Scopes;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RegisterDependencyAdapterTests
    {

        public RegisterDependencyAdapterTests()
        {
            new InitCommand().Execute();
            var _scope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", _scope).Execute();
        }

        [Fact]
        public void Execute_ShouldRegisterGameAdapterStrategy()
        {
            // Arrange
            var strategy = new RegisterDependencyAdapter();

            // Act
            strategy.Execute();

            // Assert - проверяем через вызов стратегии, а не через IsRegistered
            var adapterFunc = Ioc.Resolve<Func<object[], object>>("Game.Adapter");
            Assert.NotNull(adapterFunc);
        }

        [Fact]
        public void GameAdapterStrategy_ShouldRegisterRequiredTypes()
        {
            // Arrange
            var strategy = new RegisterDependencyAdapter();

            // Act
            strategy.Execute();

            // Assert - получаем значения напрямую
            var interfaceType = Ioc.Resolve<Type>("Game.Adapter.InterfaceType");
            var typeName = Ioc.Resolve<string>("Game.Adapter.TypeName");

            Assert.Equal(typeof(IMoving), interfaceType);
            Assert.Equal("IMovingAdapter", typeName);
        }

        [Fact]
        public void GameAdapterStrategy_ShouldCreateValidAdapter()
        {

            new RegisterDependencyAdapter().Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Position.Get",
                (object[] args) => new Vec(new[] { 1, 2 })).Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Velocity.Get",
                (object[] args) => new Vec(new[] { 0, 1 })).Execute();

            var obj = new Dictionary<string, object>();

            // Act
            var adapter = Ioc.Resolve<IMoving>("Game.Adapter", typeof(IMoving), obj);

            // Assert
            Assert.Equal(new Vec(new[] { 1, 2 }), adapter.Position);
            Assert.Equal(new Vec(new[] { 0, 1 }), adapter.Velocity);

        }
    }
}
