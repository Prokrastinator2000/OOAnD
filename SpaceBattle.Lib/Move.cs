namespace SpaceBattle.Lib;

public interface IMoving
{
    Vec Position { get; set; }
    Vec Velocity { get; }
}
public interface ICommand
{
    public void Execute();

}
public class MoveCommand : ICommand
{
    private readonly IMoving obj;
    public MoveCommand(IMoving obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        obj.Position = obj.Position + obj.Velocity;
    }
}
public class Vec
{
    public int[] Values { get; set; }
    public Vec(int[] Values)
    {
        this.Values = Values;
    }

    public static Vec operator +(Vec a, Vec b)
    {
        for (var i = 0; i < a.Values.Length; i++)
        {
            a.Values[i] += b.Values[i];
        }

        return a;
    }
}

