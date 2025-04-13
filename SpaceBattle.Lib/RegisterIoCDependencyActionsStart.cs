using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyActionsStart : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "Actions.Start",
            (object[] orderData) => new StartCommand((IDictionary<string, object>)orderData[0])
        ).Execute();
    }
}
