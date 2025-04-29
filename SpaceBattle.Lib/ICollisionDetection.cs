namespace SpaceBattle.Lib;

public interface ICollisionDetection
{
    IMoving FirstObject { get; }
    IMoving SecondObject { get; }
    int[,] FirstObjectMatrix { get; }
    int[,] SecondObjectMatrix { get; }
}
