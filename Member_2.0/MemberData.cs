using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Traffic_Booster.Guest_2._0;

namespace Traffic_Booster.Member_2._0
{
    class MemberData
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string AuthentificationCode { get; set; }
        public MemberData(string proxy)
        {
            string[] data = proxy.Split('+');
            Username = data[0];
            Password = data[1];
        }
        public void getAuthentificationCode()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.myfreecams.com/php/login.php");

            var postData = "submit_login: 9 \n uid=24211047168145 \n tz: 3 \n ss: 1719x725 \n useername: " + Username + "\n password: " + Password;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
       
    }
}
