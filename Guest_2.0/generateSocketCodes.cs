using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Traffic_Booster.Guest_2._0
{
    public class generateSocketCodes
    {
        //lsita de threaduri pe care o sa rulam socketuri
        private static List<Thread> Threads;
        //lsita de generatori de guesturi
        private static List<MfcGuest> Guests = new List<MfcGuest>();
        //lsita de proxy pentru guestii
        List<WebSharerProxy> proxys = new List<WebSharerProxy>();
        //lsita de modele
        List<Model> Models = new List<Model>();
        //consola pentru mesaje
        Consola_Form Consola;
        //rulam pe thread separat si generam threaduri pentru fiecare Socket(1 proxy combinat)
        //fiecare Proxy o sa aiba un thread si fieacare thread si fiecare thread os a genereze la randul lui un numar de
        //threaduri egal cu numarl de profile
        public void generateGuests(MainMFC main,Consola_Form consola)
        {
            if(Threads==null)
                Threads = main.jasmin_threads;
            Consola = consola;
            //facem lista de modele 
            for (int i=0; i<main.GetProfiles().Length;i++)
            {
                if (main.GetProfiles()[i].Split('/')[0].Split(':').Length == 2)
                    Models.Add(new Model() { Name = main.GetProfiles()[i].Split('/')[0].Split(':')[0], Code = Convert.ToInt32(main.GetProfiles()[i].Split('/')[0].Split(':')[1]),Number=i });
                else
                    Consola.WriteLine(main.GetProfiles()[i].Split('/')[0] + " doesn't have a code");
            }
            if(Models.Count>0)
            {
                //facem lsita de proxy-uri
                foreach (var line in main.GetGuetsProxyListBox())
                {
                    var tempProx = new WebSharerProxy(line.ToString());
                    if (tempProx.Url != null)
                        proxys.Add(tempProx);
                }

                //genram Socketuri pe threaduri multiple
                var rand = new Random();
                for (int i = 0; i < Convert.ToInt32(main.GetGuestTextBox()); i++)
                {

                    Thread t1 = new Thread(() => CreateGuestInstance(Models, i, proxys, rand.Next(0, proxys.Count)));
                    Threads.Add(t1);
                    t1.Start();
                    main.jasmin_online_guests();
                    Thread.Sleep(rand.Next(0, 5000));
                }
            }
            else
            {
                Consola.WriteLine("Cannot use new Guests please generate codes while the girls are online");
            }
           
        }
        //dezactivam socketuruile(vor mai rula pana cand trece un ciclu de ping)
        public void Stop()
        {
            foreach(var guest in Guests)
            {
                guest.Close();
            }
        }
        //generam un threaduri pentru fiecare Socket(1 proxy combinat cu 1 Model)
        private void CreateGuestInstance(List<Model> Models, int i, List<WebSharerProxy> proxys,int random)
        {
            //aici facem defapt socketul
            MfcGuest Guest = new MfcGuest(Models, proxys[random],Consola);
            Guest.generatebotsAsync(i);
            lock (Guests)
            {
                Guests.Add(Guest);
            }


        }

        #region outdated

        //private void GetCodes(List<Model> Models)
        //{
        //    //  if (Proxy.Username != string.Empty)
        //    //{
        //    //    webProxy = new WebProxy(Proxy.GetLink());
        //    //}
        //    //else
        //    //{
        //    //            webProxy = new WebProxy
        //    //            {
        //    //                Address = new Uri($"http://" + Proxy.Url + ":" + Proxy.Port),
        //    //                BypassProxyOnLocal = false,
        //    //                UseDefaultCredentials = false,

        //    //                // *** These creds are given to the proxy server, not the web server ***
        //    //                Credentials = new NetworkCredential(userName: Proxy.Username, password: Proxy.Password)
        //    //            };
        //    //        }
        //    Consola.WriteLine("getting girl codes");
        //    var cService = ChromeDriverService.CreateDefaultService();
        //    cService.HideCommandPromptWindow = true;
        //    ChromeOptions options = new ChromeOptions();
        //    string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
        //    options.AddArgument("--no-sandbox");
        //    options.AddArgument("--headless");
        //    options.AddArgument("--disable-gpu");
        //    options.AddArgument("--disable-web-security");
        //    options.AddArgument("--disable-crash-reporter");
        //    options.AddArgument("--disable-default-apps");
        //    options.AddArgument("--disable-extensions");
        //    options.AddArgument("--disable-logging");
        //    options.AddExcludedArguments(new List<string>() { "enable-automation" });
        //    options.AddArgument("--disable-sync");
        //    options.AddArgument("--user-agent=" + userAgent);
        //    ChromeDriver chrome = new ChromeDriver(cService, options, TimeSpan.FromSeconds(3600));
        //    string url = "https://www.myfreecams.com/#Homepage";
        //    chrome.Navigate().GoToUrl(url);
        //    Thread.Sleep(5000);
        //    chrome.FindElementByXPath("//*[@id=\"enter_desktop\"]").Click();
        //    foreach (var model in Models)
        //    {
        //        try
        //        {
        //            chrome.FindElementByXPath("//*[@id=\"online_model_search\"]").SendKeys(model.Name + Keys.Enter);
        //            Thread.Sleep(1000);

        //            var element = chrome.FindElementByXPath("/html/body/div[5]/table/tbody/tr[4]/td/table/tbody/tr[2]/td/table/tbody/tr[1]/td/div/div[2]/div/table/tbody/tr/td/div/table/tbody/tr[2]/td/div/div/table/tbody/tr[1]/td/div[2]/div[2]/div/div/div/div[2]/img");
        //            model.Code = Convert.ToInt32(element.GetAttribute("data-src").Split(new string[] { "mfc_" }, StringSplitOptions.None)[1].Split(new string[] { "?no-" }, StringSplitOptions.None)[0]);
        //            Thread.Sleep(1000);
        //            IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
        //            string title = (string)js.ExecuteScript("document.getElementById(\"online_model_search\").value='';");
        //            Thread.Sleep(1000);
        //            Consola.WriteLine("Done for:" + model.Name);
        //        }
        //        catch (Exception ex)
        //        {
        //            IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
        //            string title = (string)js.ExecuteScript("document.getElementById(\"online_model_search\").value='';");
        //            Thread.Sleep(1000);
        //            Consola.WriteLine("Error for:" + model.Name);
        //        }
        //    }
        //    chrome.Close();
        //    Consola.WriteLine("Done with codes");
        //}
        #endregion
    }
}
