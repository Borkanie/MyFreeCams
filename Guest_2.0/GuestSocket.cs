using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Traffic_Booster.Guest_2._0;
using WebSocketSharp;
using System.Net.WebSockets;
using System.Net;
using System.Runtime.CompilerServices;

namespace Traffic_Booster.Guest_2._0
{
    class GuestSocket
    {
        #region Fields
        private string Server;
        private WebSharerProxy Proxy;
        public static Random RandomGenerator;
        private Model Model;
        private WebSocketSharp.WebSocket Client;
        //Mesaj de login ca guest
        private Message LoginMessage = new Message() { c = 1, t = 0, f = 0, a1 = 20071025, a2 = 0, Data = "1/guest:guest" };
        //mesaj de ping repetat la 20 de secunde
        private Message PingMessage = new Message() { c = 0, t = 0, f = 0, a1 = 0, a2 = 0, Data = string.Empty };
        //mesaj de logare pe chatul fetei
        private Message GirlMessage = new Message() { c = 10, t = 0, f = 0, a2 = 0 };
        private Consola_Form Consola;
        private Thread PingThread;
        public bool hasACcount = false;
        private bool ON = false;
        #endregion
        #region methods
        public GuestSocket(string server, Random random, WebSharerProxy proxy, Model model, Consola_Form consola)
        {
            Consola = consola;
            ON = true;
            Proxy = proxy;
            Model = model;

            RandomGenerator = random;
            string webSocketBaseUrl = "wss://" + server + ".myfreecams.com/fcsl";
            Server = server;
            //webSocketBaseUrl = "wss://xchat83.myfreecams.com/fcsl";
            var url = new Uri(webSocketBaseUrl);

            Client = new WebSocketSharp.WebSocket(webSocketBaseUrl);

            //aici setam evente pentru socket
            Client.OnOpen += new EventHandler(OnSocketOpened);
            Client.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(OnSocketError);
            Client.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(OnSocketClosed);
            Client.OnMessage += new EventHandler<WebSocketSharp.MessageEventArgs>(OnSocketMessageReceived);
            Client.SetProxy(this.Proxy.GetLink(), Proxy.Username, Proxy.Password);

            Client.Connect();
            //ne conectam
            string word = "fcsws_20180422\n\0";
            Consola.WriteLine(model.Number.ToString() + " Connecting to socket");
            Client.Send(word);
            Thread.Sleep(3000);
            //ne logam
            Consola.WriteLine(model.Number.ToString() + " Logging in as guest");
            Client.Send(LoginMessage.ToString());
            Thread.Sleep(3000);
            while (GirlMessage.t == 0)
            {
                Thread.Sleep(1000);
            }
            //ne redirectionam la fata
            Consola.WriteLine(model.Number.ToString() + " Connecting to girl");
            Client.Send(GirlMessage.ToString());
            Thread.Sleep(3000);
            Consola.WriteLine(model.Number.ToString() + " Message 1");
            //nu stiu ce face mesajul dar e necesar
            Client.Send(new Message() { c = 44, t = GirlMessage.t, f = 0, a1 = 1, a2 = 0, Data = string.Empty }.ToString());

            Consola.WriteLine(model.Number.ToString() + " Message 2");
            //nu stiu ce face mesajul dar e necesar
            Client.Send(new Message() { c = 51, t = GirlMessage.t, f = 0, a1 = model.Code, a2 = 9, Data = string.Empty }.ToString());


        }
        //sa o putem opri fortat
        internal void Close()
        {
           ON = false;
           if(PingThread!=null)
                if(PingThread.IsAlive)
                    PingThread.Abort();   
        }
        //setam tjhreadul care ramane sa dea ping pentru a il putea inchide mai incolo
        public void SetPingThread(Thread ping)
        {
            PingThread = ping;
        }
        //dam ping
        public void Ping()
        {
            
            try { 
            while (ON)
            {

                //Consola.WriteLine("Bobo:" + word);
                Consola.WriteLine(Model.Number.ToString() + " PING   " + Model.Name);
                Client.Send(PingMessage.ToString());
                Thread.Sleep(30000);
            }
            }
            catch(Exception ex)
            {
                Console.WriteLine(Model.Number.ToString() + " ERROR " + Model.Name);
                Client.Close();
            }
          
            Consola.WriteLine(Model.Number.ToString() + " closed " + Model.Name);
        }
        //emsaj la deshcidere socket
        private void OnSocketOpened(object sender, EventArgs e)
        {
            Consola.WriteLine(Model.Number.ToString() + " Socket opned");
        }
        //mesaj al eroare socket
        private void OnSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Consola.WriteLine(Model.Number.ToString() + "Socket ERROR:" + e.Message);
            Consola.WriteLine(Model.Number.ToString() + "Exception:" + e.Exception);
        }
        //mesajd e inchdiere
        private void OnSocketClosed(object sender, WebSocketSharp.CloseEventArgs e)
        {
            Consola.WriteLine(Model.Number.ToString() + "Socket Closing with code" + e.Code);
            if(e.Code==1006)
            {
                string webSocketBaseUrl = "wss://" + this.Server + ".myfreecams.com/fcsl";
                //webSocketBaseUrl = "wss://xchat83.myfreecams.com/fcsl";
                var url = new Uri(webSocketBaseUrl);

                Client = new WebSocketSharp.WebSocket(webSocketBaseUrl);

                //aici setam evente pentru socket
                Client.OnOpen += new EventHandler(OnSocketOpened);
                Client.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(OnSocketError);
                Client.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(OnSocketClosed);
                Client.OnMessage += new EventHandler<WebSocketSharp.MessageEventArgs>(OnSocketMessageReceived);
                Client.SetProxy(this.Proxy.GetLink(), Proxy.Username, Proxy.Password);

                Client.Connect();
                //ne conectam
                string word = "fcsws_20180422\n\0";
                Consola.WriteLine(this.Model.Number.ToString() + " Connecting to socket");
                Client.Send(word);
                Thread.Sleep(3000);
                //ne logam
                Consola.WriteLine(this.Model.Number.ToString() + " Logging in as guest");
                Client.Send(LoginMessage.ToString());
                Thread.Sleep(3000);
                while (GirlMessage.t == 0)
                {
                    Thread.Sleep(1000);
                }
                //ne redirectionam la fata
                Consola.WriteLine(this.Model.Number.ToString() + " Connecting to girl");
                Client.Send(GirlMessage.ToString());
                Thread.Sleep(3000);
                Consola.WriteLine(this.Model.Number.ToString() + " Message 1");
                //nu stiu ce face mesajul dar e necesar
                Client.Send(new Message() { c = 44, t = GirlMessage.t, f = 0, a1 = 1, a2 = 0, Data = string.Empty }.ToString());

                Consola.WriteLine(this.Model.Number.ToString() + " Message 2");
                //nu stiu ce face mesajul dar e necesar
                Client.Send(new Message() { c = 51, t = GirlMessage.t, f = 0, a1 = this.Model.Code, a2 = 9, Data = string.Empty }.ToString());
                if(PingThread.ThreadState==ThreadState.Running)
                {
                    PingThread.Abort();
                }
                PingThread = new Thread(() => this.Ping());
                PingThread.Start();

            }
            Consola.WriteLine(Model.Number.ToString() + e.Reason);
        }
        //priomire mesaj aici defapt asteptam numais a primim codul unic al camerei
        private void OnSocketMessageReceived(object sender, WebSocketSharp.MessageEventArgs e)
        {
            if (GirlMessage.t == 0)
            {
                try
                {
                    GirlMessage.t = Convert.ToInt32(e.Data.Split(' ')[2]);
                    GirlMessage.Data = this.Model.Name;
                    lock (RandomGenerator)
                    {
                        GirlMessage.a1 = Convert.ToInt32(RandomGenerator.NextDouble() * int.MaxValue);
                    }
                }
                catch (Exception ex)
                {
                    Consola.WriteLine(ex.Message);
                }
            }    
        }
        #endregion
        #region outdated
        private string GetRandomString(int numberOfChars)
        {
            string random = string.Empty;
            lock (RandomGenerator)
            {
                char[] characters = "abcdefghijklmnopqrstuvwxyz012345".ToCharArray();
                for (int i = 0; i < numberOfChars; i++)
                {
                    random += characters[RandomGenerator.Next(0, characters.Length)];
                }
            }

            return random;
        }
        private int GetRandomInt(int max)
        {
            int random = 0;
            lock (RandomGenerator)
            {
                random = RandomGenerator.Next(0, max);
            }

            return random;
        }
        #endregion
    }
}


