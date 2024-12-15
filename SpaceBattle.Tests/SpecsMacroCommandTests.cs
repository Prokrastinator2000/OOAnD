using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class MacroCommandTests
{
    [Fact]
    public void Test_Resolve_MacroCommand_Success()
    {
        // Список наименований команд, содержащихся в Ioc
        IEnumerable<string> CmdNames = new List<string> { "Command1", "Command2", "Command.Move", "Commands.Rotate" };

        var cmd1 = new Mock<Command1>();
        var cmd2 = new Mock<Command2>();
        var cmd3 = new Mock<CommandMove>();
        var cmd4 = new Mock<CommandRotate>();

        // Формируем Ioc
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command1",
                            //(object[] args) => cmd1.Object).Execute();
                            (object[] args) => cmd1.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command2",
                            (object[] args) => cmd2.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Move",
                            (object[] args) => cmd3.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Rotate",
                            (object[] args) => cmd4.Object).Execute();

        // Формируем исходную макрокоманду
        ICommand[] list = [cmd1.Object, cmd2.Object, cmd3.Object, cmd4.Object];
        var SourceMacroCommand = new MacroCommand(list);
        
         // Создадим стратегию
        var strategy = new CreateMacroCommandStrategy("Macro.Test");

        // Разрешим макрокоманду
        var macroCommand = strategy.Resolve();

        // Выполним макрокоманду
        macroCommand.Execute();

        // Проверим вывод или другие ожидания
         cmd1.Verify(x => x.Execute());
         cmd2.Verify(x => x.Execute());
         cmd3.Verify(x => x.Execute());
         cmd4.Verify(x => x.Execute());
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        // Здесь контейнер IoC уже заполнен. Как его сбросить - непонятно.
        // Поэтому используем стратегию, которая требует больше команд, чем есть в IoC
        var strategy = new CreateMacroCommandStrategy("Macro.Extended");

        // Попробуем разрешить макрокоманду и поймаем исключение
        Assert.Throws<InvalidOperationException>(() => strategy.Resolve());
    }
}