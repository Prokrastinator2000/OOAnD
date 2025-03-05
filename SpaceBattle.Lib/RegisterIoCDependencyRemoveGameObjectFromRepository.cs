using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyRemoveGameObjectFromRepository : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Object.Remove",
            new Func<object[], object>((object[] args) =>
            {
                ((IDictionary<string, object>)Ioc.Resolve<object>("Game.Object.Repository")).Remove((string)args[0]);
                return true;
            }
        )).Execute();
    }
}
