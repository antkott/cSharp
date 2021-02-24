using Microsoft.Extensions.Logging;
using System.Globalization;

namespace QMaticWebBookingParser
{
    public static class AnswerComparer
    {
        public static bool AreFreePlaces(
            this Dictionary<string, string> timeslots,
            string noPlacesString,
            ILogger logger,
            out string? result)
        {
            result = null;
            var uaComparer = StringComparer.Create(new CultureInfo("uk-UA"), true);
            foreach (KeyValuePair<string, string> entry in timeslots)
            {
                result = $"{entry.Key}: {entry.Value}";
                if (uaComparer.Compare(entry.Value, noPlacesString) < 0)
                {
                    logger.LogWarning(result);
                    return true;
                }
                else
                {
                    logger.LogInformation(result);
                }
            }
            return false;
        }
    }
}
