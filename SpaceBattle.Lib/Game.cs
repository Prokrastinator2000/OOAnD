using App;
namespace SpaceBattle.Lib;

public class Game : ICommand
{
    private readonly object gameScope;
    public Game(object gameScope)
    {
        var oldScope = Ioc.Resolve<object>("IoC.Scope.Current");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", gameScope).Execute();
        var regNextCommand = new RegisterIoCDependencyNextCommand();
        regNextCommand.Execute();
        this.gameScope = gameScope;
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", oldScope).Execute();
    }

    public void Execute()
    {
        var oldScope = Ioc.Resolve<object>("IoC.Scope.Current");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", gameScope).Execute();

        while ((Ioc.Resolve<IStopFlag>("Game.StopFlag")).GetStopFlag())
        {
            var cmd = Ioc.Resolve<ICommand>("Commands.GetNextCommand");
            try
            {
                cmd.Execute();
            }
            catch (Exception exception)
            {
                Ioc.Resolve<object>("Game.ExceptionHandle", exception);
            }
        }

        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", oldScope).Execute();
    }
}
