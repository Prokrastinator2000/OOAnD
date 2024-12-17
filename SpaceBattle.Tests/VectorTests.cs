
using SpaceBattle.Lib;
namespace SpaceBattle.Tests;
public class VectorTests
{
    [Fact]
    public void Vec_AdditionFirstVecDimBigger_ThrowsArgumentException()
    {
        var vec1 = new Vec(new int[] { 1, 2, 3 });
        var vec2 = new Vec(new int[] { 1, 2 });
        Assert.Throws<ArgumentException>(() => vec1 + vec2);
    }
    [Fact]
    public void Vec_AdditionFirstVecDimSmaller_ThrowsArgumentException()
    {
        var vec1 = new Vec(new int[] { 1, 2 });
        var vec2 = new Vec(new int[] { 1, 2, 3 });
        Assert.Throws<ArgumentException>(() => vec1 + vec2);
    }
    [Fact]
    public void Vec_AdditionCorrect()
    {
        var vec1 = new Vec(new int[] { 1, -1, 2 });
        var vec2 = new Vec(new int[] { -1, 1, -2 });
        var expected = new Vec(new int[] { 0, 0, 0 });
        Assert.True((vec1 + vec2).Equals(expected));
    }
    [Fact]
    public void Vec_Equals_ShouldReturnTrueForEqualVectors()
    {
        var vec1 = new Vec(new int[] { 1, 2 });
        var vec2 = new Vec(new int[] { 1, 2 });
        Assert.True(vec1.Equals(vec2));
    }

    [Fact]
    public void Vec_Equals_ShouldReturnFalseForDifferentVectors()
    {
        var vec1 = new Vec(new int[] { 1, 2 });
        var vec2 = new Vec(new int[] { 4, 5 });
        Assert.False(vec1.Equals(vec2));
    }
    [Fact]
    public void Vec_Equals_ShouldReturnFalseForNonVecObjects()
    {
        var vec = new Vec(new int[] { 1, 2, 3 });
        var notAVec = new object();
        Assert.False(vec.Equals(notAVec));
    }
    [Fact]
    public void Vec_Equality_ShouldReturnTrueForEqualVectors()
    {
        var vec1 = new Vec(new int[] { 1, 2, 4 });
        var vec2 = new Vec(new int[] { 1, 2, 4 });
        Assert.True(vec1 == vec2);
    }

    [Fact]
    public void Vec_Unequality_ShoulReturnTrueForUnequalVectors()
    {
        var vec1 = new Vec(new int[] { 1, 7 });
        var vec2 = new Vec(new int[] { 5, 8 });
        Assert.True(vec1 != vec2);
    }

    [Fact]
    public void Vec_Equality_ShouldReturnFalseForDifferentSizeVectors()
    {
        var vec1 = new Vec(new int[] { 1, 2 });
        var vec2 = new Vec(new int[] { 1, 2, 3 });
        Assert.False(vec1 == vec2);
    }

    [Fact]
    public void Vec_Equality_ShouldReturnTrueForVectorsWithOneInstance()
    {
        var expected = new Vec(new int[] { 1, 2, 5 });
        var vec1 = expected;
        var vec2 = expected;
        Assert.True(vec1 == vec2);
    }

    [Fact]
    public void Vec_GetHashCode_ShouldReturnSameHashForEqualVectors()
    {
        var vec1 = new Vec(new int[] { 1, 2, 3 });
        var vec2 = new Vec(new int[] { 1, 2, 3 });
        Assert.Equal(vec1.GetHashCode(), vec2.GetHashCode());
    }
}
