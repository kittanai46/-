using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_ADMIN_USER๘SESSION_TIME_OUT_01
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
        public void TestLoginAndNavigate()
        {
            driver.Navigate().GoToUrl("https://36cf-2001-fb1-b2-7d7f-b86f-88a0-c6db-4dc1.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");


            var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
            if (visitButton.Displayed)
            {
                visitButton.Click();
            }

            // Wait for 3 seconds 
            Thread.Sleep(2000);

            // Find username and password fields and login button
            var inputUser = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
            var inputPassword = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("login"));

            // Input username and password 
            if (inputUser.Displayed)
            {
                inputUser.SendKeys("nopmontolPN");
            }

            if (inputPassword.Displayed)
            {
                inputPassword.SendKeys("vtm@Promptnow2024");
            }

            if (loginButton.Displayed)
            {
                loginButton.Click();
            }

            // Verify no error alert is present
            try
            {
                var errorAlert = driver.FindElement(By.XPath("//*[@id='error']/div/div"));
                Assert.That(errorAlert.Displayed, Is.False, "User/Password is incorrect.");
            }
            catch (NoSuchElementException)
            {
                // If no error element is found, assume login was successful
            }

            var resultStatus = driver.FindElement(By.ClassName("title"));
            Assert.That(resultStatus.Text, Is.EqualTo("Welcome to RAIJIN-VTM Back Office"), "Unexpected result in the login page.");


            var firstMenuItem = driver.FindElement(By.XPath("//*[@id='myslidemenu']/ul/li[1]/a"));
            if (firstMenuItem.Displayed)
            {
                firstMenuItem.Click();
            }

            Thread.Sleep(1000);

            var submenuItem = driver.FindElement(By.XPath("//*[@id='ui-id-1']"));
            if (submenuItem.Displayed)
            {
                submenuItem.Click();
            }

            var user = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
            
            
            if (user.Displayed)
            {
                user.SendKeys("KittanaiSri46");
            }

            Thread.Sleep(900000);

            var s = driver.FindElement(By.XPath("//*[@id=\"onSearchBtn\"]/span"));
            if(s.Displayed)
            {
              s.Click();
            }
           



















        }
            [TearDown]
            public void Teardown()
            {
                //driver.Quit();
            }
        }
    }
