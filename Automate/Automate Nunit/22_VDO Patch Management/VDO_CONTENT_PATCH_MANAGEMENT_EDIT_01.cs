using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;

using System.Threading;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class VDO_CONTENT_PATCH_MANAGEMENT_EDIT_01
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
            driver.Navigate().GoToUrl("https://f9f9-2001-fb1-b0-2184-b1e7-2215-6134-e8ac.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

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

            inputUser.SendKeys("nopmontolPN");
            inputPassword.SendKeys("vtm@Promptnow2024");

            loginButton.Click();


            Thread.Sleep(3000);
            var Path = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[12]/a"));
            Path.Click();

            Thread.Sleep(2000);
            var vdo = driver.FindElement(By.XPath("//*[@id=\"ui-id-1\"]"));
            vdo.Click();

            Thread.Sleep(3000);

            var edit = driver.FindElement(By.XPath("//*[@id=\"frmUser1\"]/div[5]/div/div/fieldset/div/table/tbody/tr/td[8]/a[1]"));
            edit.Click();

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            var desdription = driver.FindElement(By.XPath("//*[@id=\"description\"]"));
            desdription.SendKeys("Hello this is a test nakub");

            js.ExecuteScript("window.scrollBy(0, 100);");

            // รอเล็กน้อยเพื่อให้มั่นใจว่าทุกอย่างโหลดเรียบร้อย
            Thread.Sleep(1000);


            var save = driver.FindElement(By.XPath("//*[@id=\"editBtn\"]"));
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
