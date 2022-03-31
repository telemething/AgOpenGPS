﻿using AgIO.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Speech.Recognition;
using System.Text;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormLoop : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();

        public FormLoop()
        {
            InitializeComponent();
        }

        public StringBuilder logNMEASentence = new StringBuilder();
        public StringBuilder logNMEASentence2 = new StringBuilder();

        public bool isKeyboardOn = true;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        public double secondsSinceStart, lastSecond;

        public string lastSentence;

        //The base directory where Drive will be stored and fields and vehicles branch from
        public string baseDirectory;

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            if (Settings.Default.setF_workingDirectory == "Default")
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else baseDirectory = Settings.Default.setF_workingDirectory + "\\AgOpenGPS\\";

            //get the fields directory, if not exist, create
            commDirectory = baseDirectory + "AgIO\\";
            string dir = Path.GetDirectoryName(commDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            if (Settings.Default.setUDP_isOn) LoadUDPNetwork();
            LoadLoopback();

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;

            lblGPS1Comm.Text = "---";

            //set baud and port from last time run
            baudRateGPS = Settings.Default.setPort_baudRateGPS;
            portNameGPS = Settings.Default.setPort_portNameGPS;
            wasGPSConnectedLastRun = Settings.Default.setPort_wasGPSConnected;
            if (wasGPSConnectedLastRun)
            {
                OpenGPSPort();
                if (spGPS.IsOpen) lblGPS1Comm.Text = portNameGPS;
            }

            // set baud and port for rtcm from last time run
            baudRateRtcm = Settings.Default.setPort_baudRateRtcm;
            portNameRtcm = Settings.Default.setPort_portNameRtcm;
            wasRtcmConnectedLastRun = Settings.Default.setPort_wasRtcmConnected;
            
            if (wasRtcmConnectedLastRun)
            {
                OpenRtcmPort();
            }

            ConfigureNTRIP();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            listBox1.Items.Clear();

            if (ports.Length == 0)
            {
                listBox1.Items.Add("None");
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    listBox1.Items.Add(ports[i]);
                }
            }

            lastSentence = Properties.Settings.Default.setGPS_lastSentence;

            timer1.Enabled = true;
            panel1.Visible = false;
            pictureBox1.BringToFront();
            pictureBox1.Dock = DockStyle.Fill;
        }

        //current directory of Comm storage
        public string commDirectory, commFileName = "";

        private void btnDeviceManager_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void btnRescanPorts_Click(object sender, EventArgs e)
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            listBox1.Items.Clear();

            if (ports.Length == 0)
            {
                listBox1.Items.Add("None");
                return;
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    listBox1.Items.Add(ports[i]);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval > 1000)
            {
                Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
                panel1.Visible = true;
                timer1.Interval = 1000;
                return;
            }

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            DoTraffic();

            lblCurentLon.Text = longitude.ToString("N7");
            lblCurrentLat.Text = latitude.ToString("N7");

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

            //send back to Drive proof of life
            //every 3 seconds
            if ((secondsSinceStart - lastSecond) > 2)
            {
                if (isLogNMEA)
                {
                    using (StreamWriter writer = new StreamWriter("zAgIO_log.txt", true))
                    {
                        writer.Write(logNMEASentence.ToString());
                    }
                    logNMEASentence.Clear();
                    using (StreamWriter writer = new StreamWriter("zAgIO_Tool_log.txt", true))
                    {
                        writer.Write(logNMEASentence2.ToString());
                    }
                    logNMEASentence2.Clear();
                }

                lastSecond = secondsSinceStart;

                if (wasGPSConnectedLastRun)
                {
                    if (!spGPS.IsOpen)
                    {
                        wasGPSConnectedLastRun = false;
                        lblGPS1Comm.Text = "---";
                    }
                }
            }
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void btnBringUpCommSettings_Click(object sender, EventArgs e)
        {
            SettingsCommunicationGPS();
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            SettingsUDP();
        }

        private void btnRunAOG_Click(object sender, EventArgs e)
        {
            StartAOG();
        }

        private void btnNTRIP_Click(object sender, EventArgs e)
        {
            SettingsNTRIP();
        }

        private void btnRadio_Click(object sender, EventArgs e)
        {
            SettingsRadio();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ConfigureNTRIP()
        {
            lblWatch.Text = "Wait GPS";

            //start NTRIP if required
            isNTRIP_RequiredOn = Settings.Default.setNTRIP_isOn;
            isRadio_RequiredOn = Settings.Default.setRadio_isOn;

            if (isRadio_RequiredOn)
            {
                // Immediatly connect radio
                ntripCounter = 20;
            }

            if (isNTRIP_RequiredOn || isRadio_RequiredOn)
            {
                btnStartStopNtrip.Visible = true;
                btnStartStopNtrip.Visible = true;
                lblWatch.Visible = true;
                lblNTRIPBytes.Visible = true;
                lblBytes.Visible = true;
            }
            else
            {
                btnStartStopNtrip.Visible = false;
                btnStartStopNtrip.Visible = false;
                lblWatch.Visible = false;
                lblNTRIPBytes.Visible = false;
                lblBytes.Visible = false;
            }

            btnStartStopNtrip.Text = "Off";
        }

        private void uDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsUDP();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void toolStripGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void toolStripAgDiag_Click(object sender, EventArgs e)
        {
            Process[] processName = Process.GetProcessesByName("AgDiag");
            if (processName.Length == 0)
            {
                //Start application here
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                string strPath = di.ToString();
                strPath += "\\AgDiag.exe";
                //TimedMessageBox(8000, "No File Found", strPath);

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = strPath,
                        //processInfo.ErrorDialog = true;
                        //processInfo.UseShellExecute = false;
                        WorkingDirectory = Path.GetDirectoryName(strPath)
                    };
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgDiag");
                }
            }
            else
            {
                //Set foreground window

                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);

            }

        }

        public bool isLogNMEA;

        private void cboxLogNMEA_CheckedChanged(object sender, EventArgs e)
        {
            isLogNMEA = cboxLogNMEA.Checked;
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setPort_wasGPSConnected = wasGPSConnectedLastRun;
            Properties.Settings.Default.setPort_wasRtcmConnected = wasRtcmConnectedLastRun;
            Properties.Settings.Default.Save();

            if (recvFromAOGLoopBackSocket != null)
            {
                try
                {
                    recvFromAOGLoopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { recvFromAOGLoopBackSocket.Close(); }
            }

            if (sendToAOGLoopBackSocket != null)
            {
                try
                {
                    sendToAOGLoopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { sendToAOGLoopBackSocket.Close(); }
            }

            if (sendToUDPSocket != null)
            {
                try
                {
                    sendToUDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { sendToUDPSocket.Close(); }
            }

            if (recvFromUDPSocket != null)
            {
                try
                {
                    recvFromUDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { recvFromUDPSocket.Close(); }
            }
        }

        private void DoTraffic()
        {
            lblToAOG.Text = traffic.cntrPGNToAOG == 0 ? "--" : (traffic.cntrPGNToAOG).ToString();
            lblFromAOG.Text = traffic.cntrPGNFromAOG == 0 ? "--" : (traffic.cntrPGNFromAOG).ToString();

            lblFromUDP.Text = traffic.cntrUDPIn == 0 ? "--" : (traffic.cntrUDPIn).ToString();
            lblToUDP.Text = traffic.cntrUDPOut == 0 ? "--" : (traffic.cntrUDPOut).ToString();

            lblFromGPS.Text = traffic.cntrGPSIn == 0 ? "--" : (traffic.cntrGPSIn).ToString();
            lblToGPS.Text = traffic.cntrGPSOut == 0 ? "--" : (traffic.cntrGPSOut).ToString();

            lblFromGPS2.Text = traffic.cntrGPS2In == 0 ? "--" : (traffic.cntrGPS2In).ToString();
            lblToGPS2.Text = traffic.cntrGPS2Out == 0 ? "--" : (traffic.cntrGPS2Out).ToString();

            lblFromModule1.Text = traffic.cntrModule1In == 0 ? "--" : (traffic.cntrModule1In).ToString();
            lblToModule1.Text = traffic.cntrModule1Out == 0 ? "--" : (traffic.cntrModule1Out).ToString();

            lblFromModule2.Text = traffic.cntrModule2In == 0 ? "--" : (traffic.cntrModule2In).ToString();
            lblToModule2.Text = traffic.cntrModule2Out == 0 ? "--" : (traffic.cntrModule2Out).ToString();

            lblFromModule3.Text = traffic.cntrModule3In == 0 ? "--" : (traffic.cntrModule3In).ToString();
            lblToModule3.Text = traffic.cntrModule3Out == 0 ? "--" : (traffic.cntrModule3Out).ToString();

            traffic.cntrPGNToAOG = traffic.cntrPGNFromAOG = traffic.cntrUDPIn = traffic.cntrUDPOut =
                traffic.cntrGPSIn = traffic.cntrGPSOut = traffic.cntrGPS2In = traffic.cntrGPS2Out =
                traffic.cntrModule3In = traffic.cntrModule3Out =
                traffic.cntrModule1In = traffic.cntrModule1Out =
                traffic.cntrModule2Out = traffic.cntrModule2In = 0;

            lblCurentLon.Text = longitude.ToString("N7");
            lblCurrentLat.Text = latitude.ToString("N7");
        }
    }
}

