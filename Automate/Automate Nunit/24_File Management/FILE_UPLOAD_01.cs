using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class FILE_UPLOAD_01
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestUploadFile()
        {
            // Navigate to the website
            driver.Navigate().GoToUrl("https://f9f9-2001-fb1-b0-2184-b1e7-2215-6134-e8ac.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

            // WebDriverWait สำหรับจัดการการรอ
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // คลิกปุ่ม Visit
            var visitButton = wait.Until(d => d.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button")));
            visitButton.Click();

            // รอและกรอกข้อมูลการเข้าสู่ระบบ
            var inputUser = wait.Until(d => d.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input")));
            var inputPassword = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("login"));

            inputUser.Clear();
            inputPassword.Clear();

            inputUser.SendKeys("nopmontolPN");
            inputPassword.SendKeys("vtm@Promptnow2024");

            loginButton.Click();

            // คลิกเมนู File
            var file = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[13]/a")));
            file.Click();

            // รอและคลิกปุ่ม Add File
            var upload = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"addFile\"]")));
            upload.Click();

            // ใส่ข้อมูลในช่อง lifetime
            var lifetime = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"lifetime\"]")));
            lifetime.Clear();
            lifetime.SendKeys("5");

            // ใส่ข้อมูลในช่อง description
            var description = driver.FindElement(By.XPath("//*[@id=\"description\"]"));
            description.SendKeys("Hello this is a description test101");

            // เลื่อนหน้าจอลง
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0, 300);");

            // อัปโหลดไฟล์
            var fileInput = wait.Until(d => d.FindElement(By.XPath("//input[@type='file']")));
            string filePath = @"C:\Users\bawkt\Downloads\cat.png";
            fileInput.SendKeys(filePath);

            // คลิกปุ่ม Save
            var save = driver.FindElement(By.XPath("//*[@id=\"uploadBtn\"]"));
            save.Click();

            Console.WriteLine("File upload test completed successfully!");
        }

        [TearDown]
        public void Teardown()
        {
            // ปิดเบราว์เซอร์เมื่อเสร็จ
            //driver.Quit();
        }
    }
}
