using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_LOGIN_01
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
            driver.Navigate().GoToUrl("https://16cc-2001-fb1-b3-6e71-4481-42fc-cae8-18de.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
            if (visitButton.Displayed)
            {
                visitButton.Click();
            }

            // Wait for 3 seconds
            Thread.Sleep(3000);

            for (int i = 0; i < 5; i++)
            {
                // Refresh the page to reset login fields
                driver.Navigate().Refresh();
                Thread.Sleep(2000);

                // Locate username and password fields and login button again
                var inputUser = driver.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input"));
                var inputPassword = driver.FindElement(By.Id("password"));
                var loginButton = driver.FindElement(By.Id("login"));

                // Input incorrect credentials
                inputUser.Clear();
                inputPassword.Clear();

                inputUser.SendKeys($"incorrectUsername#{i+1}");
                inputPassword.SendKeys("incorrectPassword");

                loginButton.Click();

                // Wait for potential error message
                Thread.Sleep(2000);

                try
                {
                    // Check for an error alert message after each login attempt
                    var errorAlert = driver.FindElement(By.XPath("//*[@id='error']/div/div"));
                    Assert.That(errorAlert.Displayed, Is.True, $"Attempt {i + 1}: Error message should be displayed for incorrect login.");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"Attempt {i + 1}: Error message not found. Expected an error for incorrect credentials.");
                }
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
