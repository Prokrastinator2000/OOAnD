using App;
namespace SpaceBattle.Lib;

public class StartCommand : ICommand
{
    private readonly string _operationType;
    private readonly object[] _operationArgs;
    private readonly string _targetKey;

    public StartCommand(IDictionary<string, object> orderData)
    {
        _operationType = (string)orderData["Action"];
        _operationArgs = (object[])orderData["Args"];
        _targetKey = (string)orderData["Key"];
    }

    public void Execute()
    {
        var operation = Ioc.Resolve<ICommand>("Macro." + _operationType, _operationArgs);
        var injectableCommand = Ioc.Resolve<ICommand>("Commands.CommandInjectable");
        var commandReceiver = Ioc.Resolve<ICommandReceiver>("Game.Queue");
        var compositeCommand = Ioc.Resolve<ICommand>("Commands.Macro", (ICommand[])[operation, injectableCommand]);
        var queuedCommand = Ioc.Resolve<ICommand>("Commands.Send", commandReceiver, compositeCommand);
        var configurableCommand = (ICommandInjectable)injectableCommand;
        configurableCommand.Inject(queuedCommand);
        var dispatchCommand = Ioc.Resolve<ICommand>("Commands.Send", commandReceiver, queuedCommand);
        dispatchCommand.Execute();
        var targetObject = Ioc.Resolve<IDictionary<string, object>>("Game.Object", _targetKey);
        targetObject.Add(_operationType, injectableCommand);
    }
}
