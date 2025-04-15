using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class CollisionDataSaveCommandTests
{
    public CollisionDataSaveCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    [Fact]
    public void Execute_ShouldWriteFileAndLoadToMemory()
    {
        var dummyData = new List<int[]> { new[] { 1, 2 } };
        var path = "testpath/";
        var fullPath = path + "mycollision";

        var writeCommand = new Mock<ICommand>();
        var loadCommand = new Mock<ICommand>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Data.CollisionFilesPath", (object[] args) => path).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.WriteToFile", (object[] args) =>
        {
            Assert.Equal(fullPath, args[0]);
            Assert.Same(dummyData, args[1]);
            return writeCommand.Object;
        }).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.LoadDataToMemory", (object[] args) =>
        {
            Assert.Equal("mycollision", args[0]);
            Assert.Same(dummyData, args[1]);
            return loadCommand.Object;
        }).Execute();

        new RegisterIoCDependencyCollisionDataSaveCommand().Execute();
        var command = Ioc.Resolve<ICommand>("Commands.SaveData", "mycollision", dummyData);
        command.Execute();

        writeCommand.Verify(c => c.Execute(), Times.Once);
        loadCommand.Verify(c => c.Execute(), Times.Once);
    }
}
