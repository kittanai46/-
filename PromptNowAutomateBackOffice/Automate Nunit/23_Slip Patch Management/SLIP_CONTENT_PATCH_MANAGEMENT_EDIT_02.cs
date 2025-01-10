using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebBrowser
{
    [TestFixture]
    public class SLIP_CONTENT_PATCH_MANAGEMENT_EDIT_02
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
            try
            {
                // Navigate to the website
                driver.Navigate().GoToUrl("https://f9f9-2001-fb1-b0-2184-b1e7-2215-6134-e8ac.ngrok-free.app/pn_next_automation_bo/page/verification/login.jsp");

                // WebDriverWait สำหรับจัดการการรอ
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // คลิกปุ่ม Visit ถ้าปรากฏ
                try
                {
                    var visitButton = wait.Until(d => d.FindElement(By.XPath("//*[@id='root']/div/main/div/div/section[1]/div/footer/button")));
                    if (visitButton.Displayed)
                    {
                        visitButton.Click();
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Visit button not found, skipping...");
                }

                // รอและกรอกข้อมูลการเข้าสู่ระบบ
                var inputUser = wait.Until(d => d.FindElement(By.XPath("/html/body/form/div[4]/div/div[2]/table/tbody/tr[2]/td[2]/input")));
                var inputPassword = driver.FindElement(By.Id("password"));
                var loginButton = driver.FindElement(By.Id("login"));

                inputUser.Clear();
                inputPassword.Clear();

                inputUser.SendKeys("nopmontolPN");
                inputPassword.SendKeys("vtm@Promptnow2024");

                loginButton.Click();

                // รอให้หน้าโหลดเสร็จ
                Thread.Sleep(3000); // หรือใช้ WebDriverWait รอจน URL เปลี่ยน

                // Navigate ไปยัง Path ที่ต้องการ
                var Path = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"myslidemenu\"]/ul/li[12]/a")));
                Path.Click();

                Thread.Sleep(2000);
                var slip = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"ui-id-2\"]")));
                slip.Click();

                Thread.Sleep(3000);

                var edit = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"frmUser1\"]/div[5]/div/div/fieldset/div/table/tbody/tr/td[8]/a[1]")));
                edit.Click();

                var clear = driver.FindElement(By.XPath("//*[@id=\"name\"]"));
                clear.Clear();

                // เลื่อนหน้าจอลงทีละ 100 พิกเซล
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollBy(0, 100);");

                Thread.Sleep(1000); // รอเล็กน้อย

                // คลิกปุ่ม Save
                var save = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"saveEditBtn\"]")));
                save.Click();

                Console.WriteLine("Test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [TearDown]
        public void Teardown()
        {
            //// ปิดเบราว์เซอร์เมื่อเสร็จ
            //driver.Quit();
        }
    }
}
