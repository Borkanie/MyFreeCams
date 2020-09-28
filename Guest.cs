using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
namespace Traffic_Booster
{
    public class Guest
    {
        public Guest(string proxy, bool modus)
        {
            ON = false;
            try
            {
                var cService = ChromeDriverService.CreateDefaultService();
                // var cService = FirefoxDriverService.CreateDefaultService();
                cService.HideCommandPromptWindow = true;
                // FirefoxOptions options = new FirefoxOptions();
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
                if (proxy.Split(':').Length < 3)
                    options.AddArguments("--proxy-server=" + proxy);
                else
                {
                    string[] proxypieces = proxy.Split(':');
                    String URL = "http://" + proxypieces[2] + ":" + proxypieces[3] + "@" + proxypieces[0] + ":" + proxypieces[1];
                    options.AddArguments("--proxy-server=" + URL);
                }
                options.AddArgument("--incognito");
                options.AddArgument("--no-sandbox");
                if (!modus)
                    options.AddArgument("--headless");
                //options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-web-security");
                //options.AddArgument("--user-data-dir");
                options.AddArgument("--disable-crash-reporter");
                //options.AddArgument("--disable-default-apps");
                //  options.AddArgument("--disable-extensions");
                // options.AddArgument("--disable-logging");
                options.AddExcludedArguments(new List<string>() { "enable-automation" });
                options.AddArgument("--disable-sync");
                options.AddArgument("--user-agent=" + userAgent);
                webDriver = new ChromeDriver(cService, options, TimeSpan.FromSeconds(3600));
                int bk = cService.ProcessId;
                ON = true;
            }
            catch (Exception ex)
            {
                nr_failed++;
                string s = ex.Message;
            };
        }//creeam browserul
        #region Fields
        private ChromeDriver webDriver;
        private bool ON;
        private static int nr_failed = 0;
        #endregion
        #region Methods
        private void Refresh(string[] links)
        {


            Thread.Sleep(90000);

            foreach (var window in webDriver.WindowHandles)
            {

                try
                {
                    webDriver.SwitchTo().Window(window);
                    if (webDriver.Url != "data:,")
                    {
                        webDriver.Navigate().Refresh();
                        Thread.Sleep(5000);
                        try
                        {
                            webDriver.FindElement(By.XPath("//*[@id='enter_desktop']"));
                        }
                        catch { }
                        IJavaScriptExecutor js4 = (IJavaScriptExecutor)webDriver;
                        string trem_vid = (string)js4.ExecuteScript(" document.getElementById('video_area').remove()");
                        trem_vid = (string)js4.ExecuteScript(" document.getElementById('online_broadcasters').remove()");
                        trem_vid = (string)js4.ExecuteScript("XMLHttpRequest.prototype.send = function() {return false; }");
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception)
                {
                    webDriver.Close();
                };
            };

            if (webDriver.WindowHandles.Count > 0)
                this.Refresh(links);

        }//dam refresh la taburi
        public void StartMultipleTabs(string[] links)
        {
            var wait = webDriver.Manage().Timeouts().ImplicitWait;
            //webDriver.Manage().Timeouts().ImplicitWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(15)).Until(ExpectedConditions.ElementExists((By.Id(""))));
            int tabnumber = 0;
            IJavaScriptExecutor js4 = (IJavaScriptExecutor)webDriver;
            foreach (string link in links)
            {
                try
                {

                    js4.ExecuteScript("window.open();");
                    tabnumber++;
                    if (link != links[0])
                    {
                        webDriver.SwitchTo().Window(webDriver.WindowHandles[tabnumber]);
                        webDriver.Navigate().GoToUrl("https://myfreecams.com/#" + link);
                        Thread.Sleep(10000);
                        try
                        {
                            webDriver.FindElement(By.XPath("//*[@id='enter_desktop']")).Click();
                        }
                        catch { }

                        js4.ExecuteScript(" document.getElementById('video_wrapper').remove()");
                        js4.ExecuteScript(" document.getElementById('online_broadcasters').remove()");
                        webDriver.FindElement(By.TagName("body")).SendKeys("Keys.ESCAPE");
                        //trem_vid = (string)js4.ExecuteScript("XMLHttpRequest.prototype.send = function() {return false; }");
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    string error_message = ex.Message;
                    webDriver.Close();
                    tabnumber--;
                    webDriver.SwitchTo().Window(webDriver.WindowHandles[tabnumber]);
                }
                //da crash tot nush dc
            }
            try
            {
                webDriver.SwitchTo().Window(webDriver.WindowHandles[1]);
                webDriver.Navigate().GoToUrl("https://myfreecams.com/#" + links[0]);
                Thread.Sleep(10000);
                try
                {
                    webDriver.FindElement(By.XPath("//*[@id='enter_desktop']"));
                }
                catch { }

                js4.ExecuteScript(" document.getElementById('video_wrapper').remove()");
                js4.ExecuteScript(" document.getElementById('online_broadcasters').remove()");
                webDriver.FindElement(By.TagName("body")).SendKeys("Keys.ESCAPE");
                // trem_vid = (string)js4.ExecuteScript(" window.stop");

                Thread.Sleep(1000);

            }
            catch { webDriver.Close(); }


        }//deshidem taburi si navigham pe rpfile
        public void Kill()
        {
            try
            {
                nr_failed++;
                foreach (var tab in webDriver.WindowHandles)
                    webDriver.Close();
                webDriver.Quit();
                ON = false;
                Thread.CurrentThread.Abort();
            }
            catch { };
        }//omoram threadul
        #endregion
        #region Get/SetMethods
        static public int get_nr_failed()
        {
            return nr_failed;
        }//numaram cati nu merg
        static public void set_nr_failed(int n)
        {
            nr_failed = n;
        }
        public bool is_on()
        {
            return ON;
        }
        #endregion
    }
}
