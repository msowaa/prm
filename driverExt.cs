using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Collections.ObjectModel;

namespace Automation
{
    static class driverExt
    {
        public static void Click(By by, bool ajax=true, bool hiddenElementsClick = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
                driverExt.PreventGoogleapiGlitch();
            }            
            IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            if (hiddenElementsClick == true)
            {
                try
                {
                    elem.Click();
                }
                catch
                {
                    driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
                }
            }
            else
            {
                elem.Click();
            }
        }
        public static void Click(IWebElement rootElement, By by, bool ajax = true, bool hiddenElementsClick = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
                driverExt.PreventGoogleapiGlitch();
            }
            IWebElement elem = rootElement.FindElement(by);
            if (hiddenElementsClick == true)
            {
                try
                {
                    elem.Click();
                }
                catch
                {
                    driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
                }
            }
            else
            {
                elem.Click();
            }
        }
        public static void ClickCss(string cssSelect, bool ajax = true, bool hiddenElementsClick = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
                driverExt.PreventGoogleapiGlitch();
            }
            IWebElement elem = WebdriverBaseClass.driver.FindElement(By.CssSelector(cssSelect));
            if (hiddenElementsClick == true)
            {
                try
                {
                    elem.Click();
                }
                catch
                {
                    driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
                }
            }
            else
            {
                elem.Click();
            }
        }
        public static void ClickXpath(string xpth, bool ajax = true, bool hiddenElementsClick = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
                driverExt.PreventGoogleapiGlitch();
            }
            IWebElement elem = WebdriverBaseClass.driver.FindElement(By.XPath(xpth));
            if (hiddenElementsClick == true)
            {
                try
                {
                    elem.Click();
                }
                catch
                {
                    driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
                }
            }
            else
            {
                elem.Click();
            }
        }
        public static void ClickId(string id, bool ajax = true, bool hiddenElementsClick = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
                driverExt.PreventGoogleapiGlitch();
            }
            IWebElement elem = WebdriverBaseClass.driver.FindElement(By.Id(id));
            if (hiddenElementsClick == true)
            {
                try
                {
                    elem.Click();
                }
                catch
                {
                    driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
                }
            }
            else
            {
                elem.Click();
            }
            
        }
        public static IWebElement FindElement(By by)
        {
            return WebdriverBaseClass.driver.FindElement(by);
        }
        public static void OpenUrl(string url)
        {
            WebdriverBaseClass.driver.Navigate().GoToUrl(url);
        }
        public static string GetUrl()
        {
            return WebdriverBaseClass.driver.Url;
        }
        public static string GetText(By by)
        {
            driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            return WebdriverBaseClass.driver.FindElement(by).Text;
        }
        public static string GetText(By by, By secondBy)
        {
            driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            return WebdriverBaseClass.driver.FindElement(by).FindElement(secondBy).Text;
        }
        public static string GetText(IWebElement baseElement, By by)
        {
            driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            return baseElement.FindElement(by).Text;
        }
        public static bool WaitUntilElementIsPresent(By by, int timeout = 20)
        {
            var wait = new WebDriverWait(WebdriverBaseClass.driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).Displayed);
        }
        public static bool WaitUntilElementIsPresent(By by, By secondBy, int timeout = 20)
        {
            var wait = new WebDriverWait(WebdriverBaseClass.driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).FindElement(secondBy).Displayed);
        }
        public static bool WaitUntilElementIsClickable(By by, int timeout = 20)
        {
            var wait = new WebDriverWait(WebdriverBaseClass.driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).Enabled);
        }
        public static bool WaitUntilElementDisappears(By by, int timeout = 20)
        {
            var wait = new WebDriverWait(WebdriverBaseClass.driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => !d.FindElement(by).Displayed);
        }
        public static string GetLink(By by)
        {
            IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            return elem.GetAttribute("href");
        }
        public static bool ElementDisplayed(By by, bool ajax=true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            }
            //IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            if (WebdriverBaseClass.driver.FindElement(by).Displayed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ElementNotDisplayed(By by, bool ajax = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            }
            try
            {
                driverExt.FindElement(by);
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static bool ElementDisabled(By by, bool ajax = true)
        {
            if (ajax)
            {
                WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            }
            string disabled = driverExt.GetAttributeOf(by, "disabled");
            if (disabled == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void FillTextField(By by, string text, bool enter = false)
        {
            IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            elem.SendKeys(text);
            if (enter == true)
            {
                elem.SendKeys(Keys.Return);
            }
        }
        public static void SendFile(By by, string filePath)
        {
            IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            elem.SendKeys(filePath);
        }
        public static void FillTextFieldWithClear(By by, string text)
        {
            IWebElement elem = WebdriverBaseClass.driver.FindElement(by);
            elem.Clear();
            elem.SendKeys(text);
        }
        public static void WaitForAjax(int? timeoutInSec, int? sleepBeforeAjaxInMs)
        {
            
            try
            {

                //(WebdriverBaseClass.driver as IJavaScriptExecutor).ExecuteScript("src='//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js'");
                Thread.Sleep(200);  //might prevent jquery undefined. If it fails, use the above line but only as a "last resort"
                DateTime startTime = DateTime.Now;

                if (sleepBeforeAjaxInMs != null)


                    if (timeoutInSec == null)
                        timeoutInSec = 20;

                while (((DateTime.Now - startTime).TotalSeconds) < (int)timeoutInSec) // Handle timeout somewhere
                {
                    var ajaxIsComplete = (bool)(WebdriverBaseClass.driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                    {

                        return;
                    }
                    else

                        Thread.Sleep(250);
                }
            }
            catch
            {

            }


        }

        public static void WaitForAjax(int timeoutInSec)
        {
            WaitForAjax(timeoutInSec, null);
        }
        public static IWebElement FindFirstVisibleElement(By selector)
        {
            var items = WebdriverBaseClass.driver.FindElements(selector);

            //Console.WriteLine("Find by " + selector.ToString());
            //Console.WriteLine("Items in list: " + items.Count);
            int count = 0;
            foreach (IWebElement i in items)
            {
                count++;
                if (i.Displayed)
                {
                    //Console.WriteLine("Item displayed: " + count);
                    return i;
                }
            }
            return null;
        }
        public static void ClickFirstVisibleElement(By by, bool ajax = true)
        {
            if (ajax)
            {
                driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            }
            IWebElement elem = driverExt.FindFirstVisibleElement(by);
            try
            {
                elem.Click();
            }
            catch
            {
                driverExt.ExecuteJavaScriptOnElement("arguments[0].click();", elem);
            }
            
        }
        public static ReadOnlyCollection<IWebElement> ListElements(By by)
        {
            var list = WebdriverBaseClass.driver.FindElements(by);
            return list;
        }
        public static ReadOnlyCollection<IWebElement> ListElements(IWebElement rootElement ,By by)
        {
            var list = rootElement.FindElements(by);
            return list;
        }
        public static void WaitForLoading(By by)
        {
            var wait = new WebDriverWait(WebdriverBaseClass.driver, TimeSpan.FromSeconds(10));
            wait.Until(d => (!d.FindElement(by).GetAttribute("class").Contains("loading")));
        }
        public static string GenerateRandomLetter()
        {
            Random letters = new Random();
            int letterNumber = letters.Next(0, 26);
            char firstLetter = (char)('a' + letterNumber);
            return firstLetter.ToString();
        }
        public static void PreventGoogleapiGlitch()
        {
                string js = "window.stop();";
                ((IJavaScriptExecutor)WebdriverBaseClass.driver).ExecuteScript(js);
        }
        public static void SwitchToLatestWindow()
        {
            int handlesCount = WebdriverBaseClass.driver.WindowHandles.Count;
            if (WebdriverBaseClass.IsMobile && handlesCount == 1)
            {                
                int newHandlesCount = handlesCount;
                while (newHandlesCount != handlesCount + 1)
                {
                    Thread.Sleep(100);
                    newHandlesCount = WebdriverBaseClass.driver.WindowHandles.Count;
                    Console.WriteLine("handles count = " + newHandlesCount);
                }
            }
            WebdriverBaseClass.driver.SwitchTo().Window(WebdriverBaseClass.driver.WindowHandles.Last());
        }
        public static void ReturnToFirstWindow()
        {
            WebdriverBaseClass.driver.Close();
            WebdriverBaseClass.driver.SwitchTo().Window(WebdriverBaseClass.driver.WindowHandles.First());
        }
        public static string GetAttributeOf(By by, string attribute)
        {
            return WebdriverBaseClass.driver.FindElement(by).GetAttribute(attribute);
        }
        public static string GetAttributeOf(IWebElement baseElement, By by, string attribute)
        {
            return baseElement.FindElement(by).GetAttribute(attribute);
        }
        public static string GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }
        public static void ExecuteJavaScriptOnElement(string script, IWebElement element)
        {
            ((IJavaScriptExecutor)WebdriverBaseClass.driver).ExecuteScript(script, element);
        }
        public static void ExecuteJavaScript(string script)
        {
            ((IJavaScriptExecutor)WebdriverBaseClass.driver).ExecuteScript(script);
        }
        public static bool CheckForJSErrors()
        {
            string jsErrorFinder = "return errorController.isPageValid();";
            IJavaScriptExecutor js = WebdriverBaseClass.driver as IJavaScriptExecutor;
            bool result = (bool)js.ExecuteScript(jsErrorFinder);
            return result;
        }
        public static bool CheckIfGAWasSent()
        {
            string jsErrorFinder = "return googleAnalytics.getIsPageViewSent();";
            IJavaScriptExecutor js = WebdriverBaseClass.driver as IJavaScriptExecutor;
            bool scriptResult = false;
            bool result = false;

            for (int i = 0; i < 40; i++ )
            {
                scriptResult = (bool)js.ExecuteScript(jsErrorFinder);
                if(scriptResult == true)
                {
                    result = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            return result;
        }

        public static string GetPageTitle()
        {
            return WebdriverBaseClass.driver.Title;
        }

        public static void RefreshPage()
        {
            WebdriverBaseClass.driver.Navigate().Refresh();
        }

        public static void SwitchToIframe(By by)
        {
            WebdriverBaseClass.driver.SwitchTo().Frame(driverExt.FindElement(by));
        }

        public static void SwitchToDefaultContent()
        {
            WebdriverBaseClass.driver.SwitchTo().DefaultContent();
        }
    }
}
