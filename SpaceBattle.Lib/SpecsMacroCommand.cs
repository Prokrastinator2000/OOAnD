using App;
namespace SpaceBattle.Lib;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;
    private readonly IIocContainer _iocContainer;

    public CreateMacroCommandStrategy(string commandSpec, IIocContainer iocContainer)
    {
        _commandSpec = commandSpec;
        _iocContainer = iocContainer;
    }

    public ICommand Resolve(object[] args)
    {
        try
        {
            // Получаем список наименований команд для макрокоманды
            var commandNames = GetCommandNamesForSpec(_commandSpec);

            if (commandNames == null || !commandNames.Any())
                throw new InvalidOperationException($"No commands found for spec: {_commandSpec}");

            // Создаём список команд
            var commands = new List<ICommand>();

            foreach (var commandName in commandNames)
            {
                var command = _iocContainer.Resolve<ICommand>(commandName);
                if (command == null)
                    throw new InvalidOperationException($"Command {commandName} could not be resolved.");
                
                commands.Add(command);
            }

            // Создаём и возвращаем макрокоманду
            return new MacroCommand(commands.ToArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error resolving macro command dependencies.", ex);
        }
    }

    private IEnumerable<string> GetCommandNamesForSpec(string commandSpec)
    {
        // Пример логики, возвращающей список наименований команд по спецификации
        // В реальности, например, это может быть запрос к базе данных или конфигурации
        if (commandSpec == "Macro.Test")
        {
            return new List<string> { "Command1", "Command2", "Command3" };
        }

        return null; // Или возвращать пустой список, если спецификация не найдена
    }
}

public interface IIocContainer
{
    T Resolve<T>(string name);
}

public class IocContainer : IIocContainer
{
    private readonly Dictionary<string, object> _registrations = new();

    public void Register<T>(string name, T instance)
    {
        _registrations[name] = instance;
    }

    public T Resolve<T>(string name)
    {
        return _registrations.ContainsKey(name) ? (T)_registrations[name] : default;
    }
}

// Пример команд
public class Command1 : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Command1 executed.");
    }
}

public class Command2 : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Command2 executed.");
    }
}

public class Command3 : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Command3 executed.");
    }
}