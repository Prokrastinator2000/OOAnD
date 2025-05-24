using App;

namespace SpaceBattle;
public class AdapterFactory : IAdapterFactory
{
    public object Create(IDictionary<string, object> obj)
    {
        var interfaceType = Ioc.Resolve<Type>("Game.Adapter.InterfaceType");
        // Возможно стоит изменить и вместо вытаскивания interfaceType из Ioc
        // стоит его получать на вход?
        var adapterTypeName = $"{interfaceType.FullName}Adapter";
        var generatedIMovingCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            interfaceType,
            typeof(IDictionary<string, object>)
        ).Replace("\r\n", "\n").Trim();

        var compilationResult = Compiler.CompileGeneratedCode(generatedIMovingCode,
        interfaceType,
        typeof(IDictionary<string, object>));

        var adapterType = compilationResult.GetType(adapterTypeName);

        return Activator.CreateInstance(adapterType!, obj)!;
    }
}
