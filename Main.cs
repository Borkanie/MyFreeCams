using SharpUpdate;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Traffic_Booster.Guest_2._0;

namespace Traffic_Booster
{
    public partial class MainMFC : MetroFramework.Forms.MetroForm, ISharpUpdateable
    {
        //Constructorul interfata principala
        public MainMFC(string lic)
        {
            
            InitializeComponent();
            //seteaza interfata,calendar,toate ele
            jasmin_initialize();

            //face Update
            updater = new SharpUpdater(this);
            updater.DoUpdate();
            //in loc de show si il face sa apara si in taskbar si sa nu poata fii marit
            this.ShowHim();
            //cheie licenta
            license_dat = lic;
            //data la care s-a deschis
            startdate = DateTime.Now;
            //consola ca sa trimmitem mesaje in timp real
            Consola = new Consola_Form();
            Consola.Show();
            //timer pentru Schedule
            networktimer.Start();
        }
        #region initialize
        //o interfata care este capabila sa gaseasca coduriel pentru fwete folosind proxy(pt guesti Version 2)
        public CodeGeneratorForm codeGeneratorForm;
        // Updater
        private SharpUpdater updater;
        //licenta cheie
        private string license_dat;
        //connsola pentru mesaje
        private Consola_Form Consola;
        //cheie pentru recaptcha nu mai merge decand au trecut de la alt provider
        public string recaptchakey = "a";
        //interfata pentru setarea cheii de racptcha
        private Recaptcha_Key_Form recaptcha;
        public void ShowHim()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
        }
        //numarul de guesti on
        int nr_guestii = 0;
        //numarul de membrii on
        int nr_membrii = 0;
        //data la care se deschide
        DateTime startdate = new DateTime();
        //random global pentru selectarea proxy-urilor la guestii
        private static readonly Random getrandom = new Random();
        //useragenti
        public string[] useragents = { " Mozilla / 5.0(Windows NT 10.0; rv:65.0) Gecko/20100101 Firefox/65.0" };
        //o lista care o sa fie legata la ProfileListBox
        private string[] listProfile;
        //timp de asteptat pana la restart
        public int waittime = 0;
        //mesajele pe care sa le trimmita
        string[] lines;
        //lista de membrii vechi
        List<Members> Membrii = new List<Members>();
        //lista de guestii vechi
        List<Guest> Guests = new List<Guest>();
        //lsita de threaduri pe care o sa generam si o sa rulam botii
        public List<Thread> jasmin_threads = new List<Thread>();
        //public int jasmin_nr2 = 0;
        //public int numar_timer2 = 0;
        //timp de asteptat pana cand da drumul a boti automat
        int jasmin_time_until_next_scheduled_run_in_seconds = 0;
        //sa faca trecere prin toti membrii pe rand cand da refresh
        int firstmemeber = 0;
        public generateSocketCodes generator;

        public void jasmin_initialize()
        {
            jasmin_datetimepicker.Format = DateTimePickerFormat.Custom;
            jasmin_datetimepicker.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            jasmin_Mode.SelectedItem = "Manual";
            jasmin_Schedule_Visit.Hide();
            jasmin_Progress.Hide();
            jasmin_Picture.Hide();
            jasmin_datetimepicker.Hide();
            Chk_MemberV2.Hide();
            this.label1.Text = this.ApplicationAssembly.GetName().Version.ToString();
        }

