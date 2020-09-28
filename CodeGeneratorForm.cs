using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Traffic_Booster.Guest_2._0;

namespace Traffic_Booster
{
    public partial class CodeGeneratorForm : Form
    {
        private List<Model> Models = new List<Model>();
        private MainMFC Main;
        private Consola_Form Consola;
        public CodeGeneratorForm(Consola_Form consola,MainMFC main)
        {
            Consola = consola;
            Main = main;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file1 = new OpenFileDialog();
            file1.Filter = "TXT files|*.txt";
            file1.Title = "UserList";
            var result = file1.ShowDialog();
            int numar = 0;
            if (result == DialogResult.OK)
            {
                LstbModel.Items.Clear();
                foreach (var row in System.IO.File.ReadLines(file1.FileName))
                {
                    if (row.Contains("/") && !row.Contains(":"))
                    {
                        LstbModel.Items.Add(row);
                        Models.Add(new Model(row.Split('/')[0], numar++));
                    }
                       
                }
               

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                Consola.WriteLine("getting girl codes");
                var cService = ChromeDriverService.CreateDefaultService();
                cService.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
                options.AddArgument("--no-sandbox");
                //options.AddArgument("--headless");
                if(TxtProxy.Text!="")
                {
                    options.AddArgument("--proxy-server=" + TxtProxy.Text);
                }
                options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-web-security");
                options.AddArgument("--disable-crash-reporter");
                options.AddArgument("--disable-default-apps");
                options.AddArgument("--disable-extensions");
                options.AddArgument("--disable-logging");
                options.AddExcludedArguments(new List<string>() { "enable-automation" });
                options.AddArgument("--disable-sync");
                options.AddArgument("--user-agent=" + userAgent);
                ChromeDriver chrome = new ChromeDriver(cService, options, TimeSpan.FromSeconds(3600));
                string url = "https://www.myfreecams.com/#Homepage";
                chrome.Navigate().GoToUrl(url);
                Thread.Sleep(5000);
                chrome.FindElementByXPath("//*[@id=\"enter_desktop\"]").Click();
                foreach (var model in Models)
                {
                    try
                    {
                        chrome.FindElementByXPath("//*[@id=\"online_model_search\"]").SendKeys(model.Name);
                        chrome.FindElementByXPath("//*[@id=\"online_model_search\"]").SendKeys(OpenQA.Selenium.Keys.Enter);
                        Thread.Sleep(1000);

                        var element = chrome.FindElementByXPath("/html/body/div[5]/table/tbody/tr[4]/td/table/tbody/tr[2]/td/table/tbody/tr[1]/td/div/div[2]/div/table/tbody/tr/td/div/table/tbody/tr[2]/td/div/div/table/tbody/tr[1]/td/div[2]/div[2]/div/div/div/div[2]/img");
                        model.Code = Convert.ToInt32(element.GetAttribute("src").Split(new string[] { "mfc_" }, StringSplitOptions.None)[1].Split(new string[] { "?no-" }, StringSplitOptions.None)[0]);
                        Thread.Sleep(1000);
                        IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
                        string title = (string)js.ExecuteScript("document.getElementById(\"online_model_search\").value='';");
                        Thread.Sleep(1000);
                        Consola.WriteLine("Done for:" + model.Name);
                    }
                    catch (Exception ex)
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
                        string title = (string)js.ExecuteScript("document.getElementById(\"online_model_search\").value='';");
                        Thread.Sleep(1000);
                        Consola.WriteLine("Error for:" + model.Name);
                    }
                }
                chrome.Close();
                Consola.WriteLine("Done with codes");
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\ModelsWithCodes.txt";
                if (File.Exists(path))
                {
                    // If file found, delete it    
                    File.Delete(path);

                }
                if(!File.Exists(path))
                {
                    var file=File.Create(path);
                    file.Close();
                }
                //using (System.IO.FileStream fs = System.IO.File.Create(System.Reflection.Assembly.GetEntryAssembly().Location + "\\ModelsWithCodes.txt")) { }

                using (System.IO.StreamWriter file =new System.IO.StreamWriter(path, true))
                {
                    foreach (var model in Models)
                        if(model.Code!=0)
                            file.WriteLine(model.Name + ":" + model.Code + "/");
                }
                Main.codeGeneratorForm = null;
                MessageBox.Show("Got the codes they ar in the same folder as the executable under the name \"ModelsWithCodes.txt\"");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error when getting codes:" + ex.Message,string.Empty,MessageBoxButtons.OK);

            }
        }

        private void CodeGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.codeGeneratorForm = null;
        }
    }
}
