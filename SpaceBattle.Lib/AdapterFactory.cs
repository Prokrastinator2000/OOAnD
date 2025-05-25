using App;

namespace SpaceBattle;
public class AdapterFactory : IAdapterFactory
{
    public object Create(IDictionary<string, object> obj)
    {
        var interfaceType = Ioc.Resolve<Type>("Game.Adapter.InterfaceType");
        var adapterTypeName = $"{interfaceType.FullName}Adapter";

        try
        {
            var adapter = Ioc.Resolve<object>(adapterTypeName);
            return adapter;
        }
        catch (Exception)
        {
            var generatedCode = Ioc.Resolve<string>(
                "Game.Reflection.GenerateAdapterCode",
                interfaceType,
                typeof(IDictionary<string, object>)
            ).Replace("\r\n", "\n").Trim();

            var compilationResult = Compiler.CompileGeneratedCode(
                generatedCode,
                interfaceType,
                typeof(IDictionary<string, object>)
            );

            var adapterType = compilationResult.GetType(adapterTypeName);
            var adapter = Activator.CreateInstance(adapterType!, obj)!;

            Ioc.Resolve<ICommand>("IoC.Register",
                adapterTypeName,
                (object[] args) => adapter).Execute();

            return adapter;
        }
    }
}
