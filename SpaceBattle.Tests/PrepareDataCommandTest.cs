using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class CollisionPrepareDataCommandTests
{
    public CollisionPrepareDataCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_GeneratesCollisionData_AndCallsSaveCommand()
    {

        var mockDataGenerator = new Mock<ICollisionDataGenerator>();
        var mockSaveCommand = new Mock<ICommand>();

        mockDataGenerator
            .Setup(g => g.GenerateCollisionData())
            .Returns(new List<int[]> { new int[] { 1, 2, 3, 4 } });

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.SaveData",
            (object[] args) => mockSaveCommand.Object
        ).Execute();

        var regCollisisonPrepareData = new RegisterIoCDependencyCollisionPrepareData();
        regCollisisonPrepareData.Execute();
        var regCollisionName = new RegisterIoCDependencyGetCollisionName();
        regCollisionName.Execute();
        var collisionPrepareDataCommand = Ioc.Resolve<ICommand>("Commands.PrepareData", mockDataGenerator.Object);

        collisionPrepareDataCommand.Execute();

        mockDataGenerator.Verify(g => g.GenerateCollisionData(), Times.Once);
        mockSaveCommand.Verify(c => c.Execute(), Times.Once);
    }
}
