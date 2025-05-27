
using App;
using App.Scopes;
using SpaceBattle.Lib;
namespace SpaceBattle.Tests
{
    public class GameAdapterCreateTests
    {
        public GameAdapterCreateTests()
        {

            new InitCommand().Execute();
            var scope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", scope).Execute();

            var adapterRegister = new RegisterDependencyAdapter();
            adapterRegister.Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter.InterfaceType", (object[] args) => typeof(IMoving)).Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Reflection.GenerateAdapterCode", (object[] args) =>
            {
                return @"
                namespace SpaceBattle.Lib
                {
                    public class IMovingAdapter : IMoving
                    {
                        private System.Collections.Generic.IDictionary<string, object> _data;
                        public IMovingAdapter(System.Collections.Generic.IDictionary<string, object> data)
                        {
                            _data = data;
                        }
                        public Vec Position
                        {
                            get => (Vec)_data[""Position""];
                            set => _data[""Position""] = value;
                        }
                        public Vec Velocity
                        {
                            get => (Vec)_data[""Velocity""];
                        }
                    }
                }";
            }).Execute();
        }

        [Fact]
        public void Adapter_Is_Created_And_Registered_If_Not_Exists()
        {

            var targetDict = new Dictionary<string, object>
            {
                ["Position"] = new Vec(new[] { 1, 2 }),
                ["Velocity"] = new Vec(new[] { 3, 4 })
            };

            var factory = new AdapterFactory();

            var adapter = factory.Create(targetDict) as IMoving;

            Assert.NotNull(adapter);
            Assert.Equal(new Vec(new[] { 1, 2 }), adapter.Position);
            Assert.Equal(new Vec(new[] { 3, 4 }), adapter.Velocity);

            var newPosition = new Vec(new[] { 10, 20 });
            adapter.Position = newPosition;
            Assert.Equal(newPosition, adapter.Position);
            Assert.Equal(newPosition, targetDict["Position"]);
        }
    }
}
