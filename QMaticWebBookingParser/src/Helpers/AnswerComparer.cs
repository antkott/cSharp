using System.Diagnostics.Contracts;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace QMaticWebBookingParser.Helpers
{
    public static class AnswerComparer
    {
        private static readonly StringComparer UA_COMPARER = StringComparer.Create(new CultureInfo("uk-UA"), true);

        public static bool AreFreePlaces(
            this Dictionary<string, string> timeslots,
            string noPlacesString,
            out string? result)
        {
            result = null;
            if (timeslots == null)
            {
                throw new ArgumentNullException(nameof(timeslots), "Timeslots dictionary shouldn't be null");
            }
            if (!timeslots.Any())
            {
                throw new ArgumentException("Timeslots dictionary shouldn't be empty", nameof(timeslots));
            }

            foreach (KeyValuePair<string, string> entry in timeslots)
            {
                if (string.IsNullOrEmpty(entry.Key))
                {
                    result = entry.Value;
                }
                else
                {
                    result = $"{entry.Key}: {entry.Value}";
                }
                if (UA_COMPARER.Compare(entry.Value, noPlacesString) < 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
