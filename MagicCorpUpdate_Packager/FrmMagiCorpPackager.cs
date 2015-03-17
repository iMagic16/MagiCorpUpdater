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
using System.Security.Cryptography;

namespace MagicCorpUpdate_Packager
{
    public partial class FrmMUPackage : Form
    {
        public FrmMUPackage()
        {
            InitializeComponent();
        }

        public static string version = "101";

        public static string username, hostname, password, sha256generated;
        public static int port;

        private void FrmMUPackage_Load(object sender, EventArgs e)
        {
            //write runtimes
            File.WriteAllBytes("WinSCP.exe", Properties.Resources.WinSCP);
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
            fileDialog_ToCopy.FilterIndex = 1;
            fileDialog_ToCopy.RestoreDirectory = true;

            if (fileDialog_ToCopy.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = fileDialog_ToCopy.OpenFile()) != null)
                    {
                        TxtFilesToUpload.Text = myStream.ToString();
                        using (myStream)
                        {

                           // Stream WorkingDir = new Stream();
                            // Insert code to read the stream here.
                           // myStream.CopyToAsync(Directory.GetCurrentDirectory);


                            

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnFindZip_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog fileDialog_ToCopy = new OpenFileDialog();

            fileDialog_ToCopy.InitialDirectory = "c:\\";
            fileDialog_ToCopy.Filter = "compressed files (*.zip)|*.zip";
            fileDialog_ToCopy.FilterIndex = 1;
            fileDialog_ToCopy.RestoreDirectory = true;

            if (fileDialog_ToCopy.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = fileDialog_ToCopy.OpenFile()) != null)
                    {

                        using (myStream)
                        {
                        //    SHA256Check(myStream.);

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
            
        private void BtnGensha_Click(object sender, EventArgs e)
        {
            SHA256Check(TxtTo256.Text, TxtVersion.Text);
        }




        //check file integrity
        static void SHA256Check(string FileTo256, string version)
        {
            Debug.ConOut("SHA256 check in progress...", false, true);

            Debug.ConOut("Opening file to generate sha256...");
            FileStream fs = File.Open(FileTo256, FileMode.Open, FileAccess.Read, FileShare.None);

            Debug.ConOut("Create sha256 instance");
            SHA256 sha256r = SHA256Managed.Create();

            Debug.ConOut("Create hash from file...");
            byte[] hashvalue = sha256r.ComputeHash(fs);

            Debug.ConOut("Closing file");
            fs.Close();

            Debug.ConOut("Convert byte array into a string");
            PrintByteArray(hashvalue);

            File.WriteAllText(version + ".sha256", sha256generated);

        }

        // Print the byte array in a readable format. 
        public static string PrintByteArray(byte[] array)
        {
            int i;
            for (i = 0; i < array.Length; i++)
            {
                sha256generated += String.Format("{0:X2}", array[i]);
                Debug.ConOut(sha256generated, false, false, true);
                if ((i % 4) == 3)
                {
                    Debug.ConOut(" ", false, false, true);
                    sha256generated += " ";
                }
            }
            Console.WriteLine();
            sha256generated = sha256generated.ToLower();
            sha256generated = sha256generated.Replace(" ", "");
            return sha256generated;
        }


    }
}
