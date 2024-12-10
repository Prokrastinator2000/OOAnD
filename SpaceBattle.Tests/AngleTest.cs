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
        var angle2 = new Angle(90, 460);
        var rotating = new Mock<IRotating>();
        rotating.SetupGet(r => r.Angle).Returns(angle);
        rotating.SetupGet(r => r.Velocity).Returns(angle2);

        rotating.SetupSet(r => r.Angle = It.IsAny<Angle>()).Callback(() => throw new Exception());

        var cmd = new RotateCommand(rotating.Object);

        Assert.Throws<Exception>(() => cmd.Execute());
    }
    [Fact]
    public void TestAngleAddPositive()
    {
        var angle1 = new Angle(5, 8);
        var angle2 = new Angle(7, 8);

        var result = angle1 + angle2;

        Assert.Equal(new Angle(4, 8), result);
    }

    [Fact]
    public void TestAngleEqualsPositive()
    {
        var angle1 = new Angle(15, 8);
        var angle2 = new Angle(23, 8);

        Assert.True(angle1.Equals(angle2));
    }

    [Fact]
    public void TestAngleEquals1Positive()
    {
        var angle1 = new Angle(15, 8);
        var angle2 = new Angle(23, 8);

        Assert.True(angle1 == angle2);
    }

    [Fact]
    public void TestAngleEqualsNegative()
    {
        var angle1 = new Angle(1, 8);
        var angle2 = new Angle(2, 8);

        Assert.False(angle1.Equals(angle2));
    }

    [Fact]
    public void TestAngleEquals1Negative()
    {
        var angle1 = new Angle(1, 8);
        var angle2 = new Angle(2, 8);

        Assert.True(angle1 != angle2);
    }

    [Fact]
    public void GetHashCode_ShouldReturnConsistentResult()
    {
        var angle = new Angle(15, 8);

        var hashCode1 = angle.GetHashCode();
        var hashCode2 = angle.GetHashCode();

        Assert.Equal(hashCode1, hashCode2);
    }
}
