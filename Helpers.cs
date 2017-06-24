using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using AE.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//using JSErrorCollector;




namespace Automation
{
    public class Helpers
    {
        private static string AppUrl = WebdriverBaseClass.AppUrl;
        public static IWebDriver SelectBrowserDriver(IWebDriver driver, string input)
        {
            string browser = input.ToLower();
            if (browser == "chrome")
            {
                driver = new ChromeDriver();
            }
            if (browser == "ie" || browser == "internetexplorer")
            {
                driver = new InternetExplorerDriver();
            }
            if (driver == null || browser == "fireFox" || browser == "ff" || browser == "")
            {
                FirefoxProfile profile = new FirefoxProfile{ EnableNativeEvents = false };
                //JavaScriptError.AddExtension(profile, "C:\\jsaddon\\");
                profile.SetPreference("geo.prompt.testing", true);
                profile.SetPreference("geo.prompt.testing.allow", true);
                profile.SetPreference("network.http.connection-timeout", 10);
                profile.SetPreference("network.http.connection-retry-timeout", 10);
                driver = new FirefoxDriver(profile);
                
                
            }
            return driver;
        }

        public static void PageProtectionLogIn(IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.Id("LoginStealth")).SendKeys("primark");
                driver.FindElement(By.Id("PasswordStealth")).SendKeys("0nesie5");
                driver.FindElement(By.CssSelector(".btn.btn-large.btn-primary")).Click();
            }
            catch
            {
            }
        }

        public static void WriteToCSV(string testName, DateTime startTime)
        {
            DateTime time = DateTime.Now;
            string filePath = "C:\\WebdriverResultTiming\\";
            string fileName = "Results.csv";
            var csv = new StringBuilder();
            String newLine;
            if (!Directory.Exists(filePath))
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(filePath);
                    Console.WriteLine("The directory was created successfully at {0}.",
                        Directory.GetCreationTime(filePath));
                }
                catch
                {
                    Console.WriteLine("Couldnt create the folder");
                }
            }
            if (!File.Exists(filePath + fileName))
            {
                newLine = String.Format("{0};{1};{2};{3};{4};{5}{6}", "TestName", "Date", "Started", "Finished", "TotalTime", "TotalTime(ms)", Environment.NewLine);
                csv.Append(newLine);
            }
            string name = testName;
            TimeSpan span = time.Subtract(startTime);
            string startingTime = String.Format("{0:HH:mm:ss.fff}", startTime);
            string startingDate = String.Format("{0:d.M.yyyy}", startTime);
            string spanTime = span.Hours.ToString("D2") + ":" + span.Minutes.ToString("D2") + ":" + span.Seconds.ToString("D2") + "." + span.Milliseconds.ToString("D3");
            string spanTimeMS = ((int)span.TotalMilliseconds).ToString();
            string endTime = String.Format("{0:HH:mm:ss.fff}", time);
            newLine = String.Format("{0};{1};{2};{3};{4};{5}{6}", name, startingDate, startingTime, endTime, spanTime, spanTimeMS, Environment.NewLine);
            csv.Append(newLine);
            Console.WriteLine(csv);
            try
            {
                File.AppendAllText((filePath + fileName), csv.ToString());
            }
            catch
            {
                Console.WriteLine("Couldnt save the file");
            }
        }

        public static void GoToProducts(IWebDriver driver)
        {
            driverExt.ClickId("header-products-link");
        }
        public static void GoToPrimania(IWebDriver driver)
        {
            driverExt.ClickId("header-primania-link");
        }
        public static void GoToOurStores(IWebDriver driver)
        {
            driverExt.ClickId("header-ourStores-link");
        }
        public static void GoToOurEthics(IWebDriver driver)
        {
            driverExt.ClickId("header-ourEthic-link");
        }


        public static string GetSalt()
        {
            return DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }

        public static string GetSaltWithout_Char()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public static IWebElement FindFirstVisibleElement(IWebDriver driver, By selector)
        {
            var items = driver.FindElements(selector);

            Console.WriteLine("Find by " + selector.ToString());
            Console.WriteLine("Items in list: " + items.Count);
            int count = 0;
            foreach (IWebElement i in items)
            {
                count++;
                if (i.Displayed)
                {
                    Console.WriteLine("Item displayed: " + count);
                    return i;
                }
            }
            return null;
        }

        public static bool WaitUntilElementIsPresent(IWebDriver driver, By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).Displayed);
        }

        public static bool WaitUntilElementIsPresent(IWebDriver driver, By by, By secondBy, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).FindElement(secondBy).Displayed);
        }

        public static bool WaitUntilElementIsClickable(IWebDriver driver, By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.FindElement(by).Enabled);
        }


        public static void SignIn(IWebDriver driver, string email, string password, bool usingFacebook = false)
        {
            Context.Header.SignIn();

            if (WebdriverBaseClass.IsMobile)
            {
                try
                {
                    Helpers.CloseCookiePolicy();
                }
                catch
                {
                }
                //Context.SignInSignUpPage.ChooseSignIn();
            }

            if (usingFacebook)
            {
                if (WebdriverBaseClass.IsMobile)
                {
                    driverExt.WaitUntilElementIsPresent(By.CssSelector(".cell>h2"));
                    driverExt.ClickFirstVisibleElement(By.Id("fb-login"));
                    driverExt.FillTextField(By.Name("email"), email);
                    driverExt.FillTextField(By.Name("pass"), password);
                    driverExt.Click(By.Name("login"), false);
                }
                else
                {
                    driverExt.WaitUntilElementIsPresent(By.CssSelector(".card.left"), By.CssSelector(".loginArea.transition-05s"), 5);
                    driverExt.ClickFirstVisibleElement(By.Id("fb-login"));
                    driverExt.FillTextField(By.Id("email"), email);
                    driverExt.FillTextField(By.Id("pass"), password);
                    driverExt.Click(By.Name("login"), false);
                }
            }
            else
            {
                Context.SignInSignUpPage.FillEmailSignInFieldWith(email);
                Context.SignInSignUpPage.FillPasswordSignInFieldWith(password);
                Context.SignInSignUpPage.ClickSignIn();
            }
            //driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            if (WebdriverBaseClass.IsMobile)
            {
                driverExt.WaitUntilElementIsPresent(By.Id("m-user-data"));
            }
            else
            {
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".my-primark.logged-in"));
            }
        }

        public static void SignOut(IWebDriver driver)
        {
            Context.Header.SignOut();
        }

        public static void ViewProfile(IWebDriver driver)
        {
            Context.Header.ViewProfile();
        }


        public static void EditProfile(IWebDriver driver)
        {
            Context.Header.EditProfile();
        }

        public static string AddLookToPrimania(IWebDriver driver, string path, string itemName, string itemPrice, string description)
        {
            Context.Header.OpenPrimania();
            Context.PrimaniaPage.ClickLookUpload();
            Context.PrimaniaPage.UploadPhotoToLook(path);
            Context.PrimaniaPage.ClickNextAfterUpload();
            Context.PrimaniaPage.TagItem(itemName, itemPrice);
            Context.PrimaniaPage.ClickNextAfterTagging();
            Context.PrimaniaPage.FinalizeLookUpload(description);
            driverExt.WaitUntilElementIsPresent(By.CssSelector(".primania.item"));
            string lookId = driverExt.GetUrl().Split('=').Last();

            return lookId;            
        }
        public static string AddLookToPrimaniaViaMyProfile(string path, string itemName, string itemPrice, string description)
        {
            Context.Header.ViewProfile();
            Context.ProfilePage.ViewLooksTab();
            Context.ProfilePage.UploadNewLook();
            Context.PrimaniaPage.UploadPhotoToLook(path);
            Context.PrimaniaPage.ClickNextAfterUpload();
            Context.PrimaniaPage.TagItem(itemName, itemPrice);
            Context.PrimaniaPage.ClickNextAfterTagging();
            Context.PrimaniaPage.FinalizeLookUpload(description);
            driverExt.WaitUntilElementIsPresent(By.CssSelector(".primania.item"));
            string lookId = driverExt.GetUrl().Split('=').Last();

            return lookId;
        }

        public static void LogInToSitecoreAsAdmin(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(AppUrl + "sitecore/");
            driver.FindElement(By.Id("Login_UserName")).SendKeys(Users.SitecoreAdmin.Email);
            driver.FindElement(By.Id("Login_Password")).SendKeys(Users.SitecoreAdmin.Password);
            driver.FindElement(By.Id("Login_Login")).Click();
        }

        public static string CreateUser(IWebDriver driver, string email = "", string password = "")
        {
            string salt = Helpers.GetSaltWithout_Char();

            if (email == "")
            {
                email = "primarktest+" + salt + "@gmail.com";
            }
            if (password == "")
            {
                password = Users.NewUser.Password;
            }
            string name = Helpers.GetSalt();
            //driver.Navigate().GoToUrl(AppUrl + "en/newaccount/");//change to clicking sign in/up link
            driver.FindElement(By.Id("register-link")).Click();
            Helpers.FindFirstVisibleElement(driver, By.CssSelector(".button-activeRegistrationArea")).Click();

            Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".card.right"), By.CssSelector(".registrationArea.transition-05s"), 5);//todo: implement mobile version

            IWebElement firstName = Helpers.FindFirstVisibleElement(driver, By.CssSelector(".textbox-signUp-firstName"));
            IWebElement emailField = Helpers.FindFirstVisibleElement(driver, By.CssSelector(".textbox-signUp-email"));
            IWebElement emailConfirm = Helpers.FindFirstVisibleElement(driver, By.CssSelector(".textbox-signUp-confirmEmail"));
            IWebElement passwordField = Helpers.FindFirstVisibleElement(driver, By.CssSelector(".textbox-signUp-password"));
            IWebElement submitElement = Helpers.FindFirstVisibleElement(driver, By.CssSelector(".button-signUp-submit"));

            firstName.SendKeys(name);
            //Assert.IsFalse(submitElement.Enabled);
            emailField.SendKeys(email);
            //Assert.IsFalse(submitElement.Enabled);
            emailConfirm.SendKeys(email);
            //Assert.IsFalse(submitElement.Enabled);
            passwordField.SendKeys(password);
            //Assert.IsTrue(submitElement.Enabled);
            submitElement.Click();

            Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".card.right"), By.CssSelector(".welcomeMessageArea.transition-05s"), 5);//todo: implement mobile version

            //StringAssert.Contains(name, Helpers.FindFirstVisibleElement(driver, By.CssSelector(".cell>h1>span")).Text);
            //StringAssert.Contains(email, Helpers.FindFirstVisibleElement(driver, By.CssSelector(".m-b-big>span")).Text);
            Helpers.LogInToSitecoreAsAdmin(driver);

            driver.FindElement(By.Id("Ribbon_Nav_SecurityStrip")).Click();
            driver.FindElement(By.LinkText("User Manager")).Click();
            driver.SwitchTo().Frame(driver.FindElement(By.Id("jqueryModalDialogsFrame")));
            driver.SwitchTo().Frame(driver.FindElement(By.Id("scContentIframeId0")));
            driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            driver.FindElement(By.Id("Users_searchBox")).SendKeys(salt + OpenQA.Selenium.Keys.Return);
            int i = 0;
            while (driverExt.GetText(By.XPath(".//*[@id='Users_cell_0_7']/div")) != email)
            {
                Thread.Sleep(250);
                i++;
                if (i > 20) break;
            }
            driver.FindElement(By.XPath(".//*[@id='Users_cell_0_7']/div")).Click();
            driver.FindElement(By.LinkText("Unlock")).Click();
            driver.SwitchTo().Alert().Accept();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driverExt.WaitForAjax(WebdriverBaseClass.ajaxTimeout);
            wait.Until(d => d.FindElement(By.XPath(".//*[@id='Users_cell_0_10']/div")).Text != "Disabled, Locked Out");
            driver.SwitchTo().DefaultContent();
            //end of workaround

            //Helpers.SignIn(driver, email, password);
            //Console.WriteLine(Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".menu-primark"), 5));
            //IWebElement welcomeMsg = driver.FindElement(By.CssSelector(".menu-primark"));
            //StringAssert.Contains("hello", driver.FindElement(By.CssSelector(".menu-primark")).Text.ToLower());

            driver.Navigate().GoToUrl(AppUrl);

            return email;
        }

        public static void ClearFavourites(IWebDriver driver)
        {
            int numberOfFavorites = Context.Header.FavouritesCount();

            if (numberOfFavorites != 0)
            {
                Context.Header.ViewProfile();
                Context.ProfilePage.ViewFavouritesTab();

                var listOfFavourites = driverExt.ListElements(By.CssSelector(".button.button-favourite.favourited"));
                foreach (var favourite in listOfFavourites)
                {
                    Context.ProfilePage.RemoveFirstFavouriteClickButton();
                    Context.ProfilePage.RemoveFirstFavouriteConfirm();
                }
            }
        }

        public static void ClearFollowing()
        {
            Context.ProfilePage.ViewFollowersFollowingTab();
            int followingCount = Context.ProfilePage.GetFollowingCount();

            while (followingCount != 0)
            {
                Context.ProfilePage.ViewFollowing();
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".profile-unfollow-button"), 10);
                driverExt.WaitUntilElementIsClickable(By.CssSelector(".profile-unfollow-button"), 10);

                var listOfFollowing = driverExt.ListElements(By.CssSelector(".profile-unfollow-button"));

                foreach (var following in listOfFollowing)
                {
                    driverExt.WaitUntilElementIsPresent(By.CssSelector(".profile-unfollow-button"), 10);
                    driverExt.WaitUntilElementIsClickable(By.CssSelector(".profile-unfollow-button"), 10);
                    try
                    {
                        Context.ProfilePage.ClickUnfollow();
                    }
                    catch
                    {
                    }
                    driverExt.WaitForAjax(3);
                    followingCount = Context.ProfilePage.GetFollowingCount();
                }
            }
        }

        public static void ViewMyProfileFollowersFollowing(IWebDriver driver)
        {
            driver.FindElement(By.XPath(".//*[@id='body']/div/div/div[2]/ul/li[3]/a[2]/strong")).Click();       //Followers/Following tab, waiting for id's
        }

        public static void ShareOnFB(IWebDriver driver, string fbLogin, string fbPassword, string description)
        {
            string url = driverExt.GetUrl();
            if (url.Contains("product"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("facebook-sharing-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("facebook-sharing-link"), 10);

                driver.FindElement(By.Id("facebook-sharing-link")).Click();
            }
            else if (url.Contains("our-ethics"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("fb-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("fb-link"), 10);

                driver.FindElement(By.Id("fb-link")).Click();
            }
            else
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".icon-facebook"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".icon-facebook"), 10);

                driver.FindElement(By.CssSelector(".icon-facebook")).Click();
            }

            if (WebdriverBaseClass.IsMobile)
            {
                driverExt.SwitchToLatestWindow();
                driverExt.FillTextField(By.Name("email"), fbLogin);
                driverExt.FillTextField(By.Name("pass"), fbPassword);
                driverExt.Click(By.Name("login"), false);
                driverExt.FillTextField(By.Id("share_msg_input"), description);
                driverExt.ClickId("share_submit", false);
            }
            else
            {
                driverExt.SwitchToLatestWindow();
                driver.FindElement(By.Id("email")).SendKeys(fbLogin);
                driver.FindElement(By.Id("pass")).SendKeys(fbPassword);
                driver.FindElement(By.Id("u_0_2")).Click();
                driver.FindElement(By.Id("u_0_d")).SendKeys(description);
                driver.FindElement(By.CssSelector("._42ft._4jy0._4jy3._4jy1.selected._51sy")).Click();
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.First());
            }

        }
        public static void ShareOnTwitter(IWebDriver driver, string twitterLogin, string twitterPassword, string description)
        {
            string url = driverExt.GetUrl();
            if (url.Contains("product") || url.Contains("features"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("twitter-sharing-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("twitter-sharing-link"), 10);

                driver.FindElement(By.Id("twitter-sharing-link")).Click();
            }
            else if (url.Contains("our-ethics"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("twitter-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("twitter-link"), 10);

                driver.FindElement(By.Id("twitter-link")).Click();
            }
            else
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector("#twitter-sharing-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector("#twitter-sharing-link"), 10);

                driver.FindElement(By.CssSelector("#twitter-sharing-link")).Click();
            }
            driverExt.SwitchToLatestWindow();
            driver.FindElement(By.Id("username_or_email")).SendKeys(twitterLogin);
            driver.FindElement(By.Id("password")).SendKeys(twitterPassword);
            driver.FindElement(By.Id("status")).SendKeys("+" + description);
            driver.FindElement(By.CssSelector(".button.selected.submit")).Click();
            //driver.FindElement(By.CssSelector(".button.selected.submit")).Click();
            if (!WebdriverBaseClass.IsMobile)
            {
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.First());
            }
        }
        public static void ShareOnPinterest(IWebDriver driver, string pinterestLogin, string pinterestPassword)
        {
            string url = driverExt.GetUrl();
            if (url.Contains("product"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("pinterest-sharing-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("pinterest-sharing-link"), 10);

                driver.FindElement(By.Id("pinterest-sharing-link")).Click();
            }
            else if (url.Contains("our-ethics"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("pinterest-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("pinterest-link"), 10);

                driver.FindElement(By.Id("pinterest-link")).Click();
            }
            else
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".icon-pinterest"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".icon-pinterest"), 10);

                driver.FindElement(By.CssSelector(".icon-pinterest")).Click();
            }

            driverExt.SwitchToLatestWindow();
            if (WebdriverBaseClass.IsMobile)
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector("body > div.Module.Modal.fullScreen.absoluteCenter.show > div.modalScroller > div > div > div > div > div.overlay > div.buttonContainer > div.buttonInnerContainer> div"));
                driverExt.ClickCss("body > div.Module.Modal.fullScreen.absoluteCenter.show > div.modalScroller > div > div > div > div > div.overlay > div.buttonContainer > div.buttonInnerContainer> div");
                driverExt.ClickCss("body > div.App.AppBase.Module > div.appContent > div.mainContainer > div > div.signupContentWrapper > button");
                driverExt.FillTextField(By.CssSelector("body > div.App.AppBase.Module > div.appContent > div.mainContainer > div > div > div > form > ul > li.loginUsername > input"), pinterestLogin);
                driverExt.FillTextField(By.CssSelector("body > div.App.AppBase.Module > div.appContent > div.mainContainer > div > div > div > form > ul > li.loginPassword > input[type='password']"), pinterestPassword);
                driverExt.ClickCss("body > div.App.AppBase.Module > div.appContent > div.mainContainer > div > div > div > form > div.formFooter > div > button");
                driverExt.ClickCss("body > div > div.appContent > div.mainContainer > div > div > form > div.boardPickerSection > div > div > div > ul:nth-child(1) > ul > li > div");

            }
            else
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".loginLine>a"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".loginLine>a"), 10);
                driver.FindElement(By.CssSelector(".loginLine>a")).Click();
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".email"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".email"), 10);
                driver.FindElement(By.CssSelector(".email")).SendKeys(pinterestLogin);
                driver.FindElement(By.CssSelector(".loginPassword>input")).SendKeys(pinterestPassword);
                driver.FindElement(By.CssSelector(".Module.Button.btn.rounded.large.primary.hasText")).Click();
                //new layout
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".BoardLabel.Module.pinCreate3"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".BoardLabel.Module.pinCreate3"), 10);
                driver.FindElement(By.CssSelector(".BoardLabel.Module.pinCreate3")).Click();
                //old layout (pinterest keeps changing between the two)
                /*Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".rounded.Button.repinSmall.pinIt.primary.Module.btn"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".rounded.Button.repinSmall.pinIt.primary.Module.btn"), 10);
                driver.FindElement(By.CssSelector(".rounded.Button.repinSmall.pinIt.primary.Module.btn")).Click();*/
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".standardForm>h1>em"), 10);

                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.First());
            }
        }
        public static void ShareOnTumblr(IWebDriver driver, string tumblrLogin, string tumblrPassword, string description)
        {
            string url = driverExt.GetUrl();
            bool sharePicture = false;
            if (url.Contains("product"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("tumblr-sharing-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("tumblr-sharing-link"), 10);

                driver.FindElement(By.Id("tumblr-sharing-link")).Click();
            }
            else if (url.Contains("our-ethics"))
            {
                Helpers.WaitUntilElementIsPresent(driver, By.Id("tumblr-link"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.Id("tumblr-link"), 10);

                driver.FindElement(By.Id("tumblr-link")).Click();
                sharePicture = false;
            }
            else if (url.Contains("Features"))
            {
                sharePicture = true;
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".icon-tumblr"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".icon-tumblr"), 10);

                driver.FindElement(By.CssSelector(".icon-tumblr")).Click();
            }
            else
            {
                Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".icon-tumblr"), 10);
                Helpers.WaitUntilElementIsClickable(driver, By.CssSelector(".icon-tumblr"), 10);

                driver.FindElement(By.CssSelector(".icon-tumblr")).Click();
            }
            driverExt.SwitchToLatestWindow();
            Helpers.WaitUntilElementIsPresent(driver, By.Id("signup_email"), 10);
            Helpers.WaitUntilElementIsClickable(driver, By.Id("signup_email"), 10);
            driver.FindElement(By.Id("signup_email")).Click();
            Helpers.WaitUntilElementIsPresent(driver, By.Id("signup_email"), 10);
            Helpers.WaitUntilElementIsClickable(driver, By.Id("signup_email"), 10);
            Thread.Sleep(3000);
            driver.FindElement(By.Id("signup_email")).SendKeys(tumblrLogin);
            driver.FindElement(By.Id("signup_password")).SendKeys(tumblrPassword);
            driverExt.ClickId("signup_forms_submit");
            if (sharePicture)
            {
                driverExt.ClickCss(".hover");
                driverExt.ClickCss(".flat-button.blue.right.next-button");
            }
            driver.FindElement(By.CssSelector(".editor.editor-richtext")).SendKeys(description);
            driver.FindElement(By.CssSelector(".flat-button.blue.caption.create_post_button")).Click();
            Helpers.WaitUntilElementIsPresent(driver, By.CssSelector(".success-message"), 10);
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }
        public static void ShareOnGPlus(string gplusLogin, string gplusPassword, string description)
        {
            driverExt.ClickCss(".icon-gplus");
            driverExt.SwitchToLatestWindow();

            if (WebdriverBaseClass.IsMobile)
            {
                Thread.Sleep(2000);
                driverExt.FillTextField(By.CssSelector("#Email"), gplusLogin);
                driverExt.FillTextField(By.CssSelector("#Passwd"), gplusPassword);
                driverExt.ClickCss("#signIn");
                driverExt.WaitUntilElementIsPresent(By.CssSelector("#widget_bounds > div.lQhVbe > div.z9euU > div > div.VeHKFd.PAqPJd > button"));
                driverExt.ClickCss("#widget_bounds > div.lQhVbe > div.z9euU > div > div.VeHKFd.PAqPJd > button");
                driverExt.FillTextField(By.CssSelector("#widget_bounds > div.NuWirc > div > div.SKfb8c.CgqfS.u2g9ad > div:nth-child(4) > div:nth-child(1) > div > form > textarea"), description);
                driverExt.ClickCss("#widget_bounds > div.lQhVbe > div.z9euU > div > div.VeHKFd.PAqPJd > button");

            }
            else
            {
                driverExt.WaitUntilElementIsClickable(By.Id("Email"), 10);
                driverExt.FillTextField(By.Id("Email"), gplusLogin);
                driverExt.ClickId("next");
                driverExt.FillTextField(By.Id("Passwd"), gplusPassword);
                driverExt.ClickId("signIn");
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".Rd"), 10);
                driverExt.FillTextField(By.CssSelector(".Rd"), description);
                driverExt.ClickCss(".d-k-l.b-c.b-c-Ba.qy.jt");
            }
        }

        public static void ChangeStoreToRandom(string currentStore)
        {
            Random letters = new Random();
            int number = letters.Next(0, 26);
            string currentStoreLowerCase = currentStore.ToLower();

            char firstLetter = (char)('a' + number);
            char firstLetterOfCurrentStore = currentStoreLowerCase[0];

            while (firstLetter == firstLetterOfCurrentStore)
            {
                number = letters.Next(0, 26);
                firstLetter = (char)('a' + number);
            }
            Context.ProfilePage.ChangeUserStoreMenuOpen();
            Context.ProfilePage.SearchStoreByPostcodeTownCountry(firstLetter.ToString());
            Context.ProfilePage.OpenStoreSelectList();
            driverExt.WaitUntilElementIsPresent(By.Id("store-select-list"));
            driverExt.FillTextField(By.Id("store-select-list"), OpenQA.Selenium.Keys.ArrowDown + OpenQA.Selenium.Keys.ArrowDown + OpenQA.Selenium.Keys.ArrowUp + OpenQA.Selenium.Keys.Return);
            string changedStore = driverExt.GetText(By.CssSelector(".settings-cell.label>span"));
            if (changedStore == currentStore)
            {
                Context.ProfilePage.ChangeUserStoreMenuOpen();
                Context.ProfilePage.OpenStoreSelectList();
                driverExt.FillTextField(By.Id("store-select-list"), OpenQA.Selenium.Keys.ArrowDown + OpenQA.Selenium.Keys.Return);
            }
        }
        public static void LogInToFB(string fbLogin, string fbPassword)
        {
            driverExt.WaitUntilElementIsPresent(By.Name("email"), 10);
            driverExt.FillTextField(By.Name("email"), fbLogin);
            driverExt.FillTextField(By.Name("pass"), fbPassword);
            driverExt.Click(By.Name("login"), false);
        }
        public static void SignInViaFbWhenLoggedInToFb()
        {
            Context.Header.SignIn();
            if (WebdriverBaseClass.IsMobile)
            {
                Helpers.WaitUntilElementIsPresent(WebdriverBaseClass.driver, By.CssSelector(".mobileCard"), By.CssSelector(".loginArea.transition-05s"));
                Helpers.FindFirstVisibleElement(WebdriverBaseClass.driver, By.Id("fb-login")).Click();
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".mobile-header"), 15);
            }
            else
            {
                driverExt.WaitUntilElementIsClickable(By.Id("button-signIn"), 10);
                Helpers.WaitUntilElementIsPresent(WebdriverBaseClass.driver, By.CssSelector(".card.left"), By.CssSelector(".loginArea.transition-05s"));
                Helpers.FindFirstVisibleElement(WebdriverBaseClass.driver, By.Id("fb-login")).Click();
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".desktop-header"), 15);
            }
        }
        public static string GenerateRandomLetter()
        {
            return driverExt.GenerateRandomLetter();
        }
        public static void OpenStoreDifferentFrom(string store)
        {
            if (WebdriverBaseClass.driver.FindElement(By.CssSelector(".title-level-3")).Text.Contains(store))
            {
                WebdriverBaseClass.driver.FindElement(By.XPath(".//*[@id='stores']/div[2]/div/div/a")).Click();
            }
            else
            {
                WebdriverBaseClass.driver.FindElement(By.CssSelector(".button.bg-green-dark.store-more-details-button")).Click();
            }
        }
        public static void OpenProfileOfUserWithId(string userId)
        {
            driverExt.OpenUrl(AppUrl + "en/user/" + userId);
        }
        public static void CloseCookiePolicy()
        {
            driverExt.ClickCss(".icon-close");
        }
        public static void ActivateAccount(string email, string password)
        {            
            Thread.Sleep(4000);
            ImapClient client = new ImapClient("imap.gmail.com", email, password, AuthMethods.Login, 993, true);
            client.SelectMailbox("INBOX");

            int lastMessageId = client.GetMessageCount();//.SearchMessages(SearchCondition.New().And(SearchCondition.Subject("PRIMARK - Activate account")));
            MailMessage message = client.GetMessage(lastMessageId - 1, false);
            string mailContent = message.Body;
            string start = "token=";
            string end = "\" class=\"link text-bold\">here</a> to activate";

            string token = driverExt.GetStringInBetween(start, end, mailContent, false, false);
            Console.WriteLine(mailContent);
            Console.WriteLine("token=" + token);
            driverExt.OpenUrl(AppUrl + "en/account/activate-account?token=" + token);
            driverExt.PreventGoogleapiGlitch();
        }
        public static string CreateUserNoSitecore()
        {
            string password = "qwerty";
            string email = "seleniumtester002+" + DateTime.Now.ToString("yyyyMMddHHmmss") + "@gmail.com";

            Helpers.CloseCookiePolicy();
            Context.Header.SignUp();
            if (WebdriverBaseClass.IsMobile)
            {
                driverExt.WaitUntilElementIsPresent(By.CssSelector(".mobileCard"), By.CssSelector(".textbox-signUp-firstName"));
            }
            Context.SignInSignUpPage.FillNameFieldWith("abc");
            Context.SignInSignUpPage.FillEmailSignUpFieldWith(email);
            Context.SignInSignUpPage.FillConfirmEmailSignUpFieldWith(email);
            Context.SignInSignUpPage.FillPasswordSignUpFieldWith(password);
            Context.SignInSignUpPage.ClickSignUp();
            Helpers.ActivateAccount("seleniumtester002@gmail.com", "qwer!234");

            return email;
        }

        public static void Refresh()
        {
            driverExt.RefreshPage();
        }
        public static void CreateOutfit(string outfitName, bool challenge = false)
        {
            Context.Header.OpenProducts();
            Context.ProductsPage.CloseCategoryMenu();
            Context.ProductsPage.ViewFirstProductDetails();
            Context.OutfitBuilder.ClickAddToOutfitButton();
            if (challenge == false)
            {
                Context.OutfitBuilder.CreateOutfitForYourself();
            }
            else
            {
                try
                {
                    Context.OutfitBuilder.CreateOutfitForAChallenge();
                }
                catch
                {
                    Assert.Fail("No active challenge or create outfit for challenge button disappeared!");
                }
            }
            Context.Assertions.OutfitAssertions.CompleteOutfitButtonInactive();
            Context.OutfitBuilder.ToggleOutfitBuilderTab();
            Context.ProductsPage.NextProduct();
            Context.OutfitBuilder.ClickAddToOutfitButton();
            Context.Assertions.OutfitAssertions.CompleteOutfitButtonInactive();
            Context.OutfitBuilder.ToggleOutfitBuilderTab();
            Context.ProductsPage.NextProduct();
            Context.OutfitBuilder.ClickAddToOutfitButton();
            Context.OutfitBuilder.ToggleOutfitBuilderTab();
            Context.OutfitBuilder.CompleteOutfit();
            Context.OutfitBuilder.NameOutfit(outfitName);
            if (challenge == false)
            {
                Context.OutfitBuilder.SaveOutfitToProfile();
            }
            else
            {
                Context.OutfitBuilder.SubmitOutfitToChallenge();
            }
        }

        public static void HideHeader()         //need to hide header and footer due to problems with test failing on small resolutions
        {
            string hideFooter = "jQuery(document.getElementsByClassName('desktop-header')).css('display', 'none');";
            driverExt.ExecuteJavaScript(hideFooter);
        }
        public static void HideFooter()         //need to hide header and footer due to problems with test failing on small resolutions
        {
            string hideHeader = "jQuery(document.getElementsByClassName('desktop-footer')).css('display', 'none');";
            driverExt.ExecuteJavaScript(hideHeader);
        }
        public static bool ScanURLs(int i, bool signedInFlag, string[] urls)
        {
            switch (urls[i])
            {
                case "en/primania/look/142674,casual-by-tester-s":
                    Context.Header.OpenPrimania();
                    Context.PrimaniaPage.OpenFirstLookDeeplink();
                    break;
                case "en/features/Women/2015/02/SebaTest3":
                    driverExt.OpenUrl(AppUrl + "en/features/Women");
                    driverExt.ClickCss(".box-ctaButton.feature-button");
                    break;
                case "en/product/text-to-translate41,R1991627320150104":
                    Context.Header.OpenProducts();
                    Context.ProductsPage.OpenFirstProductDeeplink();
                    break;
                case "en/account/settings":              //For URLs requiring a signed in user
                case "en/user/150995/followers":
                case "en/user/150995/looks":
                case "en/user/150995/favourites":
                    if (signedInFlag == false)
                    {
                        Helpers.SignIn(WebdriverBaseClass.driver, Users.PrimarkUser001.Email, Users.PrimarkUser001.Password);
                        signedInFlag = true;
                    }
                    switch (urls[i])
                    {
                        case "en/account/settings":
                            Context.Header.EditProfile();
                            break;
                        case "en/user/150995/followers":
                            Context.Header.ViewProfile();
                            Context.ProfilePage.ViewFollowersFollowingTab();
                            break;
                        case "en/user/150995/looks":
                            Context.Header.ViewProfile();
                            Context.ProfilePage.ViewLooksTab();
                            break;
                        case "en/user/150995/favourites":
                            Context.Header.ViewProfile();
                            Context.ProfilePage.ViewFavouritesTab();
                            break;
                    }
                    break;
                default:
                    driverExt.OpenUrl(AppUrl + urls[i]);
                    break;
            }
            return signedInFlag;    
            
        }
    }
}
