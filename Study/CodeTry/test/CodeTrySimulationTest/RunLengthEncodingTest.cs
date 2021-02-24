namespace CodeTryTests
{
    public class RunLengthEncodingTest
    {

        public static IEnumerable<object[]> RunLengthEncodingData =>
        new List<object[]>
        {
            new object[] {
                new List<string> {
                "AAA",
                "BBB",
                "CCC"
                }, "3A3B3C" },
            new object[] {
                new List<string> {
                "AAB",
                "CCC",
                "BAA"
                }, "2AB3CB2A" }
        };

        [Theory]
        [MemberData(nameof(RunLengthEncodingData))]
        public void EncodeList(
            IEnumerable<string> input,
            string expectedOutput)
        {
            var actualOutput = RunLengthEncoding.EncodeList(input);
            expectedOutput.Should().Be(actualOutput);
        }

    }
}
