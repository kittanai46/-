﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V128.WebAuthn;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using System.Timers;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_STATUS_SLOT_EDIT_01
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
            var Device_Management = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[6]/a"));
            Device_Management.Click();

            Thread.Sleep(2000);
            var status= driver.FindElement(By.XPath("//*[@id=\"ui-id-1\"]"));
            status.Click();

            Thread.Sleep(2000);
            var Name = driver.FindElement(By.XPath("//*[@id=\"statusslotcode\"]"));
            Name.SendKeys("Test13655");


            Thread.Sleep(2000);
            var S = driver.FindElement(By.XPath("//*[@id=\"SearchBtn\"]"));
            S.Click();

            // เลื่อนหน้าเว็บลงมา 100 พิกเซล
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 100);");

            Thread.Sleep(3000);
            var edit = driver.FindElement(By.XPath("//*[@id=\"frmUser1\"]/div[5]/div/div/fieldset/div/table/tbody/tr/td[6]/a[1]"));
            edit.Click();

            Thread.Sleep(3000);
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 100);");

            var save = driver.FindElement(By.XPath("//*[@id=\"ValidateBtn\"]/span"));
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
