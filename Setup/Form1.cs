using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Setup
{
    public partial class Form1 : Form
    {
        public static Form1 instance;
        public Form1()
        {
            instance = this;
            InitializeComponent();
        }

        public static void writeLog(string text)
        {
            instance.textBox2.AppendText(text);
            instance.textBox2.ScrollToCaret();
        }
        public static void writeLogLine(string text)
        {
            instance.textBox2.AppendText(text + "\r\n");
            instance.textBox2.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs ev)
        {
            try
            {
                writeLogLine("Starting installation...");
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                writeLogLine("Checking path...");
                if (textBox1.Text.Length == 0) throw new ArgumentException("Selected installation path is empty.");
                string path = Directory.Exists(textBox1.Text) ? textBox1.Text : Path.GetDirectoryName(textBox1.Text);
                writeLogLine("Checking " + path);
                if (!Directory.Exists(path)) throw new ArgumentException("Selected installation path does not exist.");
                if (!File.Exists(path + "\\LaunchGTAIV.exe") || !File.Exists(path + "\\GTAIV.exe")) throw new InvalidDataException("Selected path does not seem to contain a valid GTA IV installation.");
                writeLogLine("Path seems valid - downloading required files!");
                WebClient client = new WebClient();
                string tmpUnpacker = Path.GetTempFileName() + ".exe"; // generates a random name for temp file with .exe on the end
                string tmpFiles = Path.GetTempFileName() + ".zip"; // generates a random name for temp file with .zip on the end
                //writeLogLine("Downloading 7z unpacker.");
                //Application.DoEvents();
                //client.DownloadFile("http://gta.vdgtech.eu/install_7z.exe", tmpUnpacker); // might not need.
                writeLogLine("Downloading sources and binaries from GitHub.");
                Application.DoEvents();
                client.DownloadFile("https://github.com/vhexs/gta-iv-multiplayer/archive/master.zip", tmpFiles); // download entire source inc binaries
                // write files
                writeLogLine("Saving to file and unpacking...");
                Application.DoEvents();
                //start unpacking
                string zipPath = @tmpFiles;
                string extractPath = @path+"\\temp_miv\\";// this will extract to the temp_miv folder in the path that the user chose
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                /*Process unpacker = new Process();
                unpacker.StartInfo = new ProcessStartInfo(tmpUnpacker, "x -y -o\"" + path + "\" \"" + tmpFiles + "\"");
                unpacker.StartInfo.UseShellExecute = false;
                unpacker.StartInfo.CreateNoWindow = true;
                unpacker.StartInfo.RedirectStandardOutput = true;
                unpacker.StartInfo.RedirectStandardError = true;
                unpacker.Start();
                var stdout = unpacker.StandardOutput;
                while (!stdout.EndOfStream)
                {
                    char[] block = new char[32];
                    int count = stdout.ReadBlock(block, 0, 32);

                    string line = String.Join<char>("", block);
                    writeLog(line);
                    Application.DoEvents();
                }*/
                string fileListPathServer = extractPath+"\\server\\";
                string fileListPathClient = extractPath+"\\client\\";
                string[] fileList = System.IO.Directory.GetFiles(fileListPathServer); //move server files first
                foreach (string file in fileList) {
                    string fileToMove = fileListPathServer+file;
                    string moveTo = path+file;
                    //moving file
                    File.Move(fileToMove, moveTo);
                }
                string[] fileListC = System.IO.Directory.GetFiles(fileListPathServer); //move server files first
                foreach (string file in fileListC) {
                    string fileToMove = fileListPathClient+file;
                    string moveTo = path+file;
                    //moving file
                    File.Move(fileToMove, moveTo);
                }
                writeLogLine("Finished!");
                var result = MessageBox.Show("MIV has been installed. Would you like to play now?", "Hooray!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(path + "\\MIVClientGUI.exe")
                    {
                        WorkingDirectory = path
                    });
                }
                Application.Exit();
            }
            catch (Exception e)
            {
                writeLogLine("Oh no! Error: " + e.GetType().ToString() + ": " + e.Message);
                writeLogLine("Installation failed, something went wrong. (Submit an issue on the GitHub page with the above error message if you don't know what happened.)");
            }
            finally
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileOk += (o, ev) =>
            {
                if (!ev.Cancel && openFileDialog1.FileName.Length > 0)
                {
                    textBox1.Text = Path.GetDirectoryName(openFileDialog1.FileName);
                }
            };
            openFileDialog1.ShowDialog();
        }
    }
}
