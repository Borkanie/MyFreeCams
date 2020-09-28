using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Traffic_Booster.Guest_2._0;
using Traffic_Booster.Member_2._0;

namespace Traffic_Booster.Member_2._0
{
    class MemberSocket
    {
        private WebSharerProxy Proxy;
        public static Random RandomGenerator;
        private Model Model;
        private WebSocketSharp.WebSocket Client;
        private Message LoginMessage = new Message() { c = 1, t = 0, f = 0, a1 = 20071025, a2 = 0, Data = "1/guest:guest" };
        private Message PingMessage = new Message() { c = 0, t = 0, f = 0, a1 = 0, a2 = 0, Data = string.Empty };
        private Message GirlMessage = new Message() { c = 10, t = 0, f = 0, a2 = 0 };
        private Consola_Form Consola;
        private Thread PingThread;
        public bool hasACcount = false;
        private bool ON = false;
        private MemberData Member;
        public MemberSocket(string server, Random random, WebSharerProxy proxy, Model model, Consola_Form consola,MemberData member)
        {
            Member = member;
            Consola = consola;
            ON = true;
            Proxy =proxy;
            Model = model;

            RandomGenerator = random;
            string webSocketBaseUrl = "wss://" + server + ".myfreecams.com/fcsl";
            //webSocketBaseUrl = "wss://xchat83.myfreecams.com/fcsl";
            var url = new Uri(webSocketBaseUrl);

            Client = new WebSocketSharp.WebSocket(webSocketBaseUrl);


            Client.OnOpen += new EventHandler(OnSocketOpened);
            Client.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(OnSocketError);
            Client.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(OnSocketClosed);
            Client.OnMessage += new EventHandler<WebSocketSharp.MessageEventArgs>(OnSocketMessageReceived);
            Client.SetProxy(this.Proxy.GetLink(), Proxy.Username, Proxy.Password);

            Client.Connect();

            string word = "fcsws_20180422\n\0";
            Consola.WriteLine(Member.Username.ToString() + " Connecting to socket");
            Client.Send(word);
            Thread.Sleep(3000);

            LoginMessage.Data = "1/" + Member.Username + ":"+member.AuthentificationCode;
            Consola.WriteLine(Member.Username.ToString() + " Logging in as guest");
            Client.Send(LoginMessage.ToString());
            Thread.Sleep(3000);
            while (GirlMessage.t == 0)
            {
                Thread.Sleep(1000);
            }
            Consola.WriteLine(Member.Username.ToString() + " Connecting to girl");
            Client.Send(GirlMessage.ToString());
            Thread.Sleep(3000);
            Consola.WriteLine(Member.Username.ToString() + " Message 1");
            Client.Send(new Message() { c = 44, t = GirlMessage.t, f = 0, a1 = 1, a2 = 0, Data = string.Empty }.ToString());

            Consola.WriteLine(Member.Username.ToString() + " Message 2");
            Client.Send(new Message() { c = 51, t = GirlMessage.t, f = 0, a1 = model.Code, a2 = 9, Data = string.Empty }.ToString());


        }

        internal void Close()
        {
            ON = false;
            if (PingThread != null)
                if (PingThread.IsAlive)
                    PingThread.Abort();
        }
        public void SetPingThread(Thread ping)
        {
            PingThread = ping;
        }
        public void Ping()
        {

            try
            {
                while (ON)
                {

                    //Consola.WriteLine("Bobo:" + word);
                    Consola.WriteLine(Member.Username.ToString() + " PING   " + Model.Name);
                    Client.Send(PingMessage.ToString());
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Member.Username.ToString() + " ERROR " + Model.Name);
                Client.Close();
            }

            Consola.WriteLine(Member.Username.ToString() + " closed " + Model.Name);
        }
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
        private void OnSocketOpened(object sender, EventArgs e)
        {
            Consola.WriteLine(Member.Username.ToString() + " Socket opned");
        }

        private void OnSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Consola.WriteLine(Member.Username.ToString() + "Socket ERROR:" + e.Message);
            Consola.WriteLine(Member.Username.ToString() + "Exception:" + e.Exception);
        }

        private void OnSocketClosed(object sender, WebSocketSharp.CloseEventArgs e)
        {
            Consola.WriteLine(Member.Username.ToString() + "Socket Closing with code" + e.Code);
            Consola.WriteLine(Member.Username.ToString() + e.Reason);
        }

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
            //if (nrmesaje<1)
            //{
            //    Consola.WriteLine(Number.ToString() + "Message:" + e.Data);
            //    nrmesaje++;

            //}
        }

    }
}

