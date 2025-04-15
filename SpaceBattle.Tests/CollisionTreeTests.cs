
using Moq;
namespace SpaceBattle.Tests
{
    public class CollisionCheckCommandTests
    {
        [Fact]
        public void Execute_CollisionExists_ThrowsException()
        {

            var mockTree = new Mock<ICollision>();

            var collisionTree = new Dictionary<int, IDictionary<int, IDictionary<int, int>>>
            {
                {
                    2, new Dictionary<int, IDictionary<int, int>>
                    {
                        {
                            4, new Dictionary<int, int>
                            {
                                { 6, 8 }
                            }
                        }
                    }
                }
            };

            mockTree.SetupGet(t => t.Tree).Returns(collisionTree);
            mockTree.SetupGet(t => t.DeltaPosX).Returns(2);
            mockTree.SetupGet(t => t.DeltaPosY).Returns(4);
            mockTree.SetupGet(t => t.DeltaVelX).Returns(6);
            mockTree.SetupGet(t => t.DeltaVelY).Returns(8);

            var command = new CollisionCheckCommand(mockTree.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void Execute_NoCollision_DoesNotThrow()
        {

            var mockTree = new Mock<ICollision>();

            var collisionTree = new Dictionary<int, IDictionary<int, IDictionary<int, int>>>();

            mockTree.SetupGet(t => t.Tree).Returns(collisionTree);
            mockTree.SetupGet(t => t.DeltaPosX).Returns(1);
            mockTree.SetupGet(t => t.DeltaPosY).Returns(2);
            mockTree.SetupGet(t => t.DeltaVelX).Returns(3);
            mockTree.SetupGet(t => t.DeltaVelY).Returns(4);

            var command = new CollisionCheckCommand(mockTree.Object);

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }

        [Fact]
        public void Execute_PartialMatch_DoesNotThrow()
        {

            var mockTree = new Mock<ICollision>();

            var collisionTree = new Dictionary<int, IDictionary<int, IDictionary<int, int>>>
            {
                {
                    1, new Dictionary<int, IDictionary<int, int>>
                    {
                        {
                            2, new Dictionary<int, int>
                            {
                                { 3, 999 }
                            }
                        }
                    }
                }
            };

            mockTree.SetupGet(t => t.Tree).Returns(collisionTree);
            mockTree.SetupGet(t => t.DeltaPosX).Returns(1);
            mockTree.SetupGet(t => t.DeltaPosY).Returns(2);
            mockTree.SetupGet(t => t.DeltaVelX).Returns(3);
            mockTree.SetupGet(t => t.DeltaVelY).Returns(4);

            var command = new CollisionCheckCommand(mockTree.Object);

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }
    }
}
