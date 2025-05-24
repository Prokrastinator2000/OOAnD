namespace SpaceBattle.Test;
using App;
using App.Scopes;
using Moq;
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

        var RegisterGeneratorCommand = new RegisterIocDependencyGenerator();
        RegisterGeneratorCommand.Execute();
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
    public void Adapter_ForIMoving_ShouldGetAndSetPropertiesCorrectly()
    {
        var mockCommand = new Mock<ICommand>();
        var mockVec = new Mock<Vec>(MockBehavior.Strict, new[] { 0, 0 });

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Position.Get", (object[] args) => mockVec.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Velocity.Get", (object[] args) => mockVec.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Position.Set", (object[] args) => mockCommand.Object).Execute();

        Ioc.Resolve<ICommand>("IoC.Register",
            "Game.Adapter.InterfaceType",
            (object[] args) => typeof(IMoving)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register",
            "Game.Adapter.TypeName",
            (object[] args) => "SpaceBattle.Lib.IMovingAdapter").Execute();

        var targetDictionary = new Dictionary<string, object>();
        var factory = new AdapterFactory();

        var adapter = factory.Create(targetDictionary) as IMoving;

        var position = adapter!.Position;
        var velocity = adapter.Velocity;

        adapter.Position = mockVec.Object;

        mockCommand.Verify(cmd => cmd.Execute(), Times.Once);
        Assert.NotNull(position);
        Assert.NotNull(velocity);
    }
}
