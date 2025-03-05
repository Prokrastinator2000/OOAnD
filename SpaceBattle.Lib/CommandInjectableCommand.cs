using App;
public class InjectableCommand : ICommand, ICommandInjectable
{
    private ICommand? injectedCommand;

    public void Inject(ICommand command)
    {
        injectedCommand = command;
    }

    public void Execute()
    {
        if (injectedCommand == null)
        {
            throw new InvalidOperationException("Command not injected.");
        }

        injectedCommand.Execute();
    }
}
