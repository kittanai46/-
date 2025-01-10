using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V128.WebAuthn;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_MACHINE_VIEW_01
    { 
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestLoginWithIncorrectCredentials()
        {
           // Locate username and password fields and login button again
            var inputUser = driver.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input"));
            var inputPassword = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("login"));

            // Input incorrect credentials
            inputUser.Clear();
            inputPassword.Clear();

            inputUser.SendKeys("nopmontolPN");
            inputPassword.SendKeys("vtm@Promptnow2024");

            loginButton.Click();

            Thread.Sleep(3000);
            var Machine_Management = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[7]/a"));
            Machine_Management.Click();

            var Machine = driver.FindElement(By.XPath("//*[@id=\"ui-id-2\"]"));
            Machine.Click();

            Thread.Sleep(2000);
            var input = driver.FindElement(By.XPath("//*[@id=\"name\"]"));
            input.Clear();
            input.SendKeys("Muhaha");

            Thread.Sleep(3000);
            var search = driver.FindElement(By.XPath("//*[@id=\"SearchBtn\"]"));
            search.Click();

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 100);");
            var view = driver.FindElement(By.XPath("//*[@id=\"frmUser1\"]/div[4]/div/div/fieldset/div/table/tbody/tr[1]/td[6]/a[2]"));
            view.Click();
         
        }

        [TearDown]
        public void Teardown()
        {
            // Uncomment the line below to close the browser after the test
            // driver.Quit();
        }
    }
}
