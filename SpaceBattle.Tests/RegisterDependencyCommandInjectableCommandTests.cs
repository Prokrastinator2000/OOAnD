using App;
using App.Scopes;
namespace SpaceBattle.Tests;
public class RegisterDependencyCommandInjectableCommandTests
{
    [Fact]
    public void TestInjectableCommandRegistration()
    {
        // Инициализация IoC и скоупов
        new InitCommand().Execute();
        var scope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", scope).Execute();

        // Регистрация CommandInjectableCommand
        Ioc.Resolve<ICommand>(
           "IoC.Register",
            "Commands.CommadInjectable",
            (object[] args) => new InjectableCommand()
        ).Execute();

        // Проверка
        var command = Ioc.Resolve<ICommand>("Commands.CommadInjectable");
        var injectable = Ioc.Resolve<ICommandInjectable>("Commands.CommadInjectable");
        var commandInjectableCommand = Ioc.Resolve<InjectableCommand>("Commands.CommadInjectable");

        Assert.NotNull(command);
        Assert.NotNull(injectable);
        Assert.NotNull(commandInjectableCommand);
    }
}
