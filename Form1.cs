using System;
using System.Windows.Forms;

namespace Traffic_Booster
{
    public partial class Consola_Form : Form
    {
        public Consola_Form()
        {
            this.ControlBox = false;
            InitializeComponent();
            this.WriteLine("Successfully loaded Software");
            this.WriteLine("Created by loc-softlab all rights reserved");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void clearlist()
        {
            if (InvokeRequired)
            {
                listbox.Items.Clear();
                this.WriteLine("Successfully cleared software Consola");
            }
            else
            {
                listbox.Items.Clear();
                this.WriteLine("Successfully cleared software Consola");

            }
        }
        public void WriteLine(string Line)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    listbox.Items.Add(Line);
                }));
            }
            else
            {
                listbox.Items.Add(Line);
            }



        }

        private void Send_Click(object sender, EventArgs e)
        {
            listbox.Items.Add(textbox.Text);
        }

        private void Consola_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void Consola_Form_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Q)
            {
                this.Hide();
            }
        }

        private void Hide_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
