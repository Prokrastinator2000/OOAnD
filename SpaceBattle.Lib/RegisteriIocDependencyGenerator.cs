using App;
namespace SpaceBattle.Lib;

public class RegisterIocDependencyGenerator : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Reflection.GenerateAdapterCode", (object[] args) =>
        {
            var adapterType = (Type)args[0];
            var targetType = (Type)args[1];

            var builder = new AdapterBuilder(adapterType, targetType);

            foreach (var prop in adapterType.GetProperties())
            {
                builder.CreateProperty(prop);
            }

            return builder.Build();
        }).Execute();
    }
}
