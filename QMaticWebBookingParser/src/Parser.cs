using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace QMaticWebBookingParser
{
    public class Parser : IDisposable
    {
        private readonly ParserSettings _parserSettings;
        private readonly ChromeOptions _silentOptions;
        private readonly ChromeDriverService _chromeDriverService;
        private readonly ChromeDriver _driver;
        private bool _pageAlreadyOpened;
        private bool _disposed;

        public Parser(
            ParserSettings parserSettings)
        {
            _chromeDriverService = ChromeDriverService.CreateDefaultService();
            _chromeDriverService.SuppressInitialDiagnosticInformation = true;
            _chromeDriverService.HideCommandPromptWindow = true;

            _silentOptions = new ChromeOptions();
            _silentOptions.AddArgument("--silent");
            _silentOptions.AddArgument("headless");
            _silentOptions.AddArgument("log-level=3");
            _parserSettings = parserSettings;

            _driver = _parserSettings.HideSeleniumExecution ?
               new ChromeDriver(_chromeDriverService, _silentOptions) :
               new ChromeDriver(_chromeDriverService);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _chromeDriverService?.Dispose();
            _driver?.Quit();
            _driver?.Dispose();
        }

        public static void KillChromeDrivers()
        {
            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }
        }


        public async Task<Dictionary<string, string>> Parse(CancellationToken cancellationToken)
        {
            var dic = new Dictionary<string, string>();
            if (cancellationToken.IsCancellationRequested)
            {
                return dic;
            }

            if (_parserSettings.City.Equals("Prague"))
            {
                return await ParsePrague(_driver, dic);
            }
            if (_parserSettings.City.Equals("Brno"))
            {
                return await ParseBrno(_driver, dic);
            }
            throw new NotImplementedException($"City {_parserSettings.City} not implemented yet");
        }

        public async Task<Dictionary<string, string>> ParsePrague(
            ChromeDriver driver,
            Dictionary<string, string> resultDictionary)
        {
            if (_pageAlreadyOpened)
            {
                driver.Navigate().Refresh();
            }
            else
            {
                driver.Navigate().GoToUrl(_parserSettings.CityPrague.Url);
                _pageAlreadyOpened = true;
            }

            driver.Url = _parserSettings.CityPrague.Url;
            await Task.Delay(_parserSettings.ClickDelayMs);
            WebDriverWait waitForElement = new(driver, TimeSpan.FromSeconds(30));
            waitForElement.Until(e => e.FindElement(By.ClassName("process-step-name")));

            //branch cover
            await FindClickWait("(//div[@class='flex'])[4]");
            //branch
            await FindClickWait(@"//div[@webid='branchId1'][contains(.,'Прага / Prague')]");
            //service
            await FindClickWait(@$"(//div[@class='v-input--selection-controls__ripple'])[{_parserSettings.CityPrague.ServiceNumber}]");
            //dateTime
            await FindClickWait(@"(//div[contains(@class,'process-step-name')])[3]");

            for (int i = 0; i <= _parserSettings.SearchDepthMonth; i++)
            {
                var timeSlotsFound = false;
                var monthYear = FindClickWait(@"(//button[contains(@type,'button')])[12]", false).Result?.Text.Trim();
                var timeslots = FindClickWait("//div[@class='timeslots']", false).Result?.GetAttribute("textContent").Trim();
                Console.Write($"{monthYear} ");
                if (i != 0)
                {
                    for (int j = 1; j <= 31; j++)
                    {
                        var dateTimeCells = driver.FindElements(By.XPath(@$"//td[contains(.,'{j}')]"));
                        var dateTimeCell = (dateTimeCells.Count >= 1) ? dateTimeCells.First() : null;
                        dateTimeCell?.Click();
                        await Task.Delay(10);
                        var dateTimeCellSelected = dateTimeCell?.Selected;
                        var dateTimeCellText = dateTimeCell?.Text ?? string.Empty;
                        // Console.Write($" {dateTimeCellText}");
                        if (dateTimeCellSelected.HasValue && dateTimeCellSelected.Value)
                        {
                            timeslots = driver.FindElement(By.XPath("//div[@class='timeslots']"))
                            .GetAttribute("textContent").Trim();
                            var foundTime = $"{monthYear} {dateTimeCellText} {timeslots}";
                            resultDictionary.Add(foundTime, foundTime);
                            timeSlotsFound = true;
                        }
                    }
                }
                if (!timeSlotsFound)
                {
                    resultDictionary.Add(monthYear, timeslots);
                }
                // selectMonth
                await FindClickWait(@"//i[contains(.,'chevron_right')]");
            }
            Console.WriteLine();
            return resultDictionary;
        }

        public async Task<Dictionary<string, string>> ParseBrno(
            ChromeDriver driver,
            Dictionary<string, string> resultDictionary)
        {
            driver.Url = _parserSettings.CityBrno.Url;
            await Task.Delay(_parserSettings.ClickDelayMs);
            WebDriverWait waitForElement = new(driver, TimeSpan.FromSeconds(30));
            waitForElement.Until(e => e.FindElement(By.ClassName("MuiInput-root")));

            var complainCover = driver.FindElement(By.XPath(@"(//span[contains(@class,'MuiButton-label')])[2]"));
            complainCover?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs);

            string country = @"Чехія";
            var countryDropDownList = driver.FindElement(By.XPath(@"//input[@name='country']"));
            countryDropDownList?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs);
            var countryFromDropDownList = driver.FindElement(By.XPath(@"//li[contains(@data-option-index,'78')]"));
            countryFromDropDownList?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs + 1500);

            driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("iframe[src*='recaptcha']")));
            waitForElement.Until(e => e.FindElement(By.XPath(@"//label[contains(.,'Я не робот')]")));
            var recaptcha = driver.FindElement(By.XPath(@"//div[@class='recaptcha-checkbox-border']"));
            recaptcha?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs + 1500);

            var consulatDropDownList = driver.FindElement(By.XPath(@"//input[@name='consulate']"));
            consulatDropDownList?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs);
            var consulatFromDropDownList = driver.FindElement(By.XPath(@"//li[contains(@id,'consulates-option-0')]"));
            consulatFromDropDownList?.Click();
            await Task.Delay(_parserSettings.ClickDelayMs);


            Thread.Sleep(TimeSpan.FromSeconds(30));

            //SelectElement dropDown = new SelectElement(countryDropDownList);
            //dropDown.SelectByValue(country);
            return resultDictionary;
        }

        private async Task<IWebElement?> FindClickWait(string xPath, bool click = true)
        {
            var webElement = _driver.FindElement(By.XPath(xPath));
            if (click)
            {
                webElement?.Click();
                await Task.Delay(_parserSettings.ClickDelayMs);
            }
            return webElement;
        }
    }
}
