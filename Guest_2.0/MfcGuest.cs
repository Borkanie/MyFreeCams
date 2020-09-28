using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using WebSocketSharp;
using Traffic_Booster.Guest_2._0;
using System.Net;


namespace Traffic_Booster.Guest_2._0
{
    class MfcGuest
    {
        #region Fields
        //lsita dde toate serverele pentru fete
        public static string[] servers = new string[] { "xchat107", "xchat108", "xchat61", "xchat94", "xchat109", "xchat37", "xchat38", "xchat47", "xchat48", "xchat49", "xchat50", "xchat51", "xchat52", "xchat53", "xchat54", "xchat57", "xchat26", "xchat95", "xchat111", "xchat112", "xchat113", "xchat114", "xchat115", "xchat116", "xchat118", "xchat119", "xchat41", "xchat42", "xchat43", "xchat44", "xchat58", "xchat27", "xchat45", "xchat46", "xchat39", "xchat59", "xchat120", "xchat121", "xchat122", "xchat123", "xchat124", "xchat125", "xchat126", "xchat67", "xchat66", "xchat62", "xchat63", "xchat64", "xchat65", "xchat69", "xchat70", "xchat71", "xchat72", "xchat73", "xchat74", "xchat75", "xchat76", "xchat77", "xchat40", "xchat60", "xchat80", "xchat28", "xchat29", "xchat30", "xchat31", "xchat32", "xchat33", "xchat34", "xchat35", "xchat36", "xchat90", "xchat91", "xchat92", "xchat93", "xchat81", "xchat82", "xchat83", "xchat79", "xchat68", "xchat78", "xchat84", "xchat85", "xchat86", "xchat87", "xchat88", "xchat89", "xchat96", "xchat97", "xchat98", "xchat99", "xchat100", "xchat101", "xchat102", "xchat103", "xchat104", "xchat105", "xchat106", "xchat110", "xchat127", "xchat2", "xchat3", "xchat4", "xchat5", "xchat6", "xchat7", "xchat8", "xchat9", "xchat10", "xchat11", "xchat12", "xchat13", "xchat14", "xchat15", "xchat16", "xchat17", "xchat18", "xchat19", "xchat20", "xchat21", "xchat22", "xchat23", "xchat24", "xchat25" };
        //rendam local pe tot Proxy-ul
        public static Random Random = new Random();
        //threaduri pentru fiecare model
        public static List<Thread> Threads = new List<Thread>();
        //modele
        private List<Model> Models;
        //socketuri
        private List<GuestSocket> Sockets = new List<GuestSocket>();
        //proxy
        private WebSharerProxy Proxy;
        //Consola pentru emsaje
        private Consola_Form Consola;
        #endregion
        #region methods
        //constructor
        public MfcGuest(List<Model> model, WebSharerProxy proxy,Consola_Form consola)
        {
            Consola = consola;
            Proxy = proxy;
            //Client = new HttpClient();
            this.Models = model;
        }
        //inchidem socketurile
        public void Close()
        {
            foreach(var socket in Sockets)
            {
                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Consola.WriteLine(ex.Message);
                } 
            }         
        }
        //generam socheturile pe thread separat pentru fiecare
        public void generatebotsAsync(int i)
        {
            
            GuestSocket.RandomGenerator = Random;
            foreach (var model in Models)
            {
                try
                {

                    model.Number = i;
                    GuestSocket socket = new GuestSocket(servers[Random.Next(0, servers.Length)], Random, Proxy, model, Consola);
                    Thread t1 = new Thread(() => socket.Ping());
                    socket.SetPingThread(t1);
                    lock(Threads){
                        Threads.Add(t1);
                    }
                    t1.Start();
                    Sockets.Add(socket);

                }
                catch (Exception ex)
                {
                    Consola.WriteLine(ex.Message);
                }
            }
        }
        #endregion
        #region outdated
        private int GetCode(string model)
        {
            try
            {
                string url = @"https://profiles.myfreecams.com/" + model;
                // First create a proxy object
                WebProxy proxy;
                if (Proxy.Username!=string.Empty)
                {
                    proxy = new WebProxy
                    {
                        Address = new Uri($"http://" + Proxy.Url + ":" + Proxy.Port),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = false,

                        // *** These creds are given to the proxy server, not the web server ***
                        Credentials = new NetworkCredential(
                       userName: Proxy.Username,
                       password: Proxy.Password)
                    };
                }
                else
                {
                    proxy = new WebProxy(Proxy.GetLink());
                }
               

                // Now create a client handler which uses that proxy
                var httpClientHandler = new HttpClientHandler
                {
                    //Proxy = proxy,
                };

                // Omit this part if you don't need to authenticate with the web server:
                //if (needServerAuthentication)
                //{
                //    httpClientHandler.PreAuthenticate = true;
                //    httpClientHandler.UseDefaultCredentials = false;

                //    // *** These creds are given to the web server, not the proxy server ***
                //    httpClientHandler.Credentials = new NetworkCredential(
                //        userName: serverUserName,
                //        password: serverPassword);
                //}

                // Finally, create the HTTP client object
                HttpClient Client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                var cookieContainer = new CookieContainer();
                //HttpClient Client = new HttpClient(new HttpClientHandler() { CookieContainer = cookieContainer });
                HttpResponseMessage placebetResponseMessage = Client.GetAsync(url).Result;

                placebetResponseMessage.EnsureSuccessStatusCode();
                string html = placebetResponseMessage.Content.ReadAsStringAsync().Result;
                int code = Convert.ToInt32(html.Split(new string[] { "nProfileUserId: " }, StringSplitOptions.None)[1].Split(',')[0]);

                return code;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
        #endregion

    }
}
