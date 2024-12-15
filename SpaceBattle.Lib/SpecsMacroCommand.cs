using App;
namespace SpaceBattle.Lib;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        _commandSpec = commandSpec;
    }

    public ICommand Resolve()
    {
        try
        {
            var commandNames = GetCommandNamesForSpec(_commandSpec);

            if (commandNames == null || !commandNames.Any())
            {
                throw new InvalidOperationException($"No commands found for spec: {_commandSpec}");
            }

            var commands = new List<ICommand>();

            foreach (var commandName in commandNames)
            {
                var command = Ioc.Resolve<ICommand>(commandName);
                if (command == null)
                {
                    throw new InvalidOperationException($"Command {commandName} could not be resolved.");
                }

                commands.Add(command);
            }

            return new MacroCommand(commands.ToArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error resolving macro command dependencies.", ex);
        }
    }

    private IEnumerable<string> GetCommandNamesForSpec(string commandSpec)
    {
        if (commandSpec == "Macro.Test")
        {
            return new List<string> { "Command1", "Command2", "Commands.Move", "Commands.Rotate" };
        }
        else if (commandSpec == "Macro.Extended")
        {
            return new List<string> { "Command1", "Command2", "Commands.Move", "Commands.Rotate", "Command3" };
        }
        else if (commandSpec == "Specs.Move")
        {
            return new List<string> { "Commands.Move" };
        }
        else if (commandSpec == "Specs.Rotate")
        {
            return new List<string> { "Commands.Rotate" };
        }

        throw new InvalidOperationException($"Strategy \"{commandSpec}\" not supported.");
    }
}

public interface IIocContainer
{
    T Resolve<T>(string name);
}

// public class IocContainer : IIocContainer
// {
//     private readonly Dictionary<string, object> _registrations = new();

//     public void Register<T>(string name, T instance)
//     {
//         _registrations[name] = instance;
//     }

//     public T Resolve<T>(string name)
//     {
//         return _registrations.ContainsKey(name) ? (T)_registrations[name] : default;
//     }
// }

// Пример команд
public class Command1 : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command1 executed.");
    }
}

public class Command2 : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command2 executed.");
    }
}

public class CommandRotate : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command.Rotate executed.");
    }
}
public class CommandMove : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command.Move executed.");
    }
}
