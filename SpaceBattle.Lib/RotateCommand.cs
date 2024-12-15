namespace SpaceBattle.Lib
{
    public class RotateCommand : ICommand
    {
        private readonly IRotating rotating;

        public RotateCommand(IRotating rotating)
        {
            this.rotating = rotating;
        }

        public virtual void Execute()
        {
            rotating.Angle = new Angle(rotating.Angle + rotating.Velocity);
        }
    }
}
