using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_ADMIN_USER_SEARCH_01
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
            driver.Navigate().GoToUrl("https://16cc-2001-fb1-b3-6e71-4481-42fc-cae8-18de.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
            if (visitButton.Displayed)
            {
                visitButton.Click();
            }

            // Wait for 3 seconds 
            Thread.Sleep(3000);

            // Find username and password fields and login button
            var inputUser = driver.FindElement(By.Id("username"));
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

            //search
            var input = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
            var search = driver.FindElement(By.XPath("//*[@id=\"onSearchBtn\"]/span"));
            if (input.Displayed)
            {
                input.SendKeys("KittanaiSri");
                search.Click();
            }


          

           
        }
            [TearDown]
            public void Teardown()
            {
                //driver.Quit();
            }
        }
    }
