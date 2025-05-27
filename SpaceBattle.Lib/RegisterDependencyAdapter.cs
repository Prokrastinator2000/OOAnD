using App;

namespace SpaceBattle.Lib
{
    public class RegisterDependencyAdapter : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) =>
{
    var interfaceType = (Type)args[0];
    var obj = (IDictionary<string, object>)args[1];

    var adapterTypeName = $"{interfaceType.FullName}Adapter";

    try
    {
        return Ioc.Resolve<object>(adapterTypeName);
    }
    catch
    {
        var generatedCode = (string)Ioc.Resolve<object>(
            "Game.Reflection.GenerateAdapterCode",
            interfaceType,
            typeof(IDictionary<string, object>)
        );

        generatedCode = generatedCode.Replace("\r\n", "\n").Trim();

        var compilationResult = Compiler.CompileGeneratedCode(
            generatedCode,
            interfaceType,
            typeof(IDictionary<string, object>)
        );

        var adapterType = compilationResult.GetType(adapterTypeName);
        var adapter = Activator.CreateInstance(adapterType!, obj)!;

        Ioc.Resolve<ICommand>("IoC.Register", adapterTypeName, (object[] a) => adapter).Execute();

        return adapter;
    }
}).Execute();

        }
    }
}
