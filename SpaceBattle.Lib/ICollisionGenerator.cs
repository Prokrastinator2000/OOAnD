namespace SpaceBattle.Lib;

public interface ICollisionDataGenerator
{
    string FirstShape { get; }
    string SecondShape { get; }

    IList<int[]> GenerateCollisionData();
}