        #endregion
        #region Interface
        //event button de selectat mesaje pentru membrii
        private void Jasmin_Member_Proxy_Select_Click(object sender, EventArgs e)
        {
            OpenFileDialog mkd = new OpenFileDialog();
            mkd.Filter = "TXT files|*.txt";
            mkd.Title = "ProxyList";
            var result = mkd.ShowDialog();
            if (result == DialogResult.OK)
            {
                lines = File.ReadAllLines(mkd.FileName);
                MsgNumberCountLabel.Text = lines.Length.ToString();
            }
            mkd.Dispose();

        }
        //event buton pentru alegere profile
        private void Jasmin_ChooseFile_Click(object sender, EventArgs e)
        {

            int Count = 0;
            OpenFileDialog file1 = new OpenFileDialog();
            file1.Filter = "TXT files|*.txt";
            file1.Title = "UserList";
            var result = file1.ShowDialog();

            if (result == DialogResult.OK)
            {
                jasmin_Profile_ListBox.Items.Clear();
                foreach (var row in System.IO.File.ReadLines(file1.FileName))
                {
                    if (row.Contains("/"))
                        jasmin_Profile_ListBox.Items.Add(row);


                    Count++;
                }
                metroLabel4.Text = Count.ToString();

            }
        }
        //alegi daca sa fie schedule sau nu
        private void Jasmin_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = jasmin_Mode.SelectedItem.ToString();
            if (item == "Schedule")
            {
                jasmin_Schedule_Visit.Visible = true;
                jasmin_datetimepicker.Visible = true;


            }
            else
            {

                jasmin_Schedule_Visit.Visible = false;
                jasmin_datetimepicker.Visible = false;
            }
        }
        //alegem txt cu proxi pt guesti
        private void Jasmin_Guest_Proxy_Select_Click(object sender, EventArgs e)
        {
            int Count2 = 0;
            OpenFileDialog mkd = new OpenFileDialog();
            mkd.Filter = "TXT files|*.txt";
            mkd.Title = "ProxyList";
            var result = mkd.ShowDialog();

            if (result == DialogResult.OK)
            {
                jasmin_Guest_Proxy_ListBox.Items.Clear();
                foreach (var row in System.IO.File.ReadLines(mkd.FileName))
                {
                    if (row.Contains(":"))
                        jasmin_Guest_Proxy_ListBox.Items.Add(row);
                    Count2++;
                }
                jasmin_Guest_Proxy_Label.Text = Count2.ToString();

            }
            mkd.Dispose();
        }
        //alegem txt cu membrii si proxi pt membrii
        private void Jasmin_Select_Member_Click(object sender, EventArgs e)
        {
            int Countm = 0;
            OpenFileDialog mkd = new OpenFileDialog();
            mkd.Filter = "TXT files|*.txt";
            mkd.Title = "ProxyList";
            var result = mkd.ShowDialog();

            if (result == DialogResult.OK)
            {
                jasmin_Member_ListBox.Items.Clear();
                foreach (var row in System.IO.File.ReadLines(mkd.FileName))
                {
                    if (row.Contains(":"))
                        jasmin_Member_ListBox.Items.Add(row);
                    Countm++;
                }
                jasmin_Total_Member_Label.Text = Countm.ToString();

            }
            mkd.Dispose();
        }
        #endregion
        #region Start
        //pregatim interfata si testam inputurile inainte de a da run
        private void jasmin_Start_Click(object sender, EventArgs e)
        {
            int nprim = 0;

            nprim = Convert.ToInt32(jasmin_Guest_Visit_TextBox.Text) * 10 + Convert.ToInt32(jasmin_Member_Visit_TextBox.Text) * 25;
            if (Convert.ToInt32(jasmin_Visit_Time.Text) < nprim && Convert.ToInt32(jasmin_Visit_Time.Text) != 0)
                MessageBox.Show("min vizit time=" + nprim.ToString());
            else
            if (Convert.ToInt32(jasmin_Member_Visit_TextBox.Text) > jasmin_Member_ListBox.Items.Count || Convert.ToInt32(jasmin_Guest_Visit_TextBox.Text) > jasmin_Guest_Proxy_ListBox.Items.Count)
                MessageBox.Show("Prea multi vizitatori");
            else
            {
                Consola.clearlist();
                jasmin_Progress.Show();
                jasmin_Picture.Show();
                jasmin_Picture.BringToFront();
                jasmin_Progress.BringToFront();
                if (jasmin_Mode.SelectedItem.ToString() == "Schedule")
                {

                    jasmin_time_until_next_scheduled_run_in_seconds = Convert.ToInt32((jasmin_datetimepicker.Value - DateTime.Now).TotalSeconds);
                    jasmin_timer1.Start();
                    jasmin_ChooseFile.Enabled = false;
                    jasmin_Guest_Proxy_Select.Enabled = false;
                    jasmin_Member_Proxy_Select.Enabled = false;
                    jasmin_Select_Member.Enabled = false;
                    jasmin_Start.Enabled = false;
                    jasmin_Mode.Enabled = false;
                }
                else
                {
                    if (jasmin_Member_Visit_TextBox.Text != "0")
                        online_member_nr.BackColor = Color.LightGreen;
                    if (jasmin_Guest_Visit_TextBox.Text != "0")
                        online_guest_nr.BackColor = Color.LightGreen;
                    listProfile = new string[jasmin_Profile_ListBox.Items.Count];
                    firstmemeber = Convert.ToInt32(jasmin_Member_Visit_TextBox.Text);
                    for (int j = 0; j < jasmin_Profile_ListBox.Items.Count; j++)
                        listProfile[j] = jasmin_Profile_ListBox.Items[j].ToString().Split('/')[0];
                    waittime = Convert.ToInt32(jasmin_off_time_textbox.Text);
                    jasmin_time_until_next_scheduled_run_in_seconds = Convert.ToInt32(jasmin_Visit_Time.Text);
                    Thread t1 = new Thread(() => add_functions.jasmin_Launch(Consola, this));
                    Consola.WriteLine("[" + DateTime.Now + "] " + "Started the launch_Thread");
                    jasmin_threads.Add(t1);
                    t1.Start();


                };
            };
        }
        //interfata de incarcare practic splashscreen
        public void jasmin_hide()
        {

            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    jasmin_Picture.Hide();
                    jasmin_Progress.Hide();
                }));
            }
            else
            {
                jasmin_Progress.Hide();
                jasmin_Picture.Hide();
            }
        }
        //deschidem timer pt refresh
        public void jasmin_timer_start()
        {
            Consola.WriteLine("[" + DateTime.Now + "] " + "Started timer2");
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    jasmin_timer2.Start();
                }));
            }
            else
            {
                jasmin_timer2.Start();
            }
        }
        //aici incrementam label care arata cati membrii is on
        public void jasmin_online_members()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    online_member_nr.Text = (Convert.ToInt32(online_member_nr.Text) + 1).ToString();
                }));
            }
            else
            {
                online_member_nr.Text = (Convert.ToInt32(online_member_nr.Text) + 1).ToString();
            }
        }
        //aici incrementam label care arata cati guesti is on
        public void jasmin_online_guests()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    online_guest_nr.Text = (Convert.ToInt32(online_guest_nr.Text) + 1).ToString();
                }));
            }
            else
            {
                online_guest_nr.Text = (Convert.ToInt32(online_guest_nr.Text) + 1).ToString();
            }
        }
        #endregion
        #region running
        //generam numar sincron pe toate thredurile
        public int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
        //vedem cand trebuie sa dams tart la scheduled run
        private void Jasmin_timer2_Tick(object sender, EventArgs e)
        {
            jasmin_time_until_next_scheduled_run_in_seconds--;
            if (jasmin_time_until_next_scheduled_run_in_seconds <= 0)
            {
                timer3.Stop();
                jasmin_timer2.Stop();
                Thread t2 = new Thread(() => Refresher());
                jasmin_threads.Add(t2);
                t2.Start();
                jasmin_time_until_next_scheduled_run_in_seconds = Convert.ToInt32(jasmin_Visit_Time.Text) + Convert.ToInt32(jasmin_off_time_textbox.Text) + 10;
                jasmin_timer_start();
            }
        }
        //vedem cand trebe sa dam restart la membrii
        private void Jasmin_timer1_Tick(object sender, EventArgs e)
        {
            jasmin_time_until_next_scheduled_run_in_seconds--;
            if (jasmin_time_until_next_scheduled_run_in_seconds <= 0)
            {
                Consola.WriteLine("[" + DateTime.Now + "] " + "Stopping Program for restart");
                jasmin_timer1.Stop();
                jasmin_time_until_next_scheduled_run_in_seconds = 0;
                jasmin_ChooseFile.Enabled = true;
                jasmin_Guest_Proxy_Select.Enabled = true;
                jasmin_Member_Proxy_Select.Enabled = true;
                jasmin_Select_Member.Enabled = true;
                jasmin_Start.Enabled = true;
                jasmin_Mode.Enabled = true;
                jasmin_Mode.SelectedItem = "Manual";
                jasmin_Start_Click(sender, e);
            }
        }
        //sa nu primeasca decat digiti
        private void Visit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        #endregion
        #region Stop
        //curatam procesele latente cu cmd
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.Start("cmd.exe", "/C taskKill /im chromedriver.exe /f /q /h");
            Process.Start("cmd.exe", "/C taskKill /im chrome.exe /f /q /h");
            Process.Start("cmd.exe", "/C taskKill /im MyFreeCams_Traffic_Booster.exe /f /q /h");
            Application.Exit();
        }
        //curatam procesele latente cu cmd sis alvam in savefile
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //salvam in savefile
            ActivateClass.datas_to_api(AppDomain.CurrentDomain.BaseDirectory, nr_membrii, nr_guestii, startdate, license_dat);
            //kill all processes!
            MfcGuest.Random = null;
            MfcGuest.Threads = null;
            MfcGuest.servers = null;
            Process.Start("cmd.exe", "/C taskKill /im chromedriver.exe /f /q /h");
            Process.Start("cmd.exe", "/C taskKill /im chrome.exe /f /q /h");
           
        }
        //aici ne oocupam de interfata cand dam refresh la membrii
        private void Invoker(bool refresh_begin)
        {
            if (refresh_begin)
            {
                Consola.WriteLine("[" + DateTime.Now + "] " + "intarface for refresh");
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        jasmin_Picture.Show();
                    }));
                }
                else
                {
                    jasmin_Picture.Show();
                }
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        jasmin_Picture.BringToFront();
                    }));
                }
                else
                {
                    jasmin_Picture.BringToFront();
                }
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        jasmin_Progress.BringToFront();
                    }));
                }
                else
                {
                    jasmin_Progress.BringToFront();
                }
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        jasmin_Progress.Show();
                    }));
                }
                else
                {
                    jasmin_Progress.Show();
                }
            }
            else
                jasmin_hide();

        }
        //aici este functia de refresh
        private void Refresher()
        {
            Invoker(true);
            Consola.WriteLine("[" + DateTime.Now + "] " + "Starting Refresher");
            for (int i = 0; i < Membrii.Count; i++)
                Membrii[i].GoHome();
            Thread.Sleep(waittime * 1000);
            int n = Membrii.Count;
            bool Overflow = false;
            for (int i = firstmemeber; i < firstmemeber + Convert.ToInt32(jasmin_Member_Visit_TextBox.Text); i++)
                if (i < Membrii.Count)
                {
                    Membrii[i].Navigate(listProfile);
                    Thread t1 = new Thread(() => { Membrii[i].Write_on_chat(this.GetLines()); });
                    jasmin_threads.Add(t1);
                    t1.Start();
                    Thread.Sleep(2000);
                }
                else
                {
                    Membrii[i - Membrii.Count].Navigate(listProfile);
                    Thread t1 = new Thread(() => { Membrii[i - Membrii.Count].Write_on_chat(this.GetLines()); });
                    jasmin_threads.Add(t1);
                    t1.Start();
                    Thread.Sleep(2000);
                    Overflow = true;
                }
            if (!Overflow)
                firstmemeber = firstmemeber + Convert.ToInt32(jasmin_Member_Visit_TextBox.Text);
            else
                firstmemeber = firstmemeber + Convert.ToInt32(jasmin_Member_Visit_TextBox.Text) - Membrii.Count;
            Consola.WriteLine("[" + DateTime.Now + "] " + "Done Refresher");
            jasmin_hide();

        }
        //functia de inchidere
        private void Jasmin_Stop_Click(object sender, EventArgs e)//functia de inchidere
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you really want to exit?", "Message Box", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    Consola.WriteLine("[" + DateTime.Now + "] " + "Stopping Program");
                    if(this.generator != null)
                    {
                        Consola.WriteLine("[" + DateTime.Now + "] " + "Stopping GuestV2");
                        generator.Stop();
                        Consola.WriteLine("[" + DateTime.Now + "] " + "Done with GuestV2");
                    }
                    nr_membrii = nr_membrii + Convert.ToInt32(online_member_nr.Text);
                    nr_guestii = nr_guestii + Convert.ToInt32(online_guest_nr.Text);
                    for (int i = 0; i < jasmin_threads.Count; i++)
                    {
                        try
                        {
                            Thread t = jasmin_threads[i];
                            jasmin_threads.RemoveAt(i);
                            i--;
                            t.Abort();
                        
                        }
                        catch (Exception ex)
                        {
                            
                            Consola.WriteLine("[" + DateTime.Now + "] " + ex.Message);
                        };
                    }
                    timer3.Stop();
                    jasmin_timer1.Stop();
                    jasmin_timer2.Stop();
                    online_member_nr.Text = "0";
                    online_guest_nr.Text = "0";
                    if (jasmin_Member_Visit_TextBox.Text != "0")
                        online_member_nr.BackColor = Color.White;
                    if (jasmin_Guest_Visit_TextBox.Text != "0")
                        online_guest_nr.BackColor = Color.White;
                    Process.Start("cmd.exe", "/C taskKill /im chromedriver.exe /f");
                    Process.Start("cmd.exe", "/C taskKill /im chrome.exe /f");
                    Process.Start("cmd.exe", "/C taskKill /im geckodriver.exe /f");
                    Process.Start("cmd.exe", "/C taskKill /im conhost.exe /f");
                }

                if (jasmin_Mode.SelectedItem.ToString() == "Schedule")
                {

                    jasmin_Mode.SelectedItem = "Manual";
                    jasmin_Member_Proxy_Select.Enabled = true;
                    jasmin_Mode.Enabled = true;
                    jasmin_Guest_Proxy_Select.Enabled = true;
                    jasmin_Start.Enabled = true;
                    jasmin_Select_Member.Enabled = true;
                    jasmin_ChooseFile.Enabled = true;
                }
                else
                {

                    jasmin_hide();



                    string tempfolder = Path.GetTempPath(); ;
                    string[] tempfiles = Directory.GetDirectories(tempfolder, "scoped_dir*", SearchOption.AllDirectories);
                    string[] tempfiles2 = Directory.GetDirectories(tempfolder, "Report.*", SearchOption.AllDirectories);
                    foreach (string tempfile in tempfiles)
                    {
                        try
                        {
                            Directory.Delete(tempfile, true);

                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                            Consola.WriteLine("[" + DateTime.Now + "] " + s);
                        };
                    }
                    foreach (string tempfile in tempfiles2)
                    {
                        try
                        {
                            Directory.Delete(tempfile, true);

                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                            Consola.WriteLine("[" + DateTime.Now + "] " + s);
                        };
                    }

                }


            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Consola.WriteLine("[" + DateTime.Now + "] " + s);
            };

        }

        #endregion
        #region SharpUpdate
        
        public string ApplicationName
        {
            get { return "MyFreeCams_Traffic_Booster"; }
        }
        public string ApplicationId
        {
            get { return "MyFreeCams_Traffic_Booster"; }
        }
        public Assembly ApplicationAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }

        public Icon ApplicationIcon
        {
            get { return this.Icon; }
        }


        public Uri UpdateXmlLocation
        {
            get { return new Uri("http://lic.soft-lab.ro/Updates/MFC/updateMFC.xml"); }
        }

        public Form Context
        {
            get { return this.FindForm(); }
        }
        #endregion
        #region Events
        private void BtnGeneratorCod_Click(object sender, EventArgs e)
        {
            if (this.codeGeneratorForm == null)
            {
                this.codeGeneratorForm = new CodeGeneratorForm(Consola, this);
                this.codeGeneratorForm.Show();
            }
            else
                this.codeGeneratorForm.Show();
        }
        private void MainMFC_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Q)
            {
                if (Consola != null)
                    Consola.Show();
                
            }
        }
        private void Captcha_checkbox_Click(object sender, EventArgs e)
        {
            if (captcha_checkbox.Checked == true && recaptcha == null)
            {
                recaptcha = new Recaptcha_Key_Form(this);
                recaptcha.Show();
            }
        }
        #endregion
        #region Get/SetMethods
        public bool GetMemberV2Checkbox()
        {
            return Chk_MemberV2.Checked;
        }
        public bool GetMemberVisibleCheck()
        {
            return chkboxMode.Checked;
        }
        public bool GetReferer()
        {
            return chkReferer.Checked;
        }
        public ListBox.ObjectCollection GetMemberListBox()
        {
            return jasmin_Member_ListBox.Items;
        }
        public string[] GetUserAgents()
        {
            return useragents;
        }
        public string[] GetLines()
        {
            return lines;
        }
        public void AddMemberToList(Members target)
        {
            Membrii.Add(target);
        }
        public int GetMemberCount()
        {
            return Membrii.Count;
        }
        public string GetMemberTextBox()
        {
            return jasmin_Member_Visit_TextBox.Text;
        }
        public void IncreaseFirstMember()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    firstmemeber++;
                }));
            }
            else
            {
                firstmemeber++;
            }
        }
        public void RemoveThread(Thread target)
        {
            jasmin_threads.Remove(target);
        }
        public int GetGuetsListBoxCount()
        {
            return jasmin_Guest_Proxy_ListBox.Items.Count;
        }
        public ListBox.ObjectCollection GetGuetsProxyListBox()
        {
            return jasmin_Guest_Proxy_ListBox.Items;
        }
        public bool GetGuestWebSocketCheckBox()
        {
            return this.guestV2.Checked;
        }
        public bool GetGuestVisible()
        {
            return checkbox_visible_guest.Checked;
        }
        public void AddGuestToList(Guest target)
        {
            Guests.Add(target);
        }
        public string[] GetProfiles()
        {
            return listProfile;
        }
        public void AddThreadToList(Thread target)
        {
            jasmin_threads.Add(target);
        }
        public string GetGuestTextBox()
        {
            return jasmin_Guest_Visit_TextBox.Text;
        }
        public int GetONlineGuests()
        {
            return Convert.ToInt32(online_guest_nr.Text);
        }
        public int GetGuestsCount()
        {
            return Guests.Count;
        }
        public int GetJasminNr()
        {
            return jasmin_time_until_next_scheduled_run_in_seconds;
        }
        #endregion

       
    }
}

