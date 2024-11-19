using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_PERMISSION_EDIT_01
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

            var edit = driver.FindElement(By.XPath("//*[@id=\"frmRole1\"]/div[5]/div/div/fieldset/div/table/tbody/tr/td[8]/a[1]"));
            edit.Click();

            var tik = driver.FindElement(By.XPath("//*[@id=\"search\"]/tbody/tr[3]/td[2]/table/tbody/tr[1]/td[2]/ul[1]/li[2]/input"));
            tik.Click();

            // Scroll down to the addper button before clicking it
            var addper = driver.FindElement(By.XPath("//*[@id=\"addSubmitBtn\"]/span"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addper);
            Thread.Sleep(500); // Optional delay to ensure smooth scrolling

            addper.Click();
        }

        [TearDown]
        public void Teardown()
        {
            //driver.Quit();
        }
    }
}
