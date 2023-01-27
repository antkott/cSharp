namespace QMaticWebBookingParser
{
    public class ParserSettings
    {
        public string City { get; set; }

        public Prague CityPrague { get; set; }

        public Brno CityBrno { get; set; }

        public int AttemptsDelaySec { get; set; }

        public bool HideSeleniumExecution { get; set; }

        public int SearchDepthMonth { get; set; }

        public int ClickDelayMs { get; set; }
    }

    public class Prague
    {
        public string Url { get; set; }

        public string NoPlacesMessage { get; set; }

        public int ServiceNumber { get; set; }
    }

    public class Brno
    {
        public string Url { get; set; }

        public string NoPlacesMessage { get; set; }

        public int ServiceNumber { get; set; }
    }
}
