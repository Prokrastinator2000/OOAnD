using Moq;
using SpaceBattle.Lib;

using App;
using App.Scopes;

namespace SpaceBattle.Tests;

public class RegisterIoCDependencyMoveCommandTest
{
    [Fact]
    public void ResolveOfDependensyExist()
    {
        //Arrange
        new InitCommand().Execute();
        var mockMovingAdapter = new Mock<IMoving>();
        var movingObject = new object();

        var scope = Ioc.Resolve<IDictionary<string, Func<object[], object>>>("IoC.Scope.Create");
        Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", scope).Execute();


        Ioc.Resolve<ICommand>(
                    "IoC.Register",
                   "Commands.Move",
                   (object[] args) => new MoveCommand(Ioc.Resolve<IMoving>("Adapters.IMovingObject", args[0]))
                ).Execute();

                Ioc.Resolve<ICommand>(
                    "IoC.Register",
                    "Adapters.IMovingObject",
                    (object[] args)=>mockMovingAdapter.Object
                    ).Execute();
        

        //Act
         var moveCommand = Ioc.Resolve<ICommand>("Commands.Move", movingObject);

        //Assert
        Assert.IsType<MoveCommand>(moveCommand);
    }
}