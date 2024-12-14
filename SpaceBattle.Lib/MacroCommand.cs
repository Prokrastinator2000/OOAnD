using App;
namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly ICommand[] cmds;
    public MacroCommand(ICommand[] cmds)
    {
        this.cmds = cmds;
    }

    public void Execute()
    {
        var ICommandList = new List<ICommand>();
        ICommandList = cmds.ToList<ICommand>();
        ICommandList.ForEach(c => c.Execute());
    }
}
