namespace SpaceBattle.Test;

using App;
using App.Scopes;
using SpaceBattle.Lib;
using Xunit;

public class AdapterGeneratorTests
{
    public class PositionSetCommand : ICommand
    {
        private readonly IDictionary<string, object> _target;
        private readonly Vec _value;

        public PositionSetCommand(IDictionary<string, object> target, Vec value)
        {
            _target = target;
            _value = value;
        }

        public void Execute()
        {
            _target["Position"] = _value;
        }
    }
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
        using System.Collections.Generic;
        using System;
        class IMovingAdapter : IMoving {
                    IDictionary<String, Object> target;
                    public IMovingAdapter(IDictionary<String, Object> target) => this.target = target;
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
            typeof(IDictionary<string, object>)
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
        using System.Collections.Generic;
        using System;
        class MoveCommandAdapter : MoveCommand {
                    IDictionary<String, Object> target;
                    public MoveCommandAdapter(IDictionary<String, Object> target) => this.target = target;
                }
        """.Replace("\r\n", "\n").Trim();

        var generatedMoveCommandCode = Ioc.Resolve<string>(
            "Game.Reflection.GenerateAdapterCode",
            typeof(MoveCommand),
            typeof(IDictionary<string, object>)
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
            typeof(IDictionary<string, object>)
        ).Replace("\r\n", "\n").Trim();

        var compilationResult = Compiler.CompileGeneratedCode(generatedIMovingCode,
        typeof(IMoving),
        typeof(IDictionary<string, object>));

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
    [Fact]
    public void Create_GeneratesValidAdapter()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Position.Get", (object[] args) =>
        {
            var dict = (IDictionary<string, object>)args[0];
            return (Vec)dict["Position"];
        }).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Velocity.Get", (object[] args) =>
        {
            var dict = (IDictionary<string, object>)args[0];
            return (Vec)dict["Velocity"];
        }).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Position.Set", (object[] args) =>
        {
            return new PositionSetCommand(
                (IDictionary<string, object>)args[0],
                (Vec)args[1]
            );
        }).Execute();

        var factory = new MovingObjectAdapterFactory();
        var position = new Vec(new[] { 1, 2 });
        var velocity = new Vec(new[] { 0, 1 });

        var targetDictionary = new Dictionary<string, object>();
        targetDictionary["Position"] = position;
        targetDictionary["Velocity"] = velocity;

        var adapter = factory.Create(targetDictionary);

        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<IMoving>(adapter);

        var movingAdapter = (IMoving)adapter;
        Assert.NotNull(movingAdapter.Position);

        movingAdapter.Position = new Vec([2, 2]);

        Assert.Equal(new Vec([2, 2]), movingAdapter.Position);
    }
}
