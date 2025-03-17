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
    public void GenerateUuidTest()
    {
        var registerUuidGenerate = new RegisterIoCDependencyUuidGenerate();
        registerUuidGenerate.Execute();

        var uuid1 = Ioc.Resolve<string>("Game.Object.id.Generate");
        var uuid2 = Ioc.Resolve<string>("Game.Object.id.Generate");

        Assert.False(string.IsNullOrEmpty(uuid1));
        Assert.False(string.IsNullOrEmpty(uuid2));

        Assert.True(Guid.TryParse(uuid1, out _));
        Assert.True(Guid.TryParse(uuid2, out _));

        Assert.NotEqual(uuid1, uuid2);
    }
}
