using System;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Traffic_Booster
{
    class ActivateClass
    {

        public static string IsActivated(string Key)//checks if key is activated
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.Expect100Continue = true;
            WebRequest wrGETURL;

            string urld = "https://lic.soft-lab.ro/API/Api/?key=" + Key + "&PID=" + GetCpuId() + "&ip=" + GetLocalIPAddress() + "&mac=" + GetMotherBoardID() + "&graphics=" + GetGraphicsId();

            wrGETURL = WebRequest.Create(urld);
            wrGETURL.Method = "GET";
            wrGETURL.Proxy = WebProxy.GetDefaultProxy();

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string sLine = objReader.ReadToEnd();
            if (sLine.Contains("Congrats"))
            {

                return "ACTIVATED";
            }

            if (sLine.Contains("already"))
            {

                return "SECOND-ACTIVATION";
            }
            if (sLine == " ")
            {
                return "EXPIRED";
            }
            return "ERROR";
        }

        static public void datas_to_api(string path, int nr_guestii, int nr_membrii, DateTime startdate, string license_dat)
        {

            DateTime stoptime = DateTime.Now;
            if (ActivateClass.send_user_data(nr_membrii, nr_guestii, "a", startdate, stoptime, license_dat) == true)
            {
                if (File.Exists(path + "\\savedata.dat"))
                    File.Delete(path + "savedata.dat");
                return;
            }
            else
                save_data(path + "@" + nr_guestii.ToString() + "@" + nr_membrii.ToString() + "@" + startdate.ToString() + "@" + stoptime.ToString() + "@" + license_dat, path);

        }

        static private void save_data(string value, string path)
        {
            try
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(value);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("bow down for Cthulhu"));//complet aleatoriu ales
                    using (TripleDESCryptoServiceProvider tripDes =
                        new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        if (File.Exists(path + "\\savedata.dat"))
                            File.Delete(path + "savedata.dat");
                        FileStream fsCrypt = new FileStream(path + "savedata.dat", FileMode.Create);
                        fsCrypt.Close();
                        File.WriteAllBytes(path + "//savedata.dat", results);

                    }
                }
            }
            catch (Exception ex)
            { string s = ex.Message; }


        }
        public static bool send_user_data_after_restart(string details, string license_data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest wrGETURL;
                string urld = "https://lic.soft-lab.ro/API/User_activity/?traffic=" + "?" + "&url=" + "?" + "&time=" + "?" + "&time_on="
                         + "?" + "&time_off=" + "?" + "&full_detail=" + "The software was unable to send information last time:" + details + "Nr membrii:" + "?"
                         + " Nr guestii:" + "?" + "&licenseID=" + license_data.Split('@')[0] + "&token=" + license_data.Split('@')[2];
                string[] urlds = urld.Split(' ');
                urld = "";
                foreach (string str in urlds)
                    urld = urld + str;
                wrGETURL = WebRequest.Create(urld);
                wrGETURL.Method = "GET";
                wrGETURL.Proxy = WebProxy.GetDefaultProxy();//pana aici este webrequest-ul
                Stream objStream;//cu asta luam raspunsul
                objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                //aici salvam tot textul din raspouns
                string sLine = objReader.ReadToEnd();
                //de aici vedem ce raaspuns avem
                if (sLine.Contains("succes"))
                    return true;

            }
            catch { }
            return false;

        }
        public static bool send_user_data(int nr_guestii, int nr_membrii, string details, DateTime ON, DateTime OFF, string license_data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest wrGETURL;
                string urld = "https://lic.soft-lab.ro/API/User_activity/?traffic=" + "?" + "&url=" + "?" + "&time=" + Convert.ToString(ON - OFF) + "&time_on="
                        + ON.ToString() + "&time_off=" + OFF.ToString() + "&full_detail=" + details + "Nr membrii:" + nr_membrii.ToString()
                        + " Nr guestii:" + nr_guestii.ToString() + "&licenseID=" + license_data.Split('@')[0] + "&token=" + license_data.Split('@')[2];
                string[] urlds = urld.Split(' ');
                urld = "";
                foreach (string str in urlds)
                    urld = urld + str;
                //string urld = "https://lic.soft-lab.ro/API/User_activity123/?traffic=";
                wrGETURL = WebRequest.Create(urld);
                wrGETURL.Method = "GET";
                wrGETURL.Proxy = WebProxy.GetDefaultProxy();//pana aici este webrequest-ul
                Stream objStream;//cu asta luam raspunsul
                objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                //aici salvam tot textul din raspouns
                string sLine = objReader.ReadToEnd();
                //de aici vedem ce raaspuns avem
                if (sLine.Contains("succes"))
                    return true;

            }
            catch { }
            return false;

        }


        #region hardwaredata
        private string GetMacAddress()
        {
            try
            {
                string macAddresses = string.Empty;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }

                return macAddresses;
            }
            catch
            {
                return "VPS";
            }
        }
        public static string GetCpuId()
        {
            try
            {
                string cpuInfo = "ERROR";
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec)
                {
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
                return cpuInfo;
            }
            catch
            {
                return "VPS";
            }
        }
        public static string GetMotherBoardID()
        {
            try
            {
                string mbInfo = String.Empty;
                ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
                scope.Connect();
                ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

                foreach (PropertyData propData in wmiClass.Properties)
                {
                    if (propData.Name == "SerialNumber")
                        mbInfo = String.Format("{0}", Convert.ToString(propData.Value));
                }

                return mbInfo;
            }
            catch
            {
                return "VPS";
            }
        }
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                throw new Exception("No network adapters with an IPv4 address in the system!");

            }
            catch
            {
                return "VPS";
            }
        }
        public static string GetGraphicsId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

                string graphicsCard = string.Empty;
                foreach (ManagementObject mo in searcher.Get())
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Name == "Description")
                        {
                            graphicsCard = property.Value.ToString();
                        }
                    }
                }
                return graphicsCard;
            }
            catch
            {
                return "VPS";
            }
        }
        #endregion

    }
}
