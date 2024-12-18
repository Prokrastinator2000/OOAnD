namespace SpaceBattle.Lib;

public interface IRotating
{
    public Angle Angle { get; set; }
    public Angle Velocity { get; }
}

