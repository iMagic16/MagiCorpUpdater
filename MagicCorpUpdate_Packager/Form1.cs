using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MagicCorpUpdate_Packager
{
    public partial class FrmMUPackage : Form
    {
        public FrmMUPackage()
        {
            InitializeComponent();
        }

        string version = "101";

        public static string username, hostname, password;
        public static int port;

        private void FrmMUPackage_Load(object sender, EventArgs e)
        {
            //write runtimes
            if (!File.Exists("WinSCP.exe"))
                File.WriteAllBytes("WinSCP.exe", Properties.Resources.WinSCP);
            if (!File.Exists("WinSCPnet.dll"))
                File.WriteAllBytes("WinSCPnet.dll", Properties.Resources.WinSCPnet);

            //load settings
            LoadSettings();

            //set label to version
            LblVersion.Text = version;
        }
        private void FrmMUPackage_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }



        private void BtnVerifyLogin_Click(object sender, EventArgs e)
        {
            SaveSettings();

            GpPackager.Enabled = true;
            BtnSubmit.Enabled = true;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            BrowseDialog();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            SCP_Start();
        }


        private void SCP_Start()//string url, string username, string password, int port)
        {
            /*
            * scp -P yourport yourusername@yourserver:/home/yourusername/examplefile . <- localdir
            */

            Process.Start("klink.exe", "scp -P " + port + " " + username + "@" + hostname + ":/home/" + username + "/test.scp" + " .");


        }

        private void SaveSettings()
        {
            Properties.Settings.Default.hostname = TxtHostname.Text;
            Properties.Settings.Default.username = TxtUsername.Text;
            //       Properties.Settings.Default.password = TxtPasswd.Text;
            Properties.Settings.Default.port = 22;
            Properties.Settings.Default.Save();
            Console.WriteLine("Settings saved.");
        }


        private void LoadSettings()
        {
            TxtHostname.Text = Properties.Settings.Default.hostname;
            TxtUsername.Text = Properties.Settings.Default.username;
            //       password = Properties.Settings.Default.password;
            port = Properties.Settings.Default.port;
            Console.WriteLine("Settings loaded.");
        }

        private void BrowseDialog()
        {
            Stream myStream = null;
            OpenFileDialog fileDialog_ToCopy = new OpenFileDialog();

            fileDialog_ToCopy.InitialDirectory = "c:\\";
            fileDialog_ToCopy.Filter = "executable files (*.exe)|*.exe|All files (*.*)|*.*";
            fileDialog_ToCopy.FilterIndex = 2;
            fileDialog_ToCopy.RestoreDirectory = true;

            if (fileDialog_ToCopy.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = fileDialog_ToCopy.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


    }
}
