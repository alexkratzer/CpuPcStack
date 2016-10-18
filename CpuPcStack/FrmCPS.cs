using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cpsLIB;

namespace CpuPcStack
{
    public partial class FrmCPS : Form, IcpsLIB
    {
        BindingList<FrameRawData> ListFrames = new BindingList<FrameRawData>();
        cpsLIB.CpsNet CpsNet;
        
        public FrmCPS()
        {
            CpsNet = new cpsLIB.CpsNet(this);
            InitializeComponent();

            string HostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostEntry(HostName);

            label_host_name.Text = HostName;
            foreach (System.Net.IPAddress ip in hostInfo.AddressList)
                comboBox_local_ips.Items.Add(ip.ToString() );

            CpsNet.serverSTART(textBox_srv_port.Text);
            //cpsCMD.

            init_TimerUpdateGui();
            TimerUpdateGui.Start();

            listBox_frameLog.DataSource = ListFrames;

            textBox_MaxSYNCResendTrys.Text = CpsNet.MaxSYNCResendTrys.ToString();
            textBox_WATCHDOG_WORK.Text = CpsNet.WATCHDOG_WORK.ToString();
            checkBox_SendFramesCallback.Checked = CpsNet.SendFramesCallback;
            checkBox_SendOnlyIfConnected.Checked = CpsNet.SendOnlyIfConnected;
            checkBox_send_big_endian.Checked = Frame._RemoteIsBigEndian;


            makeNewClient();
            comboBox_listClients.SelectedIndex = 0;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CpsNet.cleanup();
            TimerStop();
        }

        public object newClient(string ip, string port) {
            return CpsNet.newClient(ip, port);
        }

