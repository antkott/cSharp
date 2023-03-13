using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V107.DOMSnapshot;
using OpenQA.Selenium.Support.UI;
using QMaticWebBookingParser.Helpers;
using System.Diagnostics;
using System.Media;
using System.Net;
using Cookie = OpenQA.Selenium.Cookie;

namespace QMaticWebBookingParser
{
    public class Parser : IDisposable
    {
        private readonly ParserSettings _parserSettings;
        private readonly ChromeDriverService _chromeDriverService;
        private readonly ChromeDriver _driver;
        private bool _pageAlreadyOpened;
        private bool _parisComplainCoverShown;
        private bool _disposed;

        public Parser(
            ParserSettings parserSettings)
        {
            _chromeDriverService = ChromeDriverService.CreateDefaultService();
            _chromeDriverService.SuppressInitialDiagnosticInformation = true;
            _chromeDriverService.HideCommandPromptWindow = true;

            var _silentOptions = new ChromeOptions();
            _silentOptions.AddArgument("--silent");
            _silentOptions.AddArgument("headless");
            _silentOptions.AddArgument("log-level=3");
            _parserSettings = parserSettings;

            var proxyOptions = new ChromeOptions();
            //var proxy = new Proxy
            //{
            //    Kind = ProxyKind.Manual,
            //    IsAutoDetect = false,
            //    SocksUserName = "freeopenvpn",
            //    SocksPassword= "428451887",
            //    SslProxy = "us1.freeopenvpn.org:443",
            //    SocksProxy = "us1.freeopenvpn.org:443"
            //};
            //proxyOptions.Proxy = proxy;
            proxyOptions.AddArgument("ignore-certificate-errors");
            proxyOptions.AddExtension(@"extension_3_68_0_0.crx");

            _driver = _parserSettings.HideSeleniumExecution ?
               new ChromeDriver(_chromeDriverService, _silentOptions) :
               new ChromeDriver(_chromeDriverService, proxyOptions);
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
            if (_parserSettings.City.Equals("Pari"))
            {
                return await ParsePari(_driver, dic);
            }
            throw new NotImplementedException($"City {_parserSettings.City} not implemented yet");
        }

        public async Task<Dictionary<string, string>> ParsePrague(
            ChromeDriver driver,
            Dictionary<string, string> resultDictionary)
        {
            WebDriverWait waitForElement = new(driver, TimeSpan.FromSeconds(30));
            waitForElement.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            waitForElement.IgnoreExceptionTypes(typeof(ElementNotInteractableException));
            if (_pageAlreadyOpened)
            {
                driver.Navigate().Refresh();
            }
            else
            {
                driver.Navigate().GoToUrl(_parserSettings.CityPrague.Url);
                _pageAlreadyOpened = true;
            }
            driver.ExecuteScript($"document.title = '{_parserSettings.City}|Parser'");
            WaitTillElementAppeared(waitForElement, className: "process-step-name");

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
            WebDriverWait waitForElement = new(driver, TimeSpan.FromSeconds(30));
            waitForElement.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            waitForElement.IgnoreExceptionTypes(typeof(ElementNotInteractableException));

            if (!_pageAlreadyOpened)
            {
                driver.Navigate().GoToUrl(_parserSettings.CityBrno.Url);
                driver.ExecuteScript($"document.title = '{_parserSettings.City}|Parser'");
                WaitTillElementAppeared(waitForElement, className: "MuiInput-root");
                if (!_parisComplainCoverShown)
                {
                    //complain cover
                    await FindClickWait(@"(//span[contains(@class,'MuiButton-label')])[2]");
                    _parisComplainCoverShown = true;
                }

                //country
                await FindClickWait(@"//input[@name='country']");
                await FindClickWait(@"//li[contains(@data-option-index,'78')]");

                //recaptcha
                driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("iframe[src*='recaptcha']")));
                WebDriverWait waitForElementReCaptcha = new(driver, TimeSpan.FromMinutes(5));
                WaitTillElementAppeared(waitForElementReCaptcha, className: null, xPath: "//label[contains(.,'Я не робот')]");
                await FindClickWait(@"//div[@class='recaptcha-checkbox-border']");

                //consulate
                WaitTillElementAppeared(waitForElement, className: null, xPath: "//input[@name='consulate']");
                await FindClickWait(@"//input[@name='consulate']");
                await FindClickWait(@"//li[contains(@id,'consulates-option-0')]");

                driver.SpoofCookie("__cf_bm");
            }

