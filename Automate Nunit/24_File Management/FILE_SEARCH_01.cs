using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class FILE_SEARCH_01
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
            // Navigate to the website
            driver.Navigate().GoToUrl("https://f9f9-2001-fb1-b0-2184-b1e7-2215-6134-e8ac.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            // WebDriverWait สำหรับจัดการการรอ
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

           var visitButton = driver.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button"));
           visitButton.Click();
                

            // รอและกรอกข้อมูลการเข้าสู่ระบบ
            var inputUser = driver.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input"));
            var inputPassword = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("login"));

            inputUser.Clear();
            inputPassword.Clear();

            inputUser.SendKeys("nopmontolPN");
            inputPassword.SendKeys("vtm@Promptnow2024");

            loginButton.Click();

            var file = driver.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[13]/a"));
            file.Click();

            Thread.Sleep(2000);
            var search = driver.FindElement(By.XPath("//*[@id=\"onSearch\"]"));
            search.Click();




        }

        [TearDown]
        public void Teardown()
        {
            // ปิดเบราว์เซอร์เมื่อเสร็จ
            //driver.Quit();
        }
    }
}
