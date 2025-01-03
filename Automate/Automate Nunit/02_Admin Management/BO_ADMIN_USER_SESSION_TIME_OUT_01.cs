using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class B0_ADMIN_USER_PASSWORD_LOCK_01_UserLock
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
        public void TestLoginWithIncorrectThenCorrectCredentials()
        {
            driver.Navigate().GoToUrl("https://36cf-2001-fb1-b2-7d7f-b86f-88a0-c6db-4dc1.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
            if (visitButton.Displayed)
            {
                visitButton.Click();
            }

            
            Thread.Sleep(3000);

            for (int i = 0; i < 6; i++)
            {
                
                driver.Navigate().Refresh();
                Thread.Sleep(2000);

                
                var inputUser = driver.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input"));
                var inputPassword = driver.FindElement(By.Id("password"));
                var loginButton = driver.FindElement(By.Id("login"));

                
                inputUser.Clear();
                inputPassword.Clear();

                if (i < 6)
                {
                    
                    inputUser.SendKeys("nopmontolPN");
                    inputPassword.SendKeys("incorrectPassword");
                }
                else
                {
                    
                    inputUser.SendKeys("nopmontolPN");
                    inputPassword.SendKeys("vtm@Promptnow2024");
                }

                loginButton.Click();

                
                

                
                }
            }
        }

   
}



