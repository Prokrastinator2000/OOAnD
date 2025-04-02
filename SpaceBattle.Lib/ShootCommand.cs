using App;
namespace SpaceBattle.Lib
{
    public class ShootCommand : ICommand
    {
        private readonly IShooting obj;
        public ShootCommand(IShooting obj)
        {
            this.obj = obj;
        }
        public void Execute()
        {
            var Torpedo = Ioc.Resolve<object>("Game.Objects.GetTorpedo");

            Ioc.Resolve<ICommand>("Game.Commands.InitializeTorpedo", Torpedo, obj).Execute();

            Ioc.Resolve<ICommand>("Commands.Move", Torpedo).Execute();

        }
    }
}
