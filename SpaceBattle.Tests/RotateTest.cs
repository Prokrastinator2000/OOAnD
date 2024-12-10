using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RotateTest
    {
        [Fact]
        public void TestRotationPositive()
        {
            var angle = new Angle(45, 360);
            var angle2 = new Angle(45, 360);
            var rotating = new Mock<IRotating>();
            rotating.SetupGet(r => r.Angle).Returns(angle);
            rotating.SetupGet(r => r.Velocity).Returns(angle2);

            var cmd = new RotateCommand(rotating.Object);
            cmd.Execute();
            rotating.VerifySet(r => r.Angle = It.Is<Angle>(v => v.a == 90 && v.n == 360));
        }

        [Fact]
        public void TestAngleGetThrowsException()
        {
            var angle2 = new Angle(90, 360);
            var rotating = new Mock<IRotating>();
            rotating.SetupGet(r => r.Angle).Throws<Exception>();
            rotating.SetupGet(r => r.Velocity).Returns(angle2);

            var cmd = new RotateCommand(rotating.Object);

            Assert.Throws<Exception>(() => cmd.Execute());
        }

        [Fact]
        public void TestVelocityGetThrowsException()
        {
            var rotating = new Mock<IRotating>();
            rotating.SetupGet(r => r.Angle).Returns(new Angle(45, 360));
            rotating.SetupGet(r => r.Velocity).Throws<Exception>();

            var cmd = new RotateCommand(rotating.Object);
            Assert.Throws<Exception>(() => cmd.Execute());
        }

        [Fact]
        public void TestAngleSetThrowsException()
        {
            var angle = new Angle(45, 360);
            var angle2 = new Angle(90, 360);
            var rotating = new Mock<IRotating>();
            rotating.SetupGet(r => r.Angle).Returns(angle);
            rotating.SetupGet(r => r.Velocity).Returns(angle2);

            rotating.SetupSet(r => r.Angle = It.IsAny<Angle>()).Callback(() => throw new Exception());

            var cmd = new RotateCommand(rotating.Object);

            Assert.Throws<Exception>(() => cmd.Execute());
        }
        [Fact]
        public void TestAngleSetThrowsExceptionOnMismatch()
        {
            var angle = new Angle(45, 360);
            var velocity = new Angle(90, 460);

            var rotating = new Mock<IRotating>();
            rotating.SetupGet(r => r.Angle).Returns(angle);
            rotating.SetupGet(r => r.Velocity).Returns(velocity);

            var cmd = new RotateCommand(rotating.Object);

            Assert.Throws<Exception>(() => cmd.Execute());
        }
    }
}
