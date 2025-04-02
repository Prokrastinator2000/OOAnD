using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyUuidGenerateTests
{
    public RegisterIoCDependencyUuidGenerateTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void GenerateIDTest()
    {
        var registerUuidGenerate = new RegisterIoCDependencyIDGenerate();
        registerUuidGenerate.Execute();

        var ID1 = Ioc.Resolve<string>("Game.Object.id.Generate");
        var ID2 = Ioc.Resolve<string>("Game.Object.id.Generate");

        Assert.False(string.IsNullOrEmpty(ID1));
        Assert.False(string.IsNullOrEmpty(ID2));

        Assert.True(Guid.TryParse(ID1, out _));
        Assert.True(Guid.TryParse(ID2, out _));

        Assert.NotEqual(ID1, ID2);
    }
}
