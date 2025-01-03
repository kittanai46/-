using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V128.WebAuthn;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using System.Timers;
using static System.Collections.Specialized.BitVector32;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class BO_TERMINAL_MONITORING_EDIT_01
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
            var TM = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[5]"));
            TM.Click();

            Thread.Sleep(2000);
            var Terminal= driver.FindElement(By.XPath("//*[@id=\"ui-id-2\"]"));
            Terminal.Click();

            Thread.Sleep(5000);
            var s = driver.FindElement(By.XPath("//*[@id=\"onSearchBtn\"]"));
            s.Click();

            // หา element ที่ต้องการเลื่อนหน้าเว็บไปหา
            var element = driver.FindElement(By.XPath("//*[@id='frmUser1']/div[5]/div/div/fieldset/div[1]"));

            // เลื่อนหน้าเว็บลงมาที่ตำแหน่งของ element
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

            // รอเล็กน้อยเพื่อให้การเลื่อนหน้าจอแนวตั้งเสร็จ (สามารถลบออกได้หากไม่จำเป็น)
            Thread.Sleep(2000);

            // เลื่อนภายใน element ไปทางขวาสุด
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollLeft = arguments[0].scrollWidth", element);

            Thread.Sleep(1000);

            // เลื่อนหน้าเว็บขึ้นไปที่ตำแหน่งด้านบนของ element (เลื่อนไปที่ขอบบน)
            // เลื่อนหน้าเว็บขึ้นไป 100 พิกเซล
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, -100);");

            Thread.Sleep(1000);
            var edit = driver.FindElement(By.XPath("//*[@id=\"frmUser1\"]/div[5]/div/div/fieldset/div[1]/table/tbody/tr[1]/td[37]/a"));
            edit.Click();

            Thread.Sleep(3000);
            // เลื่อนหน้าเว็บลงมา 100 พิกเซล
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 100);");

            Thread.Sleep(2000);
            var click = driver.FindElement(By.XPath("//*[@id=\"statusactioncode\"]"));
            click.Click();
            Thread.Sleep(2000);
            var TEST = driver.FindElement(By.XPath("//*[@id=\"statusactioncode\"]/option[9]"));
            TEST.Click();

            var r1 = driver.FindElement(By.XPath("/html/body/div[2]/fieldset/div/table/tbody/tr[1]/td[7]/div"));
            r1.Click();
            Thread.Sleep(2000);
            var r2 = driver.FindElement(By.XPath("//*[@id=\"statusval\"]/option[1]"));
            r2.Click();


            // รอให้ alert ปรากฏขึ้น
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());

            // สวิตช์ไปที่ alert
            IAlert alert = driver.SwitchTo().Alert();

            // อ่านข้อความของ alert (ถ้าต้องการ)
            string alertText = alert.Text;
            Console.WriteLine("Alert text: " + alertText);

            // กดปุ่ม OK เพื่อยืนยัน (หรือ Accept)
            alert.Accept(); // ใช้ Accept() เพื่อกด OK หรือ Dismiss() เพื่อกด Cancel

        }

        [TearDown]
        public void Teardown()
        {
            // Uncomment the line below to close the browser after the test
            // driver.Quit();
        }
    }
}
