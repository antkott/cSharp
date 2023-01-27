using FluentAssertions;
using QMaticWebBookingParser.Helpers;
using System.Collections;

namespace QMaticWebBookingParserTests
{
    public class AnswerComparerTests
    {
        [Theory]
        [InlineData(null, "Timeslots dictionary shouldn't be null (Parameter 'timeslots')")]
        public void AnswerComparer_NullInput_ShouldThrow(
            Dictionary<string, string> input, 
            string expMessage)
        {
            Action act = () =>
            {
                input
                .AreFreePlaces(TimeslotsTestData.NoPlacesMessages, out var actualResult);
            };
            act
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(expMessage);
        }

        [Fact]
        public void AnswerComparer_EmptyInput_ShouldThrow()
        {
            Action act = () =>
            {
                new Dictionary<string, string>()
                .AreFreePlaces(TimeslotsTestData.NoPlacesMessages, out var actualResult);
            };
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Timeslots dictionary shouldn't be empty (Parameter 'timeslots')");
        }

        [Theory]
        [ClassData(typeof(TimeslotsTestData))]
        public void AnswerComparer_CheckInputs_ShouldReturn_CorrectResult(
            Dictionary<string, string> values, 
            string result, 
            bool output)
        {
            var actualOutput = values.AreFreePlaces(TimeslotsTestData.NoPlacesMessages, out var actualResult);
            actualOutput.Should().Be(output);
            actualResult.Should().Be(result);
        }
    }
}

public class TimeslotsTestData : IEnumerable<object[]>
{
    public static string NoPlacesMessages => "Нема місць";

    private readonly Dictionary<string, string> timeslotsWithoutPlaces = new()
    {
            { "a", NoPlacesMessages },
            { "b", NoPlacesMessages },
            { "c", NoPlacesMessages }
    };
    private readonly Dictionary<string, string> timeslotsWithPlaces = new()
    {
            { "a", NoPlacesMessages },
            { "b", "21.21.21" },
            { "c", NoPlacesMessages }
    };

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { timeslotsWithoutPlaces, $"c: {NoPlacesMessages}", false };
        yield return new object[] { timeslotsWithPlaces, $"b: 21.21.21", true };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
