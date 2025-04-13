using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests;

public class GameObjectsTests
{
    public GameObjectsTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void GetObjectTest()
    {
        var registerIDGenerate = new RegisterIoCDependencyIDGenerate();
        registerIDGenerate.Execute();

        var RegisterGameObjectRepository = new RegisterIoCDependencyGameObjectRepository();
        RegisterGameObjectRepository.Execute();

        var RegisterAddGameObject = new RegisterIoCDependencyAddGameObjectToRepository();
        RegisterAddGameObject.Execute();

        var RegisterGameObject = new RegisterIoCDependencyGameObject();
        RegisterGameObject.Execute();

        var item = new object();
        var ID = Ioc.Resolve<string>("Game.Object.id.Generate");

        var repository = (IDictionary<string, object>)Ioc.Resolve<object>("Game.Object.Repository");

        Ioc.Resolve<ICommand>("Game.Object.Add", ID, item, repository).Execute();

        var retrievedItem = Ioc.Resolve<object>("Game.Object", ID);

        Assert.Same(item, retrievedItem);
    }

    [Fact]
    public void AddObjectTest()
    {
        var registerIDGenerate = new RegisterIoCDependencyIDGenerate();
        registerIDGenerate.Execute();

        var RegisterGameObjectRepository = new RegisterIoCDependencyGameObjectRepository();
        RegisterGameObjectRepository.Execute();

        var RegisterGameObject = new RegisterIoCDependencyGameObject();
        RegisterGameObject.Execute();

        var RegisterAddGameObject = new RegisterIoCDependencyAddGameObjectToRepository();
        RegisterAddGameObject.Execute();

        var item = new object();
        var ID = Ioc.Resolve<string>("Game.Object.id.Generate", item);

        var repository = (IDictionary<string, object>)Ioc.Resolve<object>("Game.Object.Repository");

        Ioc.Resolve<ICommand>("Game.Object.Add", ID, item, repository).Execute();

        Assert.True(repository.ContainsKey(ID));
        Assert.Equal(item, repository[ID]);
    }

    [Fact]
    public void RemoveObjectTest()
    {
        var RegisterGameObjectRepository = new RegisterIoCDependencyGameObjectRepository();
        RegisterGameObjectRepository.Execute();

        var RegisterAddGameObject = new RegisterIoCDependencyAddGameObjectToRepository();
        RegisterAddGameObject.Execute();

        var RegisterRemoveGameObject = new RegisterIoCDependencyRemoveGameObjectFromRepository();
        RegisterRemoveGameObject.Execute();

        var item = new object();
        var ID = Ioc.Resolve<string>("Game.Object.id.Generate", item);

        var repository = (IDictionary<string, object>)Ioc.Resolve<object>("Game.Object.Repository");

        Ioc.Resolve<ICommand>("Game.Object.Add", ID, item, repository).Execute();

        Assert.True(repository.ContainsKey(ID));

        var removeCommand = Ioc.Resolve<ICommand>("Game.Object.Remove", ID, repository);
        removeCommand.Execute();

        Assert.False(repository.ContainsKey(ID));
    }
}