        #region menue
        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("logging not active. see class log.cs");
            //string tmp = cpsLIB.log.FilePath();
            //if(System.IO.File.Exists(tmp))
            //    System.Diagnostics.Process.Start(tmp);
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CpsNet.serverSTART(textBox_srv_port.Text); 
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CpsNet.serverSTOP();
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ListServerStatus, "server status");
        }


        #endregion

        #region GUI send
        private void button_send_request_Click(object sender, EventArgs e)
        {
            send();
        }
        private void button_check_Click(object sender, EventArgs e)
        {
            if (!CpsNet.send_SYNC((CpsClient)comboBox_listClients.SelectedItem))
                MessageBox.Show("no connection found....", "CpsNet.send_SYNC()");
        }
        private void button_send_repeat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(textBox_send_multiplikator.Text); i++)
            {
                send();
            }
        }

        private void button_set_time_Click(object sender, EventArgs e)
        {
            DateTime TimeNow = DateTime.Now;
            Frame f = new Frame((CpsClient)comboBox_listClients.SelectedItem, new Int16[] { (Int16)HeaderFlagManagementData.SetTime, 
            (Int16)TimeNow.Year, (Int16)TimeNow.Month, (Int16)TimeNow.Day, (Int16)TimeNow.Hour, (Int16)TimeNow.Minute, (Int16)TimeNow.Second});
            f.SetHeaderFlag(FrameHeaderFlag.ManagementData);
            CpsNet.send(f);
        }
        private void button_get_time_Click(object sender, EventArgs e)
        {
            Frame f = new Frame((CpsClient)comboBox_listClients.SelectedItem, new Int16[] { (Int16)HeaderFlagManagementData.GetTime });
            f.SetHeaderFlag(FrameHeaderFlag.ManagementData);
            CpsNet.send(f);
        }
        #endregion

        #region send fkt
        private void send() {
            if (!String.IsNullOrEmpty(textBox_send.Text))
            {
                string[] msg = textBox_send.Text.Split(',', ';', ' ');
                
                Frame f;

                if (radioButton_send_byte.Checked)
                {
                    //### frame aus payload zahlen erstellen
                    Int16[] intArray = new Int16[msg.Length];
                    for (int i = 0; i < msg.Length; i++)
                        intArray[i] = Int16.Parse(msg[i]);

                    //f = new Frame(textBox_remote_ip.Text, textBox_remotePort.Text, intArray);
                    f = new Frame((CpsClient)comboBox_listClients.SelectedItem, intArray);
                }
                else
                {
                    //#### frame aus payload chars erstellen
                    char[] strArray = StrToChr(msg);
                    f = new Frame((CpsClient)comboBox_listClients.SelectedItem, strArray);
                }

                if (checkBox_SYNC.Checked)
                    f.SetHeaderFlag(FrameHeaderFlag.SYNC);
                if (checkBox_ManagementData.Checked)
                    f.SetHeaderFlag(FrameHeaderFlag.ManagementData);
                if (checkBox_acknowledge.Checked)
                    f.SetHeaderFlag(FrameHeaderFlag.acknowledge);
                CpsNet.send(f);

                //string s = "";
                //s += "FrameHeaderFlag.containering: " + f.GetHeaderFlag(FrameHeaderFlag.containering) + Environment.NewLine;
                //s += "FrameHeaderFlag.SYNC: " + f.GetHeaderFlag(FrameHeaderFlag.SYNC) + Environment.NewLine;
                //s += "FrameHeaderFlag.LogMessage: " + f.GetHeaderFlag(FrameHeaderFlag.LogMessage) + Environment.NewLine;
                //s += "FrameHeaderFlag.acknowledge: " + f.GetHeaderFlag(FrameHeaderFlag.acknowledge) + Environment.NewLine;
                //s += "FrameHeaderFlag.ProcessData_value: " + f.GetHeaderFlag(FrameHeaderFlag.ProcessData_value) + Environment.NewLine;
                //s += "FrameHeaderFlag.ProcessData_param: " + f.GetHeaderFlag(FrameHeaderFlag.ProcessData_param) + Environment.NewLine;
                //s += "FrameHeaderFlag.ManagementData: " + f.GetHeaderFlag(FrameHeaderFlag.ManagementData) + Environment.NewLine;
                 
                //MessageBox.Show(s);
            }
            else
                MessageBox.Show("ERROR, payload is empty");
        }

        private char[] StrToChr(string[] Strings)
        {
            //merges the items in the Strings collection into one string
            string merged = "";

            foreach (string str in Strings)
                merged += str;

            //returns a char array that represents the merged string
            return merged.ToCharArray();
        }
        private void checkBox_big_endian_CheckedChanged(object sender, EventArgs e)
        {
            Frame._RemoteIsBigEndian = checkBox_send_big_endian.Checked;
        }

        private void comboBox_local_ips_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_remote_ip.Text = comboBox_local_ips.Text;
            textBox_remotePort.Text = textBox_srv_port.Text;
        }
        #endregion

        /*
        #region server fkt
        private delegate void interprete_frameCallback(Frame f);
        public void _interprete_frame(object f)
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new interprete_frameCallback(this.interprete_frame_funkt), new object[] { f });
                else
                {
                    MessageBox.Show("interprete_frameCallback(): " + " this.InvokeRequired == false");
                    interprete_frame_funkt((cpsLIB.Frame)f);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("interprete_frameCallback: " + e.Message, "writing to GUI failed");
            }
        }
        private void interprete_frame_funkt(cpsLIB.Frame f)
        {
            richTextBox_srv_msg.AppendText("##INTERP## " + f.GetDetailedString() + Environment.NewLine);
            textBox_frame_msg.Text = f.ToString();
            textBox_msg_payload.Text = f.getPayload();
            textBox_msg_payload_ASCII.Text = f.getPayloadASCII();
        }

        #endregion 
         * */

        #region cpsLIB callback
        #region logMsg
        private delegate void srv_msgCallback(string s);
        void IcpsLIB.logMsg(string msg)
        {
            try
            {
                this.Invoke(new srv_msgCallback(this.srv_msg_funkt), new object[] { msg });
            }
            catch (Exception e)
            {
                MessageBox.Show("srv_msgCallback: " + e.Message, "writing to GUI failed");
            }
        }

        string ListServerStatus;
        private void srv_msg_funkt(string s)
        {
            //TODO: in liste abspeichern... oder liste in cmd verwalten
            tssl_server_status.Text = s;
            ListServerStatus += s + Environment.NewLine;
        }
        #endregion

        #region interprete_frame
        private delegate void interprete_frameCallback(object f);
        void IcpsLIB.interprete_frame(object o)
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new interprete_frameCallback(this.interprete_frame_fkt), new object[] { o });
                else
                    interprete_frame_fkt(o);
            }
            catch (Exception e)
            {
                MessageBox.Show("interprete_frameCallback: " + e.Message, "writing to GUI failed");
            }
        }

        
        private void interprete_frame_fkt(object f)
        {
            FrameRawData _f = (FrameRawData)f;
            //_f.ChangeState(FrameWorkingState.inWork, "frame @FrmMain: interprete_frame_fkt");

            //TODO: ############################# 
            //if(_f.frameState.Equals(FrameState.ERROR)

            ListFrames.Add(_f);
            //listBox_frameLog.Items.Add(f);

        }
        #endregion

        #region unused logSendRcv
        /*
        private delegate void logSendRcvCallback(Frame f);
        void IcpsLIB.logSendRcv(object o)
        {
            log.msg(this, "unused funktion: logSendRcv");
            
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new logSendRcvCallback(this.logSendRcv_fkt), new object[] { o });
                else
                    logSendRcv_fkt((Frame)o);
            }
            catch (Exception e)
            {
                MessageBox.Show("projectIcpsLIB.IcpsLIB.send: " + e.Message, "writing to GUI failed");
            }
        }

        private void logSendRcv_fkt(Frame f)
        {

            if (f.frameSender.Equals(FrameSender.client))
                richTextBox_fstackLog.AppendText("==> " + f.GetMetaInfo() + Environment.NewLine);
            else if (f.frameSender.Equals(FrameSender.server))
                richTextBox_fstackLog.AppendText("<== " + f.GetMetaInfo() + Environment.NewLine);
            else
                richTextBox_fstackLog.AppendText("[err] " + f.GetMetaInfo() + Environment.NewLine);
        
        }
         * * */
        #endregion

        #endregion

        #region Timer cyclic loop
        /// <summary>
        /// timer um zyklisch frames zu versenden
        /// </summary>
        System.Windows.Forms.Timer TimerSendCyclic;
        private void checkBox_cyclic_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_cyclic.Checked)
            {
                init_TimerSendCyclic();
                TimerSendCyclic.Start();
            }
            else {
                if (TimerSendCyclic != null)
                    TimerSendCyclic.Stop();
            }

        }
        private void init_TimerSendCyclic()
        {
            int interval;
            if (int.TryParse(textBox_timer_interval.Text, out interval))
            {
                TimerSendCyclic = new Timer();
                TimerSendCyclic.Interval = interval;
                TimerSendCyclic.Tick += new EventHandler(timer_cyclic_Tick);
            }
        }

        void timer_cyclic_Tick(object sender, EventArgs e)
        {
            send();
        }

        System.Windows.Forms.Timer TimerUpdateGui;
        private void init_TimerUpdateGui()
        {
            TimerUpdateGui = new Timer();
            TimerUpdateGui.Interval = 300;
            TimerUpdateGui.Tick += new EventHandler(TimerUpdateGui_Tick);            
        }
        private void TimerUpdateGui_Tick(object sender, EventArgs e)
        {
            tSSl_connection_status.Text = "[Frame min: " + CpsNet.TimeRcvAnswerMin.Milliseconds.ToString() + "ms max: " +
                CpsNet.TimeRcvAnswerMax.Milliseconds.ToString() + "ms @work: " + CpsNet.InWorkFrameCount() + " done: " +
                CpsNet.TotalFramesFinished.ToString() + " send: " + Frame.GetCountSendFrames() + " received: " + Frame.GetCountRcvFrames() + "]";
        }

        private void TimerStop() {
            TimerUpdateGui.Stop();

            if (TimerSendCyclic != null)
                TimerSendCyclic.Stop();
        }

        #endregion

        #region gui settings
        private void listBox_frameLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_frameLog.SelectedItem != null)
            {
                Frame f = (Frame)listBox_frameLog.SelectedItem;
                UpdateFrameDataInGUI(f);
            }
        }

        private void listBox_clientFrameLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_clientFrameLog.SelectedItem != null)
                UpdateFrameDataInGUI((Frame)listBox_clientFrameLog.SelectedItem);
        }

        private void UpdateFrameDataInGUI(Frame f) {
            
            textBox_msg_payload_byte.Text = f.getPayloadByte();
            textBox_msg_payload_hex.Text = f.getPayloadHex();
            textBox_msg_payload_int.Text = f.getPayloadInt();
            textBox_msg_payload_ASCII.Text = f.getPayloadASCII();
            //sender
            label_frameLog.Text = f.GetLog();
            label_frameMetadata.Text = f.ToString();

            if (f.AnswerFrame != null)
                label_frameLog_answer.Text = f.AnswerFrame.GetLog();
            else
                label_frameLog_answer.Text = "";
        }

        private void textBox_MaxSYNCResendTrys_TextChanged(object sender, EventArgs e)
        {
            CpsNet.MaxSYNCResendTrys = Convert.ToInt16(textBox_MaxSYNCResendTrys.Text);
        }

        private void textBox_WATCHDOG_WORK_TextChanged(object sender, EventArgs e)
        {
            CpsNet.WATCHDOG_WORK = Convert.ToInt16(textBox_WATCHDOG_WORK.Text);
        }

        private void checkBox_SendFramesCallback_CheckedChanged(object sender, EventArgs e)
        {
            CpsNet.SendFramesCallback = checkBox_SendFramesCallback.Checked;
        }

        private void checkBox_SendOnlyIfConnected_CheckedChanged(object sender, EventArgs e)
        {
            CpsNet.SendOnlyIfConnected = checkBox_SendOnlyIfConnected.Checked;
        }
        #endregion

        private void button_newClient_Click(object sender, EventArgs e)
        {
            makeNewClient(); 
        }

        private void makeNewClient() {
            comboBox_listClients.Items.Add(newClient(textBox_remote_ip.Text, textBox_remotePort.Text));
        }

        private void button_refresh_clientFrameLog_Click(object sender, EventArgs e)
        {
            listBox_clientFrameLog.Items.Clear();

            foreach (Frame f in ((CpsClient)comboBox_listClients.SelectedItem).LFrame)
            {
                listBox_clientFrameLog.Items.Add(f);
            }
        }

      




    }
}
