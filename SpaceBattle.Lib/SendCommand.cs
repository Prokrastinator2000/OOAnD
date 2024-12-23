using App;
namespace SpaceBattle.Lib
{
    public class SendCommand : ICommand
    {
        private readonly ICommand cmds;
        private readonly ICommandReceiver Ireceiver;

        public SendCommand(ICommand cmds, ICommandReceiver Ireceiver)
        {
            this.cmds = cmds;
            this.Ireceiver = Ireceiver;
        }
        public virtual void Execute()
        {

            Ireceiver.Receive(cmds);

        }
    }
}
