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
    public class BO_CHANGE_PASSWORD_CHANGE_PASSWORD_03
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

            var changpass = driver.FindElement(By.XPath("//*[@id=\"ui-id-3\"]"));
            changpass.Click();

            var old_pass = driver.FindElement(By.XPath("//*[@id=\"password_old\"]"));
            old_pass.SendKeys("vtm@Promptnow2024");

            var new_pass = driver.FindElement(By.XPath("//*[@id=\"password_new\"]"));
            new_pass.SendKeys("vtm@Promptnow2025");

            var new2_pass = driver.FindElement(By.XPath("//*[@id=\"password_confirm\"]"));
            new2_pass.SendKeys("vtm@Promptnow2025");


            Thread.Sleep(900000); 


            var save = driver.FindElement(By.XPath("//*[@id=\"processChangePwdSubmitBtn\"]"));
            save.Click();

        }

        [TearDown]
        public void Teardown()
        {
            // Uncomment the line below to close the browser after the test
            // driver.Quit();
        }
    }
}
