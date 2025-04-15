using App;
public class CollisionCheckCommand : ICommand
{

    private readonly ICollision _tree;
    public CollisionCheckCommand(ICollision tree)
    {
        _tree = tree;
    }
    public void Execute()
    {
        var Collision = _tree.Tree
            .TryGetValue(_tree.DeltaPosX, out var posYDict) &&
            posYDict.TryGetValue(_tree.DeltaPosY, out var velXDict) &&
            velXDict.TryGetValue(_tree.DeltaVelX, out var storedVelY) &&
            storedVelY == _tree.DeltaVelY;

        if (Collision)
        {
            throw new InvalidOperationException();
        }
    }
}
