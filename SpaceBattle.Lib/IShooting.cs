namespace SpaceBattle.Lib;
public interface IShooting
{
    Vec Position { get; }
    Vec Velocity { get; }
    Vec Impulse { get; }
}
