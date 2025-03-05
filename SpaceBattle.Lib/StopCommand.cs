using App;

namespace SpaceBattle.Lib
{
    public class StopCommand : ICommand
    {
        private readonly string _signalType;
        private readonly string _targetId;

        public StopCommand(IDictionary<string, object> terminationDetails)
        {
            _signalType = (string)terminationDetails["Action"];
            _targetId = (string)terminationDetails["Key"];
        }

        public void Execute()
        {
            var managedObject = Ioc.Resolve<IDictionary<string, object>>("Game.Object", _targetId);
            var configurable = (ICommandInjectable)managedObject[_signalType];
            var cancellationSignal = Ioc.Resolve<ICommand>("Commands.Empty");

            configurable.Inject(cancellationSignal);
        }
    }
}
