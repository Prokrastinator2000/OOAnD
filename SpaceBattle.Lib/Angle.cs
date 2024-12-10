namespace SpaceBattle.Lib
{
    public class Angle
    {
        public int a { get; set; }
        public int n { get; }

        public Angle(int x1, int x2)
        {
            a = x1 % x2;
            n = x2;
        }
        public Angle(Angle t)
        {
            a = t.a % t.n;
            n = t.n;
        }

        public void SetAngle(int newAngle)
        {
            a = newAngle % n;
        }
        public static Angle operator +(Angle a, Angle b)
        {
            //if (a.n != b.n)
            //{
            //throw new Exception("Different n");
            //}

            return new Angle(a.a + b.a, a.n);
        }

        public double Sin()
        {
            return Math.Sin(2 * Math.PI * a / n);
        }

        public double Cos()
        {
            return Math.Cos(2 * Math.PI * a / n);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Angle other)
            {
                return a == other.a && n == other.n;
            }

            return false;
        }

        public static bool operator ==(Angle a, Angle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Angle a, Angle b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return (a, n).GetHashCode();
        }
    }
}
