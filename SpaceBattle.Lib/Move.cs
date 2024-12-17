namespace SpaceBattle.Lib;
public class MoveCommand : ICommand
{
    private readonly IMoving obj;
    public MoveCommand(IMoving obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        obj.Position += obj.Velocity;
    }
}
