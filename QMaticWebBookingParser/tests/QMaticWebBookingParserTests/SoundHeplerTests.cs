using FluentAssertions;
using QMaticWebBookingParser.Helpers;

namespace QMaticWebBookingParserTests
{
    public class SoundHeplerTests
    {
        [Fact]
        public void SoundComparer_PlayFindAlarm_ShouldNotThrow()
        {
            Action act = () =>
            {
               SoundHelper.PlayFindAlarm();
            };
            act
                .Should()
                .NotThrow();
        }

        [Fact]
        public void SoundComparer_PlayAttention_ShouldNotThrow()
        {
            Action act = () =>
            {
                SoundHelper.PlayAttention();
            };
            act
                .Should()
                .NotThrow();
        }

    }
}
