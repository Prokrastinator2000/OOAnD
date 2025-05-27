using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class QuadrantInspectorTests
{
    public QuadrantInspectorTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void UpdateObjectQuadrants_SingleQuadrantTest()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Field.Quadrant.Size", (object[] args) => (object)10).Execute();

        var inspector = new GridCheck();
        var mockMoving = new Mock<IMoving>();
        mockMoving.SetupGet(o => o.Position).Returns(new Vec(new[] { 0, 0 }));

        var matrix = new int[,] {
            {1, 0, 0},
            {0, 1, 0},
            {0, 0, 1}
        };

        inspector.UpdateObjectQuadrants(mockMoving.Object, matrix);

        var result = inspector.GetObjectsInSameQuadrant(new[] { 0, 0 });
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result);
    }

    [Fact]
    public void UpdateObjectQuadrants_MultipleQuadrantsTest()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Field.Quadrant.Size", (object[] args) => (object)10).Execute();

        var inspector = new GridCheck();
        var mockMoving = new Mock<IMoving>();

        mockMoving.SetupGet(o => o.Position).Returns(new Vec(new[] { 9, 9 }));

        var matrix = new int[,] {
        {1, 1},
        {1, 1}
    };

        inspector.UpdateObjectQuadrants(mockMoving.Object, matrix);

        var result00 = inspector.GetObjectsInSameQuadrant(new[] { 0, 0 });
        var result10 = inspector.GetObjectsInSameQuadrant(new[] { 1, 0 });
        var result01 = inspector.GetObjectsInSameQuadrant(new[] { 0, 1 });
        var result11 = inspector.GetObjectsInSameQuadrant(new[] { 1, 1 });

        Assert.Single(result00);
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result00);

        Assert.Single(result10);
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result10);

        Assert.Single(result01);
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result01);

        Assert.Single(result11);
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result11);
    }

    [Fact]
    public void GetObjectsInSameSquare_MultipleObjectsTest()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Field.Quadrant.Size", (object[] args) => (object)10).Execute();

        var inspector = new GridCheck();

        var mockMoving1 = new Mock<IMoving>();
        var mockMoving2 = new Mock<IMoving>();
        var mockMoving3 = new Mock<IMoving>();

        mockMoving1.SetupGet(o => o.Position).Returns(new Vec(new[] { 9, 9 }));
        var matrix1 = new int[,] { { 1, 1 } };

        mockMoving2.SetupGet(o => o.Position).Returns(new Vec(new[] { 15, 5 }));
        var matrix2 = new int[,] { { 1 } };

        mockMoving3.SetupGet(o => o.Position).Returns(new Vec(new[] { 5, 15 }));
        var matrix3 = new int[,] { { 1 } };

        inspector.UpdateObjectQuadrants(mockMoving1.Object, matrix1);
        inspector.UpdateObjectQuadrants(mockMoving2.Object, matrix2);
        inspector.UpdateObjectQuadrants(mockMoving3.Object, matrix3);

        var quadrant00Objects = inspector.GetObjectsInSameQuadrant(new[] { 0, 0 });
        var quadrant10Objects = inspector.GetObjectsInSameQuadrant(new[] { 1, 0 });
        var quadrant01Objects = inspector.GetObjectsInSameQuadrant(new[] { 0, 1 });

        Assert.Single(quadrant00Objects);
        Assert.Equal(new List<IMoving> { mockMoving1.Object }, quadrant00Objects);

        Assert.Equal(new List<IMoving> { mockMoving1.Object, mockMoving2.Object }, quadrant10Objects);

        Assert.Single(quadrant01Objects);
        Assert.Equal(new List<IMoving> { mockMoving3.Object }, quadrant01Objects);
    }

    [Fact]
    public void UpdateObjectQuadrants_WhenObjectMoves_ShouldUpdateQuadrants()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Field.Quadrant.Size", (object[] args) => (object)10).Execute();

        var inspector = new GridCheck();
        var mockMoving = new Mock<IMoving>();
        mockMoving.SetupProperty(o => o.Position);
        mockMoving.Object.Position = new Vec(new[] { 5, 5 });

        var matrix = new int[,] { { 1 } };

        inspector.UpdateObjectQuadrants(mockMoving.Object, matrix);

        var result00 = inspector.GetObjectsInSameQuadrant(new[] { 0, 0 });
        Assert.Single(result00);

        mockMoving.Object.Position = new Vec(new[] { 15, 15 });
        inspector.UpdateObjectQuadrants(mockMoving.Object, matrix);

        var result11 = inspector.GetObjectsInSameQuadrant(new[] { 1, 1 });

        Assert.Empty(result00);
        Assert.Equal(new List<IMoving> { mockMoving.Object }, result11);
    }
}
