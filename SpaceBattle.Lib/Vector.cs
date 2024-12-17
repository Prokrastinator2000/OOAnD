
namespace SpaceBattle.Lib;
public class Vec
{
    public int[] Values { get; set; }
    public Vec(int[] Values)
    {
        this.Values = Values;
    }

    public static Vec operator +(Vec a, Vec b)
    {
        if (a.Values.Length != b.Values.Length)
        {
            throw new ArgumentException("Vectors dimensions are different");
        }

        var SumValues = a.Values.Zip(b.Values, (x, y) => x + y).ToArray();
        return new Vec(SumValues);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec vec && Values.SequenceEqual(vec.Values);
    }

    public override int GetHashCode()
    {

        return Values.Aggregate(17, (hash, value) => hash * 31 + value.GetHashCode());
    }
    public static bool operator ==(Vec a, Vec b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a.Values.Length != b.Values.Length)
        {
            return false;
        }

        return a.Values.SequenceEqual(b.Values);
    }

    public static bool operator !=(Vec a, Vec b)
    {
        return !(a == b);
    }
}
