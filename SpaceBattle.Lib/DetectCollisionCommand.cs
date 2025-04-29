using App;
namespace SpaceBattle.Lib;
public class DetectCollisionCommand : ICommand
{
    private readonly ICollisionDetection _data;

    public DetectCollisionCommand(ICollisionDetection data)
    {
        _data = data;
    }

    public void Execute()
    {
        if (GetOccupiedPoints(_data.FirstObject, _data.FirstObjectMatrix)
            .Intersect(GetOccupiedPoints(_data.SecondObject, _data.SecondObjectMatrix))
            .Any())
        {
            throw new InvalidOperationException();
        }
    }

    private static IEnumerable<(int X, int Y)> GetOccupiedPoints(IMoving obj, int[,] matrix)
    {
        return from y in Enumerable.Range(0, matrix.GetLength(0))
               from x in Enumerable.Range(0, matrix.GetLength(1))
               where matrix[y, x] == 1
               select (
                   X: obj.Position.Values[0] + x,
                   Y: obj.Position.Values[1] + y
               );
    }
}
