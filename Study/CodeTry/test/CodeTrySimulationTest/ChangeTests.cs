namespace CodeTryTests
{
    public class ChangeTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, false)]
        [InlineData(1, 0, 0, 0, true)]
        [InlineData(2, 1, 0, 0, false)]
        [InlineData(3, 1, 0, 0, true)]
        [InlineData(5, 0, 1, 0, false)]
        [InlineData(6, 3, 0, 0, false)]
        [InlineData(7, 1, 1, 0, false)]
        [InlineData(8, 4, 0, 0, false)]
        [InlineData(9, 2, 1, 0, false)]
        [InlineData(10, 0, 0, 1, false)]
        [InlineData(11, 3, 1, 0, false)]
        [InlineData(12, 1, 0, 1, false)]
        [InlineData(13, 4, 1, 0, false)]
        [InlineData(31, 3, 1, 2, false)]
        [InlineData(9223372036854775802, 1, 0, 922337203685477580L, false)]
        [InlineData(9223372036854775803, 4, 1, 922337203685477579L, false)]
        // [InlineData(long.MaxValue, 1, 1, 9223372036854775797L, false)]
        public void OptimalChangeTest(
            long input,
            long expectedCoin2,
            long expectedBill5,
            long expectedBill10,
            bool isNullReturn)
        {
            Change m = ChangeSolution.OptimalChange(input);
            if (!isNullReturn)
            {
                if (input != long.MaxValue)
                {
                    input.Should().Be(expectedCoin2 * 2 + expectedBill5 * 5 + expectedBill10 * 10);
                }
                (m.coin2 * 2 + m.bill5 * 5 + m.bill10 * 10).Should().Be(input);

                m.coin2.Should().Be(expectedCoin2);
                m.bill5.Should().Be(expectedBill5);
                m.bill10.Should().Be(expectedBill10);

            }
            else
            {
                m.Should().BeNull();
            }

        }

        [Theory]
        [InlineData(0, 0, 0, 0, false)]
        [InlineData(1, 0, 0, 0, true)]
        [InlineData(2, 1, 0, 0, false)]
        [InlineData(3, 1, 0, 0, true)]
        [InlineData(5, 0, 1, 0, false)]
        [InlineData(6, 3, 0, 0, false)]
        [InlineData(7, 1, 1, 0, false)]
        [InlineData(8, 4, 0, 0, false)]
        [InlineData(9, 2, 1, 0, false)]
        [InlineData(10, 0, 0, 1, false)]
        [InlineData(11, 3, 1, 0, false)]
        [InlineData(12, 1, 0, 1, false)]
        [InlineData(13, 4, 1, 0, false)]
        [InlineData(31, 3, 1, 2, false)]
        [InlineData(9223372036854775802, 1, 0, 922337203685477580L, false)]
        [InlineData(9223372036854775803, 4, 1, 922337203685477579L, false)]
        // [InlineData(long.MaxValue, 1, 1, 9223372036854775797L, false)]
        public void OptimalChange2Test(
           long input,
           long expectedCoin2,
           long expectedBill5,
           long expectedBill10,
           bool isNullReturn)
        {
            Change m = ChangeSolution.OptimalChange2(input);
            if (!isNullReturn)
            {
                if (input != long.MaxValue)
                {
                    input.Should().Be(expectedCoin2 * 2 + expectedBill5 * 5 + expectedBill10 * 10);
                }
                (m.coin2 * 2 + m.bill5 * 5 + m.bill10 * 10).Should().Be(input);

                m.coin2.Should().Be(expectedCoin2);
                m.bill5.Should().Be(expectedBill5);
                m.bill10.Should().Be(expectedBill10);

            }
            else
            {
                m.Should().BeNull();
            }

        }
    }
}