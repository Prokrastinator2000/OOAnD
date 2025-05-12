namespace SpaceBattle.Test;

using App;
using App.Scopes;
using SpaceBattle.Lib;
using Xunit;

public class AdapterGeneratorTests
{
    public AdapterGeneratorTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

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

    [Fact]
    public void AdapterCodeGeneratorTest_ForIMoving()
    {
        var expectedIMovingAdapterCode =
        """
        namespace SpaceBattle.Lib;
        using App;
        class IMovingAdapter : IMoving {
                    Vec target;
                    public IMovingAdapter(Vec target) => this.target = target;
                    public Vec Position {
                        set { Ioc.Resolve<ICommand>("Game.Position.Set", target, value).Execute(); }
                        get { return Ioc.Resolve<Vec>("Game.Position.Get", target); }
                    }
                    public Vec Velocity {
                        
                        get { return Ioc.Resolve<Vec>("Game.Velocity.Get", target); }
                    }
                }
        """.Replace("\r\n", "\n").Trim();

        var generatedIMovingCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            typeof(IMoving),
            typeof(Vec),
            typeof(Vec)
        ).Replace("\r\n", "\n").Trim();

        Assert.Equal(expectedIMovingAdapterCode, generatedIMovingCode);
    }

    [Fact]
    public void AdapterCodeGeneratorTest_ForMoveCommand()
    {
        var expectedMoveCommandAdapterCode =
        """
        namespace SpaceBattle.Lib;
        using App;
        class MoveCommandAdapter : MoveCommand {
                    IMoving target;
                    public MoveCommandAdapter(IMoving target) => this.target = target;
                }
        """.Replace("\r\n", "\n").Trim();

        var generatedMoveCommandCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            typeof(MoveCommand),
            typeof(IMoving)
        ).Replace("\r\n", "\n").Trim();

        Assert.Equal(expectedMoveCommandAdapterCode, generatedMoveCommandCode);
    }

    [Fact]
    public void Build_WithGenericTargetType_GeneratesCorrectCode()
    {
        var builder = new AdapterBuilder(typeof(IMoving), typeof(List<string>));

        var code = builder.Build();

        Assert.Contains("List<String> target", code);
    }

    [Fact]
    public void CompilerTest_success()
    {
        var generatedIMovingCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            typeof(IMoving),
            typeof(Vec),
            typeof(Vec)
        ).Replace("\r\n", "\n").Trim();

        var compilationResult = Compiler.CompileGeneratedCode(generatedIMovingCode,
        typeof(IMoving),
        typeof(Vec));

        var adapterType = compilationResult.GetType("SpaceBattle.Lib.IMovingAdapter");
        Assert.NotNull(adapterType);

        Assert.True(typeof(IMoving).IsAssignableFrom(adapterType));
    }

    [Fact]
    public void CompilerTest_null()
    {
        string nullCode = null!;

        var exception = Assert.Throws<ArgumentNullException>(
            () => Compiler.CompileGeneratedCode(nullCode));

        Assert.Equal("text", exception.ParamName);
    }
}
