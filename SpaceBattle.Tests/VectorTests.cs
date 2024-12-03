
using SpaceBattle.Lib;
namespace SpaceBattle.Tests;
public class VectorTests
{
    [Fact]
   public void Vec_Equals_ShouldReturnTrueForEqualVectors()
        {
            var vec1 = new Vec(new int[] { 1, 2});
            var vec2 = new Vec(new int[] { 1, 2});
            Assert.True(vec1.Equals(vec2));
        }

        [Fact]
        public void Vec_Equals_ShouldReturnFalseForDifferentVectors()
        {
            var vec1 = new Vec(new int[] { 1, 2});
            var vec2 = new Vec(new int[] { 4, 5});
            Assert.False(vec1.Equals(vec2));
        }
        [Fact]
    public void Vec_Equals_ShouldReturnFalseForNonVecObjects()
    {
        var vec = new Vec(new int[] { 1, 2, 3 });
        var notAVec = new object(); // Не Vec
        Assert.False(vec.Equals(notAVec));
    }
        
        [Fact]
        public void Vec_GetHashCode_ShouldReturnSameHashForEqualVectors()
        {
            var vec1 = new Vec(new int[] { 1, 2, 3 });
            var vec2 = new Vec(new int[] { 1, 2, 3 });
            Assert.Equal(vec1.GetHashCode(), vec2.GetHashCode());
        }
}