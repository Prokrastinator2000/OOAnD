using Moq;
using SpaceBattle.Lib;
namespace SpaceBattle.Tests;
public class MoveTest
{
    [Fact]
    public void TestPositionChange()
    {
        var moving = new Mock<IMoving>();
        moving.SetupGet(m => m.Position).Returns(new Vec(new int[] { 12, 5 }));
        moving.SetupGet(m => m.Velocity).Returns(new Vec(new int[] { -4, 1 }));
        var cmd = new MoveCommand(moving.Object);
        var expected = new Vec(new int[] { 8, 6 });
        cmd.Execute();
        Console.WriteLine(moving.Object.Position.Values[0]);
        moving.VerifySet(m => m.Position = It.Is<Vec>(m => m.Equals(new Vec(new int[] { 8, 6 }))));

    }

    [Fact]
    public void TestPositionGetThrowsException()
    {
        var moving = new Mock<IMoving>();
        moving.SetupGet(m => m.Position).Throws<Exception>();
        moving.SetupGet(m => m.Velocity).Returns(new Vec(new int[] { -7, 3 }));
        var cmd = new MoveCommand(moving.Object);
        Assert.Throws<Exception>(
            () => cmd.Execute()
        );
    }
    [Fact]
    public void TestPositionSetThrowsException()
    {
        var moving = new Mock<IMoving>();
        moving.SetupSet(m => m.Position = It.IsAny<Vec>()).Callback(() => throw new Exception());
        moving.SetupGet(m => m.Position).Returns(new Vec(new int[] { 12, 5 }));
        moving.SetupGet(m => m.Velocity).Returns(new Vec(new int[] { -7, 3 }));
        var cmd = new MoveCommand(moving.Object);
        Assert.Throws<Exception>(
            () => cmd.Execute()
        );

    }
    [Fact]
    public void TestVelocityGetThrowsException()
    {
        var moving = new Mock<IMoving>();
        moving.SetupGet(m => m.Position).Returns(new Vec(new int[] { 12, 5 }));
        moving.SetupGet(m => m.Velocity).Throws<Exception>();
        var cmd = new MoveCommand(moving.Object);
        Assert.Throws<Exception>(
            () => cmd.Execute()
        );
    }
}
