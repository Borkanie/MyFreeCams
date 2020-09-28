using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;


namespace Traffic_Booster
{
    public class Members
    {
        public Members(string prx, bool modus, string useragent, string[] link)
        {
            try
            {
                links = link;
                var cService = ChromeDriverService.CreateDefaultService();
                cService.HideCommandPromptWindow = true;
                var options = new ChromeOptions();
                options.AddArguments("--proxy-server=" + prx);
                options.AddArgument("--no-sandbox");
                if (!modus)
                {
                    options.AddArgument("--headless");
                }
                options.AddArgument("--disable-gpu");
                options.AddArgument("--windows-size=2220,1080");
                //options.AddArgument("--disable-web-security");
                //options.AddArgument("--user-data-dir");
                options.AddArgument("--disable-crash-reporter");
                options.AddArgument("--disable-default-apps");
                // options.AddArgument("--disable-extensions");
                options.AddArguments("chrome.switches", "--disable-extensions");
                // options.AddArgument("--incognito");
                options.AddArgument("--disable-logging");
                // options.AddExcludedArguments(new List<string>() { "enable-automation" });
                options.AddArgument("--disable-sync");
                options.AddArgument("--user-agent=" + useragent);
                webDriver = new ChromeDriver(cService, options, TimeSpan.FromSeconds(3600));

            }
            catch (Exception Ex)
            {
                String s = Ex.Message;
                webDriver.Quit();
            }
        }
        #region fields
        private bool ON = false;
        private ChromeDriver webDriver;
        private string[] links;
        private static readonly Random getrandom = new Random();
        #endregion
        #region Methods
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
        //facem o functie unde creeam un trab nou si dam login si nabigare dupa schimbam iara tabul
        // --asa facem un tab nou
        public void Login_on_site(string username, string password, bool referer)
        {
            webDriver.Manage().Cookies.DeleteAllCookies(); //delete all cookies
            if (referer)
            {

                webDriver.Navigate().GoToUrl("https://thebestfetishsites.com/link/mfc/");
                webDriver.Navigate().GoToUrl("https://www.myfreecams.com/#Homepage");
                Thread.Sleep(3000);
                webDriver.FindElement(By.XPath("//*[@id='enter_desktop']")).Click();
                // Thread.Sleep(10000);

                // goto Normal;

            }
            else
            {
                webDriver.Navigate().GoToUrl("https://www.myfreecams.com/#Homepage");
                Thread.Sleep(3000);
                webDriver.FindElement(By.XPath("//*[@id='enter_desktop']")).Click();
                Thread.Sleep(3000);
            }
            try
            {
                webDriver.FindElement(By.XPath("//*[@id='username_input']")).SendKeys(username);
                Thread.Sleep(2000);
                webDriver.FindElement(By.XPath("//*[@id='password_input']")).SendKeys(password);
                Thread.Sleep(3000);
                webDriver.FindElement(By.XPath("//*[@id='login_box']/form/table/tbody/tr/td[5]/input")).Click();
                Thread.Sleep(3000);
                this.ON = true;
            }
            catch (Exception)
            {

                webDriver.Close();
                webDriver.Quit();

            }
        }//aici deschidem mai muklte taburi si navigham
        public void Write_on_chat(string[] lines)
        {
            try
            {
                int nr = 2;
                nr = GetRandomNumber(0, lines.Length);
                for (int i = links.Length; i > 0; i--)
                {
                    try
                    {
                        webDriver.SwitchTo().Window(this.webDriver.WindowHandles[i]);
                        try
                        {
                            var webelement = webDriver.FindElements(By.XPath("//*[@id='column_container']/div[5]/div[1]/div/div/div[4]/div[3]/div[2]/div/div[1]/div[1]/textarea"));
                            webelement[0].SendKeys(lines[nr]);
                            webelement[0].SendKeys(Keys.Enter);
                            Thread.Sleep(2000);
                            IJavaScriptExecutor js4 = (IJavaScriptExecutor)webDriver;
                            Thread.Sleep(2000);
                        }
                        catch { }
                    }
                    catch (Exception)
                    {
                        webDriver.Navigate().GoToUrl(webDriver.Url);
                    };
                }
                Thread.Sleep(60000);
                nr = GetRandomNumber(0, lines.Length);
                for (int i = links.Length; i > 0; i--)
                {

                    try
                    {
                        webDriver.SwitchTo().Window(this.webDriver.WindowHandles[i]);
                        try
                        {
                            var webelement = webDriver.FindElements(By.XPath("//*[@id='column_container']/div[5]/div[1]/div/div/div[4]/div[3]/div[2]/div/div[1]/div[1]/textarea"));
                            webelement[0].SendKeys(lines[nr]);
                            webelement[0].SendKeys(Keys.Enter);
                            Thread.Sleep(2000);
                        }
                        catch { }
                    }
                    catch (Exception)
                    {
                        webDriver.Navigate().GoToUrl(webDriver.Url);

                    };
                }
                Thread.Sleep(80000);
                nr = GetRandomNumber(0, lines.Length);
                for (int i = links.Length; i > 0; i--)
                {

                    try
                    {
                        webDriver.SwitchTo().Window(this.webDriver.WindowHandles[i]);

                        try
                        {
                            var webelement = webDriver.FindElements(By.XPath("//*[@id='column_container']/div[5]/div[1]/div/div/div[4]/div[3]/div[2]/div/div[1]/div[1]/textarea"));
                            webelement[0].SendKeys(lines[nr]);
                            webelement[0].SendKeys(Keys.Enter);
                            Thread.Sleep(2000);
                            IJavaScriptExecutor js4 = (IJavaScriptExecutor)webDriver;
                            Thread.Sleep(2000);
                        }
                        catch { }
                    }
                    catch (Exception)
                    {
                        webDriver.Navigate().GoToUrl(webDriver.Url);

                    };
                }
                Thread.Sleep(120000);
                nr = GetRandomNumber(0, lines.Length);
                for (int i = links.Length; i > 0; i--)
                {

                    try
                    {
                        webDriver.SwitchTo().Window(this.webDriver.WindowHandles[i]);

                        try
                        {
                            var webelement = webDriver.FindElements(By.XPath("//*[@id='column_container']/div[5]/div[1]/div/div/div[4]/div[3]/div[2]/div/div[1]/div[1]/textarea"));
                            webelement[0].SendKeys(lines[nr]);
                            webelement[0].SendKeys(Keys.Enter);
                            Thread.Sleep(2000);
                            Thread.Sleep(2000);
                        }
                        catch { }

                    }
                    catch (Exception)
                    {
                        webDriver.Navigate().GoToUrl(webDriver.Url);

                    };
                }
                Thread.Sleep(35000);

            }
            catch (Exception ex)
            {

                string stringdeeroare = ex.Message;
            };

            this.Write_on_chat(lines);

        }//scriem pe chat
        //chemam start mutkliple si write
        public void Navigate(string[] links)
        {
            IJavaScriptExecutor js4 = (IJavaScriptExecutor)webDriver;
            int tabnumber = 0;
            foreach (string link in links)
            {
                try
                {

                    ((IJavaScriptExecutor)webDriver).ExecuteScript("window.open();");
                    tabnumber++;
                    if (link != links[0])
                    {
                        webDriver.SwitchTo().Window(webDriver.WindowHandles[tabnumber]);
                        webDriver.Navigate().GoToUrl("https://myfreecams.com/#" + link);
                        Thread.Sleep(5000);
                        try
                        {
                            webDriver.FindElement(By.XPath("//*[@id='enter_desktop']")).Click();
                        }
                        catch { }

                        js4.ExecuteScript(" document.getElementById('video_area').remove()");
                        js4.ExecuteScript(" document.getElementById('friends_table').remove()");
                        js4.ExecuteScript(" document.getElementById('online_broadcasters').remove()");

                        Thread.Sleep(2000);
                    }


                }
                catch (Exception ex) { string s = ex.Message; }
                //da crash tot nush dc
            }
            try
            {
                webDriver.SwitchTo().Window(webDriver.WindowHandles[1]);
                webDriver.Navigate().GoToUrl("https://myfreecams.com/#" + links[0]);
                try
                {
                    webDriver.FindElement(By.XPath("//*[@id='enter_desktop']")).Click();
                    js4.ExecuteScript(" document.getElementById('video_area').remove()");
                    js4.ExecuteScript(" document.getElementById('friends_table').remove()");
                    js4.ExecuteScript(" document.getElementById('online_broadcasters').remove()");
                }
                catch { }
                Thread.Sleep(2000);

            }
            catch { }
        }
        public void Kill()
        {
            try
            {
                foreach (var tab in webDriver.WindowHandles)
                    webDriver.Close();
                webDriver.Quit();
                ON = false;
                Thread.CurrentThread.Abort();
            }
            catch { };
        }//omoram thread
        public void GoHome()
        {
            try
            {

                for (int i = webDriver.WindowHandles.Count - 1; i > 0; i--)
                {

                    webDriver.SwitchTo().Window(webDriver.WindowHandles[i]);
                    webDriver.Close();

                }
                webDriver.SwitchTo().Window(webDriver.WindowHandles[0]);
                Thread.CurrentThread.Abort();
            }
            catch (Exception) { }

        }//dam refresh la membrii
        #endregion
        #region Get/SetMethods
        public bool is_on()
        {
            return ON;
        }
        #endregion
    }
}
