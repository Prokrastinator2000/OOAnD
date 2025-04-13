using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyActionsStop : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<ICommand>(
                "IoC.Register",
                "Actions.Stop",
                (object[] order) => new StopCommand((IDictionary<string, object>)order[0])
            ).Execute();
        }
    }
}
