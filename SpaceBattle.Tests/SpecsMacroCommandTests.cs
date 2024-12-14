using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib;

public class MacroCommandTests
{
    [Fact]
    public void Test_Resolve_MacroCommand_Success()
    {
        // Настроим IoC контейнер
        var container = new IocContainer();
        container.Register<ICommand>("Command1", new Command1());
        container.Register<ICommand>("Command2", new Command2());
        container.Register<ICommand>("Command3", new Command3());

        // Создадим стратегию
        var strategy = new CreateMacroCommandStrategy("Macro.Test", container);

        // Разрешим макрокоманду
        var macroCommand = strategy.Resolve(null);

        // Выполним макрокоманду
        macroCommand.Execute();

        // Проверим вывод или другие ожидания
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        // Настроим IoC контейнер только с одной командой
        var container = new IocContainer();
        container.Register<ICommand>("Command1", new Command1());

        // Создадим стратегию
        var strategy = new CreateMacroCommandStrategy("Macro.Test", container);

        // Попробуем разрешить макрокоманду и поймаем исключение
        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(null));
    }
}