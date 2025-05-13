using App;
using SpaceBattle.Lib;

namespace SpaceBattle;
public class MovingObjectAdapterFactory : IAdapterFactory
{
    public object Create(IDictionary<string, object> obj)
    {
        var generatedIMovingCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            typeof(IMoving),
            typeof(IDictionary<string, object>)
        ).Replace("\r\n", "\n").Trim();

        var compilationResult = Compiler.CompileGeneratedCode(generatedIMovingCode,
        typeof(IMoving),
        typeof(IDictionary<string, object>));

        var adapterType = compilationResult.GetType("SpaceBattle.Lib.IMovingAdapter");

        return Activator.CreateInstance(adapterType!, obj)!;
    }
}
