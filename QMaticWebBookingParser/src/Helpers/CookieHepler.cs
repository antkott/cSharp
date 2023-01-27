using OpenQA.Selenium.Chrome;
using System.Net;
using Cookie = OpenQA.Selenium.Cookie;

namespace QMaticWebBookingParser.Helpers
{
    public static class CookieHepler
    {
        // https://www.selenium.dev/documentation/webdriver/interactions/cookies/

        public static Cookie CloneCookieWithProlongedExpitaion(this Cookie cookie)
        {
            var expiry = cookie.Expiry;
            if (expiry.HasValue) {
                var prolonged = expiry.Value.AddHours(5);
                expiry = prolonged;
            }

            return new Cookie(
                domain: cookie.Domain,
                name: cookie.Name, 
                value: cookie.Value,
                path: cookie.Path,
                expiry: expiry,
                secure: cookie.Secure,
                isHttpOnly: cookie.IsHttpOnly,
                sameSite: cookie.SameSite);
        }

        public static void SpoofCookie(this ChromeDriver driver, string cookieName)
        {
            var cook = driver.Manage().Cookies.GetCookieNamed(cookieName);
            var cookieWithProlongedExpitation = cook.CloneCookieWithProlongedExpitaion();
            driver.Manage().Cookies.DeleteCookie(cook);
            var cook2 = driver.Manage().Cookies.GetCookieNamed(cookieName);
            if (cook2 != null) { throw new DataMisalignedException($"Can't delete cookie {cookieName}"); }
            driver.Manage().Cookies.AddCookie(cookieWithProlongedExpitation);
        }

    }
}
