using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace Traffic_Booster
{
    public partial class Activation : Form
    {
        public Activation()
        {
            InitializeComponent();
            progressBar1.Value = 0;
        }
        private string Decrypt_license()
        {
            try
            {
                byte[] data = File.ReadAllBytes(Application.StartupPath + "//license.dat");// decrypt the incrypted text
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("i am mister meesekes"));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return UTF8Encoding.UTF8.GetString(results);
                    }
                }
            }
            catch { MessageBox.Show("Something went wrong please contact the manufacturer"); return "ERROR"; }
        }
        private string Decript_savedata()
        {
            byte[] data = File.ReadAllBytes(Application.StartupPath + "//savedata.dat");// decrypt the incrypted text
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("bow down for Cthulhu"));//stillrandom but must be the same
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }
        private void Jasmin_Traffic_Booster_Shown(object sender, System.EventArgs e)
        {
            progressBar1.Value += 25;
            string input = System.Reflection.Assembly.GetExecutingAssembly().CodeBase + "license.dat";
            Thread.Sleep(500);
            progressBar1.Value += 25;
            string output = System.Reflection.Assembly.GetExecutingAssembly().CodeBase + "temp.dat";
            Thread.Sleep(500);
            progressBar1.Value += 25;
            try
            {
                string license_data = Decrypt_license();
                string Key = license_data.Split('@')[1];
                Thread.Sleep(500);
                progressBar1.Value += 25;
                string key_status = ActivateClass.IsActivated(Key);
                MainMFC MFC;
                switch (key_status)
                {
                    case "SECOND-ACTIVATION":

                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\savedata.dat"))
                        {

                            ActivateClass.send_user_data_after_restart(Decrypt_license(), Decript_savedata());
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\savedata.dat");

                        }
                        MFC = new MainMFC(license_data);
                        this.Hide();
                        MFC.Show();


                        break;
                    case "EXPIRED":
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat"))
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat");
                        File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat").Close();
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat", "Yo homie what u tryna do here nigga go to your nice place and paly a horse and some foxes bro");
                        MessageBox.Show("License is expired please contact lic-softlab");
                        break;
                    case "ACTIVATED":
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat"))
                        {
                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\savedata.dat"))
                            {

                                ActivateClass.send_user_data_after_restart(Decrypt_license(), Decript_savedata());
                                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\savedata.dat");

                            }
                            MFC = new MainMFC(license_data);
                            this.Hide();
                            MFC.Show();
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\userlicdata.dat");
                        }
                        else
                        {
                            MessageBox.Show("Not licensed Product please reinstall and activate it");
                            string oldkey = Decrypt_license();
                            string newkey = string.Empty;
                            foreach (char key_char in oldkey)
                            {
                                newkey = newkey + Convert.ToChar(key_char + 5);

                            }
                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "license.dat") == true)
                            {
                                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "license.dat");
                                File.Create(AppDomain.CurrentDomain.BaseDirectory + "license.dat").Close();
                                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "license.dat", newkey);

                            }
                            Thread.Sleep(1000);
                            this.Close();

                        }
                        break;
                    default:
                        MessageBox.Show("Not licensed Product please reinstall and activate it");
                        this.Close();
                        break;
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                MessageBox.Show("There was a problem with the license");
                this.Close();
            }
        }
    }
}
