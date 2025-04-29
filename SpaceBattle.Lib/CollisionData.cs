namespace SpaceBattle.Lib;
public class CollisionData : ICollisionDetection
{
    public IMoving FirstObject { get; }
    public int[,] FirstObjectMatrix { get; }
    public IMoving SecondObject { get; }
    public int[,] SecondObjectMatrix { get; }

    public CollisionData(IMoving obj1, int[,] matrix1, IMoving obj2, int[,] matrix2)
    {
        FirstObject = obj1;
        FirstObjectMatrix = matrix1;
        SecondObject = obj2;
        SecondObjectMatrix = matrix2;
    }
}
