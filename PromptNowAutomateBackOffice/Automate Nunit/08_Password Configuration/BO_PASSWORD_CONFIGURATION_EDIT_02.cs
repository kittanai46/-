﻿using NUnit.Framework;
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
    public class BO_PASSWORD_CONFIGURATION_EDIT_02
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

            Thread.Sleep(3000);

            var System_Configuration = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[3]/a"));
            System_Configuration.Click();


            var Password_Configuration = driver.FindElement(By.XPath("//*[@id=\"menu\"]/li[3]"));
            Password_Configuration.Click();

            Thread.Sleep(2000);
            var Edit = driver.FindElement(By.XPath("//*[@id=\"newValueID1\"]"));
            if (Edit.Displayed)
            {
                Thread.Sleep(2000);
                Edit.Clear();
                Thread.Sleep(2000);
                Edit.SendKeys("10");
            }
            Thread.Sleep(2000);
            var Save = driver.FindElement(By.XPath("//*[@id=\"data_grid\"]/fieldset/div/table/tbody/tr[1]/td[6]"));
            Save.Click();

            var ok = driver.FindElement(By.XPath("/html/body/div[4]/div[3]/div/button[2]/span"));
            ok.Click();
        }

        [TearDown]
        public void Teardown()
        {
            // Uncomment the line below to close the browser after the test
            // driver.Quit();
        }
    }
}
