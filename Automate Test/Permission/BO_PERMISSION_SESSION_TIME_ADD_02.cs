using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V128.WebAuthn;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_PERMISSION_SESSION_TIME_ADD_02
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
            driver.Navigate().GoToUrl("https://36cf-2001-fb1-b2-7d7f-b86f-88a0-c6db-4dc1.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            // Click visit button if visible
            var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
            if (visitButton.Displayed)
            {
                visitButton.Click();
            }

            // Wait for 3 seconds
            Thread.Sleep(3000);

            // Locate username and password fields and login button again
            var inputUser = driver.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input"));
            var inputPassword = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("login"));

            // Input incorrect credentials
            inputUser.Clear();
            inputPassword.Clear();

            inputUser.SendKeys("admin");
            inputPassword.SendKeys("vtm@Promptnow2024");

            loginButton.Click();

            // Navigate through the menu
            var admin = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[1]/a"));
            admin.Click();

            var permission = driver.FindElement(By.XPath("//*[@id=\"ui-id-2\"]"));
            permission.Click();

            var role = driver.FindElement(By.XPath("//*[@id=\"role\"]"));
            role.Click();

            var test = driver.FindElement(By.XPath("//*[@id=\"role\"]/option[17]"));
            test.Click();

            var s = driver.FindElement(By.XPath("//*[@id=\"onSearchBtn\"]/span"));
            s.Click();

            // Wait and locate the 'edit' button with retry mechanism
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement edit = null;
            bool elementFound = false;
            int attempts = 0;

            while (attempts < 3 && !elementFound)
            {
                try
                {
                    // Locate the 'edit' button again in case the DOM has refreshed
                    edit = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[title='Update']")));

                    // Scroll to the element to make it visible
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", edit);
                    Thread.Sleep(500); // Optional delay

                    if (edit.Displayed)
                    {
                        edit.Click();
                        elementFound = true; // Element was clicked successfully
                    }
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                }
            }

            if (!elementFound)
            {
                Console.WriteLine("Could not find the 'edit' element after multiple attempts.");
            }

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 200);"); // เลื่อนลง 200 พิกเซล
            Thread.Sleep(1000);

            Thread.Sleep(2000); // Optional delay to ensure smooth scrolling
            var userunlock = driver.FindElement(By.XPath("//*[@id=\"search\"]/tbody/tr[3]/td[2]/table/tbody/tr[1]/td[2]/ul[1]/li[5]/input"));
            userunlock.Click();


            // Scroll down to the bottom of the page
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(2000); // Optional delay to ensure smooth scrolling

            // Locate the addper button and scroll to it specifically
            var addper = driver.FindElement(By.XPath("//*[@id=\"editSubmitBtn\"]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addper);
            Thread.Sleep(2000); // Optional delay to ensure smooth scrolling

            addper.Click();

        }

        [TearDown]
        public void Teardown()
        {
            // Uncomment the line below to close the browser after the test
            // driver.Quit();
        }
    }
}