            if (_pageAlreadyOpened)
            {
                // change templorary catergory & service to different for refresh page wihout reCaptcha
                await FindClickWait(@"//input[contains(@name,'category')]");
                await FindClickWait(@"//li[contains(@id,'categories-option-1')]");
                try
                {
                    await FindClickWait(@"//input[@name='service']");
                    await FindClickWait(@"//li[contains(@id,'services-option-1')]");
                }
                catch
                {
                    _pageAlreadyOpened = false;
                    SoundHelper.PlayAttention();
                    resultDictionary.Add(string.Empty, _parserSettings.CityBrno.NoPlacesMessage);
                    return resultDictionary;
                }
            }
            //category
            await FindClickWait(@"//input[contains(@name,'category')]");
            await FindClickWait(@"//li[contains(@id,'categories-option-0')]");

            //services
            await FindClickWait(@"//input[@name='service']");
            await FindClickWait(@"//li[contains(@id,'services-option-0')]");

            var noPlaces = WaitTillElementAppeared(waitForElement, className: null, xPath: @$"//p[contains(.,'{_parserSettings.CityBrno.NoPlacesMessage}')]");
            if (noPlaces == null)
            {
                resultDictionary.Add(string.Empty, "ALARM! Places found");
            }
            else
            {
                resultDictionary.Add(string.Empty, noPlaces.Text);
            }

            _pageAlreadyOpened = true;
            return resultDictionary;
        }

        public async Task<Dictionary<string, string>> ParsePari(
            ChromeDriver driver,
            Dictionary<string, string> resultDictionary)
        {
            WebDriverWait waitForElement = new(driver, TimeSpan.FromSeconds(30));
            waitForElement.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            waitForElement.IgnoreExceptionTypes(typeof(ElementNotInteractableException));

            if (!_pageAlreadyOpened)
            {
                Console.WriteLine("TurnOn VPN and press ENTER");
                Console.ReadKey();
                driver.Navigate().GoToUrl(_parserSettings.CityPari.Url);
               

                driver.ExecuteScript($"document.title = '{_parserSettings.City}|Parser'");
                WaitTillElementAppeared(waitForElement, className: "MuiInput-root");
                if (!_parisComplainCoverShown)
                {
                    //complain cover
                    await FindClickWait(@"(//span[contains(@class,'MuiButton-label')])[2]");
                    _parisComplainCoverShown = true;
                }

                //country
                await FindClickWait(@"//input[@name='country']");
                await FindClickWait(@"//li[contains(@data-option-index,'76')]");

                //recaptcha
                Thread.Sleep(3000);
                driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("iframe[src*='recaptcha']")));
                WebDriverWait waitForElementReCaptcha = new(driver, TimeSpan.FromMinutes(5));
                WaitTillElementAppeared(waitForElementReCaptcha, className: null, xPath: "//label[contains(.,'Я не робот')]");
                await FindClickWait(@"//div[@class='recaptcha-checkbox-border']");

                //consulate
                WaitTillElementAppeared(waitForElement, className: null, xPath: "//input[@name='consulates']");
                await FindClickWait(@"//input[@name='consulates']");
                await FindClickWait(@"//li[contains(@id,'consulates-option-0')]");

                driver.SpoofCookie("__cf_bm");
            }

            if (_pageAlreadyOpened)
            {
                // change templorary catergory & service to different for refresh page wihout reCaptcha
                await FindClickWait(@"//input[contains(@name,'category')]");
                await FindClickWait(@"//li[contains(@id,'categories-option-1')]");
                try
                {
                    await FindClickWait(@"//input[@name='service']");
                    await FindClickWait(@"//li[contains(@id,'services-option-1')]");
                }
                catch
                {
                    _pageAlreadyOpened = false;
                    SoundHelper.PlayAttention();
                    resultDictionary.Add(string.Empty, _parserSettings.CityBrno.NoPlacesMessage);
                    return resultDictionary;
                }
            }
            //category
            await FindClickWait(@"//input[contains(@name,'category')]");
            await FindClickWait(@"//li[contains(@id,'categories-option-0')]");

            //services
            await FindClickWait(@"//input[@name='service']");
            await FindClickWait(@"//li[contains(@id,'services-option-0')]");

            var noPlaces = WaitTillElementAppeared(waitForElement, className: null, xPath: @$"//p[contains(.,'{_parserSettings.CityBrno.NoPlacesMessage}')]");
            if (noPlaces == null)
            {
                resultDictionary.Add(string.Empty, "ALARM! Places found");
            }
            else
            {
                resultDictionary.Add(string.Empty, noPlaces.Text);
            }
            _pageAlreadyOpened = true;
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

        private IWebElement? WaitTillElementAppeared(
            WebDriverWait waitForElement,
            string? className,
            string? xPath = null)
        {
            if (!string.IsNullOrEmpty(className)
                && waitForElement.Until(e => e.FindElements(By.ClassName($@"{className}")).Any()))
            {
                return _driver.FindElement(By.ClassName($@"{className}"));
            }
            if (!string.IsNullOrEmpty(xPath)
                && waitForElement.Until(e => e.FindElements(By.XPath($@"{xPath}")).Any()))
            {
                return FindClickWait(xPath, false)?.Result;
            }
            return null;
        }


    }
}
