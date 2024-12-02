namespace SpaceBattle.Lib
{
    public class Angle
    {
        public int a { get; set; }
        public int n { get; }

        public Angle(int x1, int x2)
        {
            a = x1;
            n = x2;
        }

        public void SetAngle(int newAngle)
        {
            a = newAngle;
        }
    }
}
