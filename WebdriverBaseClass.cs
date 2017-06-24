using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Reflection;
using System.Configuration;

namespace Automation
{
    class WebdriverBaseClass
    {
        public static IWebDriver driver;

        public static string AppUrl = System.Configuration.ConfigurationManager.AppSettings["AppUrl"];
        public static string url;
        public static int globalTimeoutInSec = 20;
        public static int ajaxTimeout = 5;

        public static Uri testServerAddress = new Uri("http://127.0.01:4723/wd/hub"); // If Appium is running locally
        public static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);
        private bool PageProtectionLogInPageOn = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PageProtectionLogInPageOn"]);
        public static bool IsMobile = false;


        public void SetUpTest()
        {
            if (driver == null)
            {                
                IsMobile = false;
                driver = Helpers.SelectBrowserDriver(driver, "ff");
                driver.Manage().Window.Maximize();//todo multiple browser sizes
                driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, globalTimeoutInSec));
                driver.Manage().Cookies.DeleteAllCookies();
                driver.Navigate().GoToUrl(AppUrl);
                if (PageProtectionLogInPageOn)
                {
                    Helpers.PageProtectionLogIn(driver);
                }
                    driver.Navigate().GoToUrl(AppUrl + "it/homepage");
                }               

            }

        }

        public void SetUpTestMobile()
        {
            if (driver == null)
            {
                IsMobile = true;
                DesiredCapabilities testCapabilities = new DesiredCapabilities();
                testCapabilities.SetCapability("browserName", "Chrome");
                testCapabilities.SetCapability("platformName", "Android");
                testCapabilities.SetCapability("deviceName", "S(Galaxy S5)");
                testCapabilities.SetCapability("noReset", true);
                testCapabilities.SetCapability("fullReset", false);
                testCapabilities.SetCapability("autoAcceptAlerts", true);
                testCapabilities.SetCapability("locationServicesAuthorized", true);
                //testCapabilities.SetCapability("autoLaunch", true);
                //testCapabilities.SetCapability("androidUseRunningApp", "true");              
                driver = new RemoteWebDriver(testServerAddress, testCapabilities, INIT_TIMEOUT_SEC);
                driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, globalTimeoutInSec));
               // string[] temp;
                if (AppUrl.Contains("www."))
                {
                    url = AppUrl.Replace("://www.", "://m.");
                }
                else
                {
                    url = AppUrl.Replace("://", "//m.");
                    //temp = AppUrl.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    //url = "http://m." + temp[1] + "/";
                }
                driver.Navigate().GoToUrl(url);
                //driver.Navigate().GoToUrl(url + "en-us/homepage");
                if (PageProtectionLogInPageOn)
                {
                    Helpers.PageProtectionLogIn(driver);
                }

            }

        }




        public void TeardownTest()
        {
            if (driver != null)
            {
                try
                {
                    driver.Close();
                    driver.Quit();
                    driver = null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

    }
}
