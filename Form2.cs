using System;
using System.Windows.Forms;

namespace Traffic_Booster
{
    public partial class Recaptcha_Key_Form : Form
    {

        private MainMFC mfc;
        public Recaptcha_Key_Form(MainMFC mainMFC)
        {
            InitializeComponent();
            mfc = mainMFC;
        }
        public void Done_button_Click(object sender, EventArgs e)
        {
            mfc.recaptchakey = recaptcha_textbox.Text;

        }
        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
