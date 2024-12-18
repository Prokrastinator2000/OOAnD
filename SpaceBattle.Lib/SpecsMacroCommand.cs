﻿using App;
namespace SpaceBattle.Lib;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        _commandSpec = commandSpec;
    }

    public ICommand Resolve(object[] args)
    {
        var commandNames = GetCommandNamesForSpec(_commandSpec);
        try
        {
            var commands = from cmd in commandNames
                           select Ioc.Resolve<ICommand>(cmd, args);

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
