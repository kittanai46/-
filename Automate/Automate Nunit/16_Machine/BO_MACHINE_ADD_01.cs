using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V128.WebAuthn;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_MACHINE_ADD_01
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
            driver.Navigate().GoToUrl("https://4de8-2001-fb1-b2-96f1-b0a8-b53f-1139-3d8c.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

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
            var Machine_Management = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[7]/a"));
            Machine_Management.Click();

            var Machine = driver.FindElement(By.XPath("//*[@id=\"ui-id-2\"]"));
            Machine.Click();

            Thread.Sleep(2000);
            var add = driver.FindElement(By.XPath("//*[@id=\"doAddBtn\"]"));
            add.Click();

            var code = driver.FindElement(By.XPath("//*[@id=\"machinemodelcode\"]"));
            code.Click();
            var code123 = driver.FindElement(By.XPath("//*[@id=\"machinemodelcode\"]/option[6]")); ;
            code123.Click();

            Thread.Sleep(3000);
            var name = driver.FindElement(By.XPath("//*[@id=\"name\"]"));
            name.Clear();
            name.SendKeys("Muhahaha");

            Thread.Sleep(3000);
            var description = driver.FindElement(By.XPath("//*[@id=\"description\"]"));
            description.SendKeys("Hello");

            Thread.Sleep(2000);
            var serial_number = driver.FindElement(By.XPath("//*[@id=\"serialnumber\"]"));
            serial_number.SendKeys("123456789");

            Thread.Sleep(1000);
            var remark = driver.FindElement(By.XPath("//*[@id=\"remark\"]"));
            remark.SendKeys("12123");

            // เลื่อนหน้าเว็บลงมา 100 พิกเซล
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 100);");

            // รอเล็กน้อยหากต้องการให้การเลื่อนหน้าจอเสร็จสมบูรณ์ (optional)
            Thread.Sleep(500); // รอ 0.5 วินาที (ปรับตามต้องการ)
            var save = driver.FindElement(By.XPath("//*[@id=\"ValidateBtn\"]"));
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
