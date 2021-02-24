
namespace CodeTryTests
{
    public class SummerSalesTest
    {
        [Theory]
        [InlineData(new int[] { 100, 400, 200 }, 20, 620)]
        [InlineData(new int[] { 10, 20, 5, 1, 2 }, 10, 36)]
        [InlineData(new int[] { 1030, 2090, 39 }, 50, 2114)]
        [InlineData(new int[] { 401, 245, 308, 123, 95 }, 23, 1079)]
        [InlineData(new int[] { 20, 40, 99, 15 }, 100, 75)]
        [InlineData(new int[] { 990, 990 }, 40, 1584)]
        [InlineData(new int[] { 100 }, 20, 80)]
        public void CalculateTotalPriceTest(
            int[] input,
            int discount,
            int expectedOutput)
        {
            var actual = SummerSales.CalculateTotalPrice(input, discount);
            actual.Should().Be(expectedOutput);

        }
    }
}