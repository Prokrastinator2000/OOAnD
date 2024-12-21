using App;
namespace SpaceBattle.Lib;

public class CreateMacroCommandStrategy(string commandSpec)
{
    public ICommand Resolve(object[] args)
    {
        var commandNames = Ioc.Resolve<IEnumerable<string>>("Specs." + commandSpec, args);

        var commands = commandNames.Select(cmd_name => Ioc.Resolve<ICommand>(cmd_name, args)).ToArray<ICommand>();
        return Ioc.Resolve<ICommand>("Commands.Macro", commands);
    }
}
