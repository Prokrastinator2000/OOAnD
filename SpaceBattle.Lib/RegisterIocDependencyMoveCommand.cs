using App;
using SpaceBattle.Lib;
public class RegisterIoCDependencyMoveCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand(Ioc.Resolve<IMoving>("Adapters.IMovingObject", args[0]))).Execute();
    }
}
