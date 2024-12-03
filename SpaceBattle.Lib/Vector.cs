
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
        for (var i = 0; i < a.Values.Length; i++)
        {
            a.Values[i] += b.Values[i];
        }

        return a;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec vec && Values.SequenceEqual(vec.Values);
    }

    public override int GetHashCode()
{
    
    return Values.Aggregate(17, (hash, value) => hash * 31 + value.GetHashCode());
}
}
