using App;
namespace SpaceBattle.Lib
{
    public class InitializeTorpedoCommand : ICommand
    {
        private readonly IDictionary<string, object> torpedo;
        private readonly IShooting obj;

        public InitializeTorpedoCommand(IDictionary<string, object> torpedo, IShooting obj)
        {
            this.torpedo = torpedo;
            this.obj = obj;
        }
        public void Execute()
        {
            torpedo["Position"] = obj.Position + obj.Velocity;
            torpedo["Velocity"] = obj.Velocity;
        }
    }
}
