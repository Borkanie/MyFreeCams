using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Booster.Guest_2._0
{
    //proxy care respecta structura Webshare
    public class WebSharerProxy
    {
        public string Url { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public WebSharerProxy(string proxy)
        {
            string[] prx = proxy.Split(':');
            if (prx.Length == 4)
            {
                Url = prx[0];
                Port = prx[1];
                Username = prx[2];
                Password = prx[3];
            }
            else
            {
                Url = prx[0];
                Port = prx[1];
                Username = string.Empty;
                Password = string.Empty;
            }
        }
        public string GetLink()
        {
            return "http://" + Url + ":" + Port;
        }
    }
}
