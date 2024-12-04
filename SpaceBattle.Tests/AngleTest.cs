using Moq;
using SpaceBattle.Lib;
namespace SpaceBattle.Tests;
public class AngleTest
{
    [Fact]
    public void TestSetAngle()
    {
        var angle = new Angle(45, 360);
        angle.SetAngle(90);

        Assert.Equal(90, angle.a);
    }
    [Fact]
    public void TestAngleSetThrowsException()
    {
        var angle = new Angle(45, 360);
        var rotating = new Mock<IRotating>();
        rotating.SetupGet(r => r.Angle).Returns(angle);
        rotating.SetupGet(r => r.Velocity).Returns(90);

        rotating.SetupSet(r => r.Angle = It.IsAny<Angle>()).Callback(() => throw new Exception());

        var cmd = new RotateCommand(rotating.Object);

        Assert.Throws<Exception>(() => cmd.Execute());
    }
}
