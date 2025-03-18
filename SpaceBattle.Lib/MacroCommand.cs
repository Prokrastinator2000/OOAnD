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
        cmds.ToList().ForEach(c => c.Execute());
    }
}
