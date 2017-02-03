using System;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            onLoad();
        }

        private void onLoad()
        {
            this.textBox_login.Text = Properties.Settings.Default.login;
            this.textBox_pass.Text = Properties.Settings.Default.pass;
            this.checkBox_SavePass.Checked = Properties.Settings.Default.savePass;

            if (Properties.Settings.Default.autoUpdate == true)
                this.checkBox1.Checked = true;
            else
                this.checkBox1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.login = this.textBox_login.Text;
            Properties.Settings.Default.pass = this.textBox_pass.Text;
            Properties.Settings.Default.autoUpdate = this.checkBox1.Checked;
            Properties.Settings.Default.savePass = this.checkBox_SavePass.Checked;

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            
            //Form1 f = new Form1();
            //f.Show();
            this.Close();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
