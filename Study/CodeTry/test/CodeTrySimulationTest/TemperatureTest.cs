
namespace CodeTryTests
{
    public class TemperatureTest
    {
        [Theory]
        [InlineData(new int[] {}, 0)]
        [InlineData(new int[] {7, 5, 9, 1, 4}, 1)]
        [InlineData(new int[] { -273 }, -273)]
        [InlineData(new int[] { 5526 }, 5526)]
        [InlineData(new int[] { -15, -7, -9, -14, -12 }, -7)]
        [InlineData(new int[] { -10, -10 },-10)]
        [InlineData(new int[] { 15, -7, 9, 14, 7, 12 }, 7)]
        public void ComputeClosestToZeroTest(
            int[] input,
            int expectedOutput)
        {
            var actual = Temperature.ComputeClosestToZero(input);
            actual.Should().Be(expectedOutput);

        }

        [Theory]
        [InlineData(new int[] { }, 0)]
        [InlineData(new int[] { 7, 5, 9, 1, 4 }, 1)]
        [InlineData(new int[] { -273 }, -273)]
        [InlineData(new int[] { 5526 }, 5526)]
        [InlineData(new int[] { -15, -7, -9, -14, -12 }, -7)]
        [InlineData(new int[] { -10, -10 }, -10)]
        [InlineData(new int[] { 15, -7, 9, 14, 7, 12 }, 7)]
        public void ComputeClosestToZero2Test(
            int[] input,
            int expectedOutput)
        {
            var actual = Temperature.ComputeClosestToZero2(input);
            actual.Should().Be(expectedOutput);

        }
    }
}