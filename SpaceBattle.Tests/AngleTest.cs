using Moq;
using SpaceBattle.Lib;
namespace SpaceBattle.Tests;
public class AngleTest
{
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
    public void TestAngleEqualsUsingASignPositive()
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
    public void TestAngleEqualsUsingASignNegative()
    {
        var angle1 = new Angle(1, 8);
        var angle2 = new Angle(2, 8);

        Assert.True(angle1 != angle2);
    }
    [Fact]
    public void TestGetHashCode()
    {
        var angle = new Angle(15, 8);

        var hashCode1 = angle.GetHashCode();
        var hashCode2 = angle.GetHashCode();

        Assert.Equal(hashCode1, hashCode2);
    }
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
    public void TestAngleToAngleConstructor()
    {
        var original = new Angle(45, 360);
        var copy = new Angle(original);

        Assert.Equal(original.a, copy.a);
        Assert.Equal(original.n, copy.n);
    }
    [Fact]
    public void TestSin()
    {
        var angle = new Angle(90, 360);
        var result = angle.Sin();

        Assert.Equal(Math.Sin(Math.PI / 2), result);
    }
    [Fact]
    public void TestCos()
    {
        var angle = new Angle(0, 360);
        var result = angle.Cos();

        Assert.Equal(Math.Cos(0), result);
    }
    [Fact]
    public void TestAngleConstructorNegativeAngle()
    {
        var angle = new Angle(-10, 360);
        Assert.Equal(-10, angle.a);
    }
    [Fact]
    public void TestSetAngleNoChange()
    {
        var angle = new Angle(45, 360);
        angle.SetAngle(45);

        Assert.Equal(45, angle.a);
    }
    [Fact]
    public void TestAngleAddOverflow()
    {
        var angle1 = new Angle(350, 360);
        var angle2 = new Angle(20, 360);

        var result = angle1 + angle2;

        Assert.Equal(new Angle(10, 360), result);
    }
    [Fact]
    public void TestAngleAddThrowsExceptionForDifferentN()
    {
        var angle1 = new Angle(5, 8);
        var angle2 = new Angle(7, 9);

        Assert.Throws<Exception>(() => angle1 + angle2);
    }
    [Fact]
    public void TestSetAngleOutOfRangeValue()
    {
        var angle = new Angle(45, 360);
        angle.SetAngle(400);
        Assert.Equal(40, angle.a);
    }

    [Fact]
    public void TestAngleEqualsDifferentN()
    {
        var angle1 = new Angle(30, 360);
        var angle2 = new Angle(30, 720);

        Assert.False(angle1.Equals(angle2));
    }
    [Fact]
    public void TestAngleConstructorLargeAngle()
    {
        var angle = new Angle(370, 360);
        Assert.Equal(10, angle.a);
    }
    [Fact]
    public void TestAngleEqualsButDifferentObjects()
    {
        var angle1 = new Angle(30, 360);
        var angle2 = new Angle(30, 360);
        Assert.True(angle1.Equals(angle2));
    }
    [Fact]
    public void TestAngleEqualsButDifferentClass()
    {
        var angle1 = new Angle(30, 360);
        var i = 6;
        Assert.False(angle1.Equals(i));
    }
    [Fact]
    public void TestAngleAddThrowsExceptionMessage()
    {
        var angle1 = new Angle(10, 360);
        var angle2 = new Angle(15, 720);
        var exception = Assert.Throws<Exception>(() => angle1 + angle2);
        Assert.Equal("Different n", exception.Message);
    }
    [Fact]
    public void TestGetHashCodeForDifferentAngles()
    {
        var angle1 = new Angle(15, 360);
        var angle2 = new Angle(15, 360);
        var angle3 = new Angle(30, 360);

        Assert.Equal(angle1.GetHashCode(), angle2.GetHashCode());
        Assert.NotEqual(angle1.GetHashCode(), angle3.GetHashCode());
    }
}
