public interface ICollision
{
    IDictionary<int, IDictionary<int, IDictionary<int, int>>> Tree { get; set; }
    int DeltaPosX { get; }
    int DeltaPosY { get; }
    int DeltaVelX { get; }
    int DeltaVelY { get; }
}
