using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class DetectCollisionCommandTests
    {
        private Mock<IMoving> CreateMovingMock(int x, int y)
        {
            var mock = new Mock<IMoving>();
            var vec = new Vec(new int[] { x, y });
            mock.SetupGet(m => m.Position).Returns(vec);
            return mock;
        }

        [Fact]
        public void Execute_NoCollision_DoesNotThrowException()
        {

            var obj1 = CreateMovingMock(0, 0);
            var matrix1 = new int[,] { { 0, 0 }, { 0, 1 } };

            var obj2 = CreateMovingMock(2, 2);
            var matrix2 = new int[,] { { 1, 0 }, { 0, 0 } };

            var collisionData = new CollisionData(obj1.Object, matrix1, obj2.Object, matrix2);
            var command = new DetectCollisionCommand(collisionData);

            command.Execute();
        }

        [Fact]
        public void Execute_CollisionDetected_ThrowsInvalidOperationException()
        {

            var obj1 = CreateMovingMock(0, 0);
            var matrix1 = new int[,] { { 1, 0 }, { 0, 0 } };

            var obj2 = CreateMovingMock(0, 0);
            var matrix2 = new int[,] { { 1, 0 }, { 0, 0 } };

            var collisionData = new CollisionData(obj1.Object, matrix1, obj2.Object, matrix2);
            var command = new DetectCollisionCommand(collisionData);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void Execute_PartialCollision_ThrowsInvalidOperationException()
        {

            var obj1 = CreateMovingMock(0, 0);
            var matrix1 = new int[,] { { 1, 1 }, { 0, 0 } };

            var obj2 = CreateMovingMock(1, 0);
            var matrix2 = new int[,] { { 1, 0 }, { 0, 0 } };

            var collisionData = new CollisionData(obj1.Object, matrix1, obj2.Object, matrix2);
            var command = new DetectCollisionCommand(collisionData);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void CollisionData_Properties_ReturnCorrectValues()
        {

            var obj1 = CreateMovingMock(0, 0);
            var matrix1 = new int[,] { { 1 } };
            var obj2 = CreateMovingMock(0, 0);
            var matrix2 = new int[,] { { 0 } };

            var collisionData = new CollisionData(obj1.Object, matrix1, obj2.Object, matrix2);

            Assert.Same(obj1.Object, collisionData.FirstObject);
            Assert.Same(obj2.Object, collisionData.SecondObject);
            Assert.Same(matrix1, collisionData.FirstObjectMatrix);
            Assert.Same(matrix2, collisionData.SecondObjectMatrix);
        }
    }
}
