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
    public partial class FrmMain : Form, IcpsLIB
    {
        cpsLIB.net_udp net_udp;
        
        public FrmMain()
        {
            cpsLIB.log.clear();
            net_udp = new cpsLIB.net_udp(this);
            InitializeComponent();

            string HostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostEntry(HostName);

            label_host_name.Text = HostName;
            foreach (System.Net.IPAddress ip in hostInfo.AddressList)
                comboBox_local_ips.Items.Add(ip.ToString());
            checkBox_start_server.Checked = true;

            init_TimerUpdateGui();
            TimerUpdateGui.Start();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            TimerUpdateGui.Stop();

            if (TimerSendCyclic != null)
                TimerSendCyclic.Stop();
        }
        #region menue
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_fstack.Clear();
            richTextBox_fstackLog.Clear();
            net_udp.reset();
        }
        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tmp = cpsLIB.log.FilePath();
            System.Diagnostics.Process.Start(tmp);
        }
        #endregion

        #region GUI send
        private void button_send_request_Click(object sender, EventArgs e)
        {
            send();
        }
        private void button_check_Click(object sender, EventArgs e)
        {
            net_udp.check_connection(textBox_remote_ip.Text, textBox_remotePort.Text);
        }
        private void button_send_repeat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(textBox_send_multiplikator.Text); i++)
                send();
        }

        #endregion

        #region send fkt
        private void send() {
            if (!String.IsNullOrEmpty(textBox_send.Text))
            {
                Int16 counter = Convert.ToInt16(textBox_send_index.Text);
                counter++;
                textBox_send_index.Text = counter.ToString();

                string[] msg = textBox_send.Text.Split(',', ';', ' ');
                Frame f;

                if (radioButton_send_byte.Checked)
                {
                    //### frame aus payload zahlen erstellen
                    Int16[] intArray = new Int16[msg.Length];
                    for (int i = 0; i < msg.Length; i++)
                        intArray[i] = Int16.Parse(msg[i]);

                    f = new cpsLIB.Frame(comboBox_frame_type.Text, counter, intArray, textBox_remote_ip.Text, textBox_remotePort.Text);

                }
                else
                {
                    //#### frame aus payload chars erstellen
                    char[] strArray = StrToChr(msg);
                    f = new cpsLIB.Frame(comboBox_frame_type.Text, counter, strArray, textBox_remote_ip.Text, textBox_remotePort.Text);
                }

                for (int i = 0; i < Convert.ToInt32(textBox_send_multiplikator.Text); i++)
                    net_udp.send(f);
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
            //cpsLIB.Frame. = true;
            cpsLIB.Frame.SendBigEndian = checkBox_send_big_endian.Checked;
        }

        private void checkBox_receive_big_endian_CheckedChanged(object sender, EventArgs e)
        {
            cpsLIB.Frame.ReceiveBigEndian = checkBox_receive_big_endian.Checked;
        }

        private void comboBox_local_ips_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_remote_ip.Text = comboBox_local_ips.Text;
            textBox_remotePort.Text = textBox_srv_port.Text;
        }
        #endregion

        #region server GUI
        private void checkBox_start_server_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_start_server.Checked)
                net_udp.serverSTART(textBox_srv_port.Text);
            else
                net_udp.serverSTOP();
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
        private void srv_msg_funkt(string s)
        {
            richTextBox_fstackLog.AppendText("srv_msgCallback: " + s + Environment.NewLine);
        }
        #endregion

        #region logSendRcv
        private delegate void logSendRcvCallback(Frame f);
        void IcpsLIB.logSendRcv(object o)
        {
            try
            {
                this.Invoke(new logSendRcvCallback(this.logSendRcv_fkt), new object[] { o });
            }
            catch (Exception e)
            {
                MessageBox.Show("projectIcpsLIB.IcpsLIB.send: " + e.Message, "writing to GUI failed");
            }
        }
        private void logSendRcv_fkt(Frame f)
        {

            if (f.sender.Equals(FrameSender.client))
                richTextBox_fstackLog.AppendText("==> " + f.GetDetailedString() + Environment.NewLine);
            else if (f.sender.Equals(FrameSender.server))
                richTextBox_fstackLog.AppendText("<== " + f.GetDetailedString() + Environment.NewLine);
            else
                richTextBox_fstackLog.AppendText("[err] " + f.GetDetailedString() + Environment.NewLine);
        }

        #endregion

        #region interprete_frame
        private delegate void interprete_frameCallback(Frame f);
        void IcpsLIB.interprete_frame(object o)
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new interprete_frameCallback(this.interprete_frame_fkt), new object[] { o });
                else
                {
                    MessageBox.Show("interprete_frameCallback(): " + " this.InvokeRequired == false");
                    interprete_frame_fkt((cpsLIB.Frame)o);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("NetUdpFrameCallback: " + e.Message, "writing to GUI failed");
            }
        }

        private void interprete_frame_fkt(Frame f)
        {
            richTextBox_fstackLog.AppendText("ITP: " + f.GetDetailedString() + Environment.NewLine);

            textBox_frame_msg.Text = f.ToString();
            textBox_msg_payload.Text = f.getPayload();
            textBox_msg_payload_ASCII.Text = f.getPayloadASCII();
        }
        #endregion

        #endregion

        #region Timer cyclic loop
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
            label1.Text =
    "state: " + net_udp.state.ToString() + Environment.NewLine +
    "InWorkFrameCount: " + net_udp.InWorkFrameCount() + Environment.NewLine +
    "TotalFramesSend: " + net_udp.TotalFramesSend + Environment.NewLine +
    "TotalFramesReceive" + net_udp.TotalFramesReceive + Environment.NewLine +
    "TotalFramesFinished: " + net_udp.TotalFramesFinished.ToString() + Environment.NewLine +
    "check_trys: " + net_udp.check_trys.ToString() + Environment.NewLine +
    "fstackLogCount: " + net_udp.fstackLogCount().ToString() + Environment.NewLine;


            //richTextBox_fstack.Clear();
            //if(net_udp.
            richTextBox_fstack.Text = richTextBox_fstack.Text + Environment.NewLine + net_udp.GetStackAsString();
        }


        #endregion









 
    }
}
