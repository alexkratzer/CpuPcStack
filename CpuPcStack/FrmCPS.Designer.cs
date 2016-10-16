namespace CpuPcStack
{
    partial class FrmCPS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCPS));
            this.label_ip = new System.Windows.Forms.Label();
            this.groupBox_client = new System.Windows.Forms.GroupBox();
            this.label_host_name_desc = new System.Windows.Forms.Label();
            this.label_srv_port = new System.Windows.Forms.Label();
            this.textBox_remotePort = new System.Windows.Forms.TextBox();
            this.textBox_srv_port = new System.Windows.Forms.TextBox();
            this.comboBox_local_ips = new System.Windows.Forms.ComboBox();
            this.label_host_name = new System.Windows.Forms.Label();
            this.label_port = new System.Windows.Forms.Label();
            this.textBox_remote_ip = new System.Windows.Forms.TextBox();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.button_send_request = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_msg_payload_int = new System.Windows.Forms.TextBox();
            this.label_payload_ascii = new System.Windows.Forms.Label();
            this.label_payload_byte = new System.Windows.Forms.Label();
            this.textBox_msg_payload_ASCII = new System.Windows.Forms.TextBox();
            this.textBox_msg_payload_byte = new System.Windows.Forms.TextBox();
            this.checkBox_receive_big_endian = new System.Windows.Forms.CheckBox();
            this.checkBox_send_big_endian = new System.Windows.Forms.CheckBox();
            this.groupBox_send_msg = new System.Windows.Forms.GroupBox();
            this.checkBox_ManagementData = new System.Windows.Forms.CheckBox();
            this.checkBox_acknowledge = new System.Windows.Forms.CheckBox();
            this.button_send_repeat = new System.Windows.Forms.Button();
            this.checkBox_SYNC = new System.Windows.Forms.CheckBox();
            this.label_send_times = new System.Windows.Forms.Label();
            this.button_check = new System.Windows.Forms.Button();
            this.label_cyclic_desc = new System.Windows.Forms.Label();
            this.checkBox_cyclic = new System.Windows.Forms.CheckBox();
            this.textBox_timer_interval = new System.Windows.Forms.TextBox();
            this.label_repeat = new System.Windows.Forms.Label();
            this.textBox_send_multiplikator = new System.Windows.Forms.TextBox();
            this.radioButton_send_ascii = new System.Windows.Forms.RadioButton();
            this.radioButton_send_byte = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_status = new System.Windows.Forms.GroupBox();
            this.label_frameLog_answer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_msg_payload_hex = new System.Windows.Forms.TextBox();
            this.label_frameMetadata = new System.Windows.Forms.Label();
            this.label_frameLog = new System.Windows.Forms.Label();
            this.listBox_frameLog = new System.Windows.Forms.ListBox();
            this.menuStrip_main = new System.Windows.Forms.MenuStrip();
            this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.stopServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssl_server_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox_settings = new System.Windows.Forms.GroupBox();
            this.checkBox_SendOnlyIfConnected = new System.Windows.Forms.CheckBox();
            this.checkBox_SendFramesCallback = new System.Windows.Forms.CheckBox();
            this.textBox_WATCHDOG_WORK = new System.Windows.Forms.TextBox();
            this.label_WATCHDOG_WORK = new System.Windows.Forms.Label();
            this.textBox_MaxSYNCResendTrys = new System.Windows.Forms.TextBox();
            this.label_MaxSYNCResendTrys_desc = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox_client.SuspendLayout();
            this.groupBox_send_msg.SuspendLayout();
            this.groupBox_status.SuspendLayout();
            this.menuStrip_main.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox_settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(6, 16);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(51, 13);
            this.label_ip.TabIndex = 0;
            this.label_ip.Text = "IPadress:";
            // 
            // groupBox_client
            // 
            this.groupBox_client.Controls.Add(this.label_host_name_desc);
            this.groupBox_client.Controls.Add(this.label_srv_port);
            this.groupBox_client.Controls.Add(this.textBox_remotePort);
            this.groupBox_client.Controls.Add(this.textBox_srv_port);
            this.groupBox_client.Controls.Add(this.comboBox_local_ips);
            this.groupBox_client.Controls.Add(this.label_host_name);
            this.groupBox_client.Controls.Add(this.label_port);
            this.groupBox_client.Controls.Add(this.textBox_remote_ip);
            this.groupBox_client.Controls.Add(this.label_ip);
            this.groupBox_client.Location = new System.Drawing.Point(12, 105);
            this.groupBox_client.Name = "groupBox_client";
            this.groupBox_client.Size = new System.Drawing.Size(313, 96);
            this.groupBox_client.TabIndex = 1;
            this.groupBox_client.TabStop = false;
            this.groupBox_client.Text = "remote";
            // 
            // label_host_name_desc
            // 
            this.label_host_name_desc.AutoSize = true;
            this.label_host_name_desc.Location = new System.Drawing.Point(165, 14);
            this.label_host_name_desc.Name = "label_host_name_desc";
            this.label_host_name_desc.Size = new System.Drawing.Size(32, 13);
            this.label_host_name_desc.TabIndex = 16;
            this.label_host_name_desc.Text = "Host:";
            // 
            // label_srv_port
            // 
            this.label_srv_port.AutoSize = true;
            this.label_srv_port.Location = new System.Drawing.Point(191, 60);
            this.label_srv_port.Name = "label_srv_port";
            this.label_srv_port.Size = new System.Drawing.Size(66, 13);
            this.label_srv_port.TabIndex = 5;
            this.label_srv_port.Text = "listen @port:";
            // 
            // textBox_remotePort
            // 
            this.textBox_remotePort.Location = new System.Drawing.Point(55, 39);
            this.textBox_remotePort.Name = "textBox_remotePort";
            this.textBox_remotePort.Size = new System.Drawing.Size(81, 20);
            this.textBox_remotePort.TabIndex = 3;
            this.textBox_remotePort.Text = "2202";
            // 
            // textBox_srv_port
            // 
            this.textBox_srv_port.Location = new System.Drawing.Point(263, 57);
            this.textBox_srv_port.Name = "textBox_srv_port";
            this.textBox_srv_port.Size = new System.Drawing.Size(44, 20);
            this.textBox_srv_port.TabIndex = 6;
            this.textBox_srv_port.Text = "50000";
            // 
            // comboBox_local_ips
            // 
            this.comboBox_local_ips.FormattingEnabled = true;
            this.comboBox_local_ips.Location = new System.Drawing.Point(168, 30);
            this.comboBox_local_ips.Name = "comboBox_local_ips";
            this.comboBox_local_ips.Size = new System.Drawing.Size(139, 21);
            this.comboBox_local_ips.TabIndex = 14;
            this.comboBox_local_ips.SelectedIndexChanged += new System.EventHandler(this.comboBox_local_ips_SelectedIndexChanged);
            // 
            // label_host_name
            // 
            this.label_host_name.AutoSize = true;
            this.label_host_name.Location = new System.Drawing.Point(203, 14);
            this.label_host_name.Name = "label_host_name";
            this.label_host_name.Size = new System.Drawing.Size(56, 13);
            this.label_host_name.TabIndex = 15;
            this.label_host_name.Text = "host name";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(6, 42);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(28, 13);
            this.label_port.TabIndex = 2;
            this.label_port.Text = "port:";
            // 
            // textBox_remote_ip
            // 
            this.textBox_remote_ip.Location = new System.Drawing.Point(55, 13);
            this.textBox_remote_ip.Name = "textBox_remote_ip";
            this.textBox_remote_ip.Size = new System.Drawing.Size(81, 20);
            this.textBox_remote_ip.TabIndex = 1;
            this.textBox_remote_ip.Text = "192.168.1.205";
            // 
            // textBox_send
            // 
            this.textBox_send.Location = new System.Drawing.Point(9, 70);
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.Size = new System.Drawing.Size(292, 20);
            this.textBox_send.TabIndex = 9;
            this.textBox_send.Text = "1,2,3,4";
            // 
            // button_send_request
            // 
            this.button_send_request.Location = new System.Drawing.Point(5, 18);
            this.button_send_request.Name = "button_send_request";
            this.button_send_request.Size = new System.Drawing.Size(112, 23);
            this.button_send_request.TabIndex = 11;
            this.button_send_request.Text = "send";
            this.button_send_request.UseVisualStyleBackColor = true;
            this.button_send_request.Click += new System.EventHandler(this.button_send_request_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(476, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "payload int:";
            // 
            // textBox_msg_payload_int
            // 
            this.textBox_msg_payload_int.Location = new System.Drawing.Point(553, 86);
            this.textBox_msg_payload_int.Name = "textBox_msg_payload_int";
            this.textBox_msg_payload_int.Size = new System.Drawing.Size(335, 20);
            this.textBox_msg_payload_int.TabIndex = 16;
            // 
            // label_payload_ascii
            // 
            this.label_payload_ascii.AutoSize = true;
            this.label_payload_ascii.Location = new System.Drawing.Point(475, 115);
            this.label_payload_ascii.Name = "label_payload_ascii";
            this.label_payload_ascii.Size = new System.Drawing.Size(71, 13);
            this.label_payload_ascii.TabIndex = 15;
            this.label_payload_ascii.Text = "payload ascii:";
            // 
            // label_payload_byte
            // 
            this.label_payload_byte.AutoSize = true;
            this.label_payload_byte.Location = new System.Drawing.Point(476, 39);
            this.label_payload_byte.Name = "label_payload_byte";
            this.label_payload_byte.Size = new System.Drawing.Size(70, 13);
            this.label_payload_byte.TabIndex = 14;
            this.label_payload_byte.Text = "payload byte:";
            // 
            // textBox_msg_payload_ASCII
            // 
            this.textBox_msg_payload_ASCII.Location = new System.Drawing.Point(553, 112);
            this.textBox_msg_payload_ASCII.Name = "textBox_msg_payload_ASCII";
            this.textBox_msg_payload_ASCII.Size = new System.Drawing.Size(335, 20);
            this.textBox_msg_payload_ASCII.TabIndex = 13;
            // 
            // textBox_msg_payload_byte
            // 
            this.textBox_msg_payload_byte.Location = new System.Drawing.Point(553, 36);
            this.textBox_msg_payload_byte.Name = "textBox_msg_payload_byte";
            this.textBox_msg_payload_byte.Size = new System.Drawing.Size(335, 20);
            this.textBox_msg_payload_byte.TabIndex = 12;
            // 
            // checkBox_receive_big_endian
            // 
            this.checkBox_receive_big_endian.AutoSize = true;
            this.checkBox_receive_big_endian.Location = new System.Drawing.Point(9, 37);
            this.checkBox_receive_big_endian.Name = "checkBox_receive_big_endian";
            this.checkBox_receive_big_endian.Size = new System.Drawing.Size(112, 17);
            this.checkBox_receive_big_endian.TabIndex = 12;
            this.checkBox_receive_big_endian.Text = "receive BigEndian";
            this.checkBox_receive_big_endian.UseVisualStyleBackColor = true;
            this.checkBox_receive_big_endian.CheckedChanged += new System.EventHandler(this.checkBox_receive_big_endian_CheckedChanged);
            // 
            // checkBox_send_big_endian
            // 
            this.checkBox_send_big_endian.AutoSize = true;
            this.checkBox_send_big_endian.Location = new System.Drawing.Point(9, 17);
            this.checkBox_send_big_endian.Name = "checkBox_send_big_endian";
            this.checkBox_send_big_endian.Size = new System.Drawing.Size(100, 17);
            this.checkBox_send_big_endian.TabIndex = 11;
            this.checkBox_send_big_endian.Text = "send BigEndian";
            this.checkBox_send_big_endian.UseVisualStyleBackColor = true;
            this.checkBox_send_big_endian.CheckedChanged += new System.EventHandler(this.checkBox_big_endian_CheckedChanged);
            // 
            // groupBox_send_msg
            // 
            this.groupBox_send_msg.Controls.Add(this.checkBox_ManagementData);
            this.groupBox_send_msg.Controls.Add(this.checkBox_acknowledge);
            this.groupBox_send_msg.Controls.Add(this.button_send_repeat);
            this.groupBox_send_msg.Controls.Add(this.button_send_request);
            this.groupBox_send_msg.Controls.Add(this.checkBox_SYNC);
            this.groupBox_send_msg.Controls.Add(this.label_send_times);
            this.groupBox_send_msg.Controls.Add(this.textBox_send);
            this.groupBox_send_msg.Controls.Add(this.button_check);
            this.groupBox_send_msg.Controls.Add(this.label_cyclic_desc);
            this.groupBox_send_msg.Controls.Add(this.checkBox_cyclic);
            this.groupBox_send_msg.Controls.Add(this.textBox_timer_interval);
            this.groupBox_send_msg.Controls.Add(this.label_repeat);
            this.groupBox_send_msg.Controls.Add(this.textBox_send_multiplikator);
            this.groupBox_send_msg.Location = new System.Drawing.Point(12, 207);
            this.groupBox_send_msg.Name = "groupBox_send_msg";
            this.groupBox_send_msg.Size = new System.Drawing.Size(313, 166);
            this.groupBox_send_msg.TabIndex = 18;
            this.groupBox_send_msg.TabStop = false;
            this.groupBox_send_msg.Text = "send message";
            // 
            // checkBox_ManagementData
            // 
            this.checkBox_ManagementData.AutoSize = true;
            this.checkBox_ManagementData.Location = new System.Drawing.Point(165, 47);
            this.checkBox_ManagementData.Name = "checkBox_ManagementData";
            this.checkBox_ManagementData.Size = new System.Drawing.Size(111, 17);
            this.checkBox_ManagementData.TabIndex = 29;
            this.checkBox_ManagementData.Text = "ManagementData";
            this.checkBox_ManagementData.UseVisualStyleBackColor = true;
            // 
            // checkBox_acknowledge
            // 
            this.checkBox_acknowledge.AutoSize = true;
            this.checkBox_acknowledge.Location = new System.Drawing.Point(70, 47);
            this.checkBox_acknowledge.Name = "checkBox_acknowledge";
            this.checkBox_acknowledge.Size = new System.Drawing.Size(90, 17);
            this.checkBox_acknowledge.TabIndex = 30;
            this.checkBox_acknowledge.Text = "acknowledge";
            this.checkBox_acknowledge.UseVisualStyleBackColor = true;
            // 
            // button_send_repeat
            // 
            this.button_send_repeat.Location = new System.Drawing.Point(177, 130);
            this.button_send_repeat.Name = "button_send_repeat";
            this.button_send_repeat.Size = new System.Drawing.Size(75, 23);
            this.button_send_repeat.TabIndex = 28;
            this.button_send_repeat.Text = "send";
            this.button_send_repeat.UseVisualStyleBackColor = true;
            this.button_send_repeat.Click += new System.EventHandler(this.button_send_repeat_Click);
            // 
            // checkBox_SYNC
            // 
            this.checkBox_SYNC.AutoSize = true;
            this.checkBox_SYNC.Location = new System.Drawing.Point(9, 47);
            this.checkBox_SYNC.Name = "checkBox_SYNC";
            this.checkBox_SYNC.Size = new System.Drawing.Size(55, 17);
            this.checkBox_SYNC.TabIndex = 28;
            this.checkBox_SYNC.Text = "SYNC";
            this.checkBox_SYNC.UseVisualStyleBackColor = true;
            // 
            // label_send_times
            // 
            this.label_send_times.AutoSize = true;
            this.label_send_times.Location = new System.Drawing.Point(91, 136);
            this.label_send_times.Name = "label_send_times";
            this.label_send_times.Size = new System.Drawing.Size(80, 13);
            this.label_send_times.TabIndex = 27;
            this.label_send_times.Text = "Frames at once";
            // 
            // button_check
            // 
            this.button_check.Location = new System.Drawing.Point(9, 97);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(112, 23);
            this.button_check.TabIndex = 26;
            this.button_check.Text = "check conection";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // label_cyclic_desc
            // 
            this.label_cyclic_desc.AutoSize = true;
            this.label_cyclic_desc.Location = new System.Drawing.Point(226, 102);
            this.label_cyclic_desc.Name = "label_cyclic_desc";
            this.label_cyclic_desc.Size = new System.Drawing.Size(20, 13);
            this.label_cyclic_desc.TabIndex = 25;
            this.label_cyclic_desc.Text = "ms";
            // 
            // checkBox_cyclic
            // 
            this.checkBox_cyclic.AutoSize = true;
            this.checkBox_cyclic.Location = new System.Drawing.Point(248, 101);
            this.checkBox_cyclic.Name = "checkBox_cyclic";
            this.checkBox_cyclic.Size = new System.Drawing.Size(53, 17);
            this.checkBox_cyclic.TabIndex = 24;
            this.checkBox_cyclic.Text = "cyclic";
            this.checkBox_cyclic.UseVisualStyleBackColor = true;
            this.checkBox_cyclic.CheckedChanged += new System.EventHandler(this.checkBox_cyclic_CheckedChanged);
            // 
            // textBox_timer_interval
            // 
            this.textBox_timer_interval.Location = new System.Drawing.Point(183, 99);
            this.textBox_timer_interval.Name = "textBox_timer_interval";
            this.textBox_timer_interval.Size = new System.Drawing.Size(37, 20);
            this.textBox_timer_interval.TabIndex = 23;
            this.textBox_timer_interval.Text = "1000";
            // 
            // label_repeat
            // 
            this.label_repeat.AutoSize = true;
            this.label_repeat.Location = new System.Drawing.Point(11, 137);
            this.label_repeat.Name = "label_repeat";
            this.label_repeat.Size = new System.Drawing.Size(33, 13);
            this.label_repeat.TabIndex = 22;
            this.label_repeat.Text = "send:";
            // 
            // textBox_send_multiplikator
            // 
            this.textBox_send_multiplikator.Location = new System.Drawing.Point(50, 133);
            this.textBox_send_multiplikator.Name = "textBox_send_multiplikator";
            this.textBox_send_multiplikator.Size = new System.Drawing.Size(31, 20);
            this.textBox_send_multiplikator.TabIndex = 21;
            this.textBox_send_multiplikator.Text = "1";
            // 
            // radioButton_send_ascii
            // 
            this.radioButton_send_ascii.AutoSize = true;
            this.radioButton_send_ascii.Location = new System.Drawing.Point(164, 36);
            this.radioButton_send_ascii.Name = "radioButton_send_ascii";
            this.radioButton_send_ascii.Size = new System.Drawing.Size(72, 17);
            this.radioButton_send_ascii.TabIndex = 14;
            this.radioButton_send_ascii.Text = "send ascii";
            this.radioButton_send_ascii.UseVisualStyleBackColor = true;
            // 
            // radioButton_send_byte
            // 
            this.radioButton_send_byte.AutoSize = true;
            this.radioButton_send_byte.Checked = true;
            this.radioButton_send_byte.Location = new System.Drawing.Point(164, 16);
            this.radioButton_send_byte.Name = "radioButton_send_byte";
            this.radioButton_send_byte.Size = new System.Drawing.Size(97, 17);
            this.radioButton_send_byte.TabIndex = 13;
            this.radioButton_send_byte.TabStop = true;
            this.radioButton_send_byte.Text = "send byte array";
            this.radioButton_send_byte.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "label1";
            // 
            // groupBox_status
            // 
            this.groupBox_status.Controls.Add(this.label_frameLog_answer);
            this.groupBox_status.Controls.Add(this.label3);
            this.groupBox_status.Controls.Add(this.textBox_msg_payload_hex);
            this.groupBox_status.Controls.Add(this.label_frameMetadata);
            this.groupBox_status.Controls.Add(this.label2);
            this.groupBox_status.Controls.Add(this.label_frameLog);
            this.groupBox_status.Controls.Add(this.textBox_msg_payload_int);
            this.groupBox_status.Controls.Add(this.listBox_frameLog);
            this.groupBox_status.Controls.Add(this.label_payload_ascii);
            this.groupBox_status.Controls.Add(this.label_payload_byte);
            this.groupBox_status.Controls.Add(this.textBox_msg_payload_ASCII);
            this.groupBox_status.Controls.Add(this.textBox_msg_payload_byte);
            this.groupBox_status.Location = new System.Drawing.Point(331, 12);
            this.groupBox_status.Name = "groupBox_status";
            this.groupBox_status.Size = new System.Drawing.Size(894, 471);
            this.groupBox_status.TabIndex = 24;
            this.groupBox_status.TabStop = false;
            this.groupBox_status.Text = "status";
            // 
            // label_frameLog_answer
            // 
            this.label_frameLog_answer.AutoSize = true;
            this.label_frameLog_answer.Location = new System.Drawing.Point(476, 268);
            this.label_frameLog_answer.Name = "label_frameLog_answer";
            this.label_frameLog_answer.Size = new System.Drawing.Size(119, 13);
            this.label_frameLog_answer.TabIndex = 28;
            this.label_frameLog_answer.Text = "label_frameLog_answer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(476, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "payload hex:";
            // 
            // textBox_msg_payload_hex
            // 
            this.textBox_msg_payload_hex.Location = new System.Drawing.Point(553, 60);
            this.textBox_msg_payload_hex.Name = "textBox_msg_payload_hex";
            this.textBox_msg_payload_hex.Size = new System.Drawing.Size(335, 20);
            this.textBox_msg_payload_hex.TabIndex = 26;
            // 
            // label_frameMetadata
            // 
            this.label_frameMetadata.AutoSize = true;
            this.label_frameMetadata.Location = new System.Drawing.Point(6, 16);
            this.label_frameMetadata.Name = "label_frameMetadata";
            this.label_frameMetadata.Size = new System.Drawing.Size(106, 13);
            this.label_frameMetadata.TabIndex = 25;
            this.label_frameMetadata.Text = "label_frameMetadata";
            // 
            // label_frameLog
            // 
            this.label_frameLog.AutoSize = true;
            this.label_frameLog.Location = new System.Drawing.Point(476, 145);
            this.label_frameLog.Name = "label_frameLog";
            this.label_frameLog.Size = new System.Drawing.Size(79, 13);
            this.label_frameLog.TabIndex = 24;
            this.label_frameLog.Text = "label_frameLog";
            // 
            // listBox_frameLog
            // 
            this.listBox_frameLog.FormattingEnabled = true;
            this.listBox_frameLog.Location = new System.Drawing.Point(5, 39);
            this.listBox_frameLog.Name = "listBox_frameLog";
            this.listBox_frameLog.Size = new System.Drawing.Size(465, 420);
            this.listBox_frameLog.TabIndex = 23;
            this.listBox_frameLog.SelectedIndexChanged += new System.EventHandler(this.listBox_frameLog_SelectedIndexChanged);
            // 
            // menuStrip_main
            // 
            this.menuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logFileToolStripMenuItem});
            this.menuStrip_main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_main.Name = "menuStrip_main";
            this.menuStrip_main.Size = new System.Drawing.Size(1237, 24);
            this.menuStrip_main.TabIndex = 25;
            this.menuStrip_main.Text = "menuStrip1";
            // 
            // logFileToolStripMenuItem
            // 
            this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
            this.logFileToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.logFileToolStripMenuItem.Text = "log file";
            this.logFileToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.tssl_server_status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 553);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1237, 22);
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopServerToolStripMenuItem,
            this.startServerToolStripMenuItem,
            this.toolStripSeparator1,
            this.statusToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownButton1.Text = "tsddb_udp_server";
            // 
            // stopServerToolStripMenuItem
            // 
            this.stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            this.stopServerToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.stopServerToolStripMenuItem.Text = "stop server";
            this.stopServerToolStripMenuItem.Click += new System.EventHandler(this.stopServerToolStripMenuItem_Click);
            // 
            // startServerToolStripMenuItem
            // 
            this.startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            this.startServerToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.startServerToolStripMenuItem.Text = "start server";
            this.startServerToolStripMenuItem.Click += new System.EventHandler(this.startServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.statusToolStripMenuItem.Text = "status";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // tssl_server_status
            // 
            this.tssl_server_status.Name = "tssl_server_status";
            this.tssl_server_status.Size = new System.Drawing.Size(72, 17);
            this.tssl_server_status.Text = "server status";
            this.tssl_server_status.ToolTipText = "ttt";
            // 
            // groupBox_settings
            // 
            this.groupBox_settings.Controls.Add(this.checkBox_SendOnlyIfConnected);
            this.groupBox_settings.Controls.Add(this.checkBox_SendFramesCallback);
            this.groupBox_settings.Controls.Add(this.textBox_WATCHDOG_WORK);
            this.groupBox_settings.Controls.Add(this.label_WATCHDOG_WORK);
            this.groupBox_settings.Controls.Add(this.textBox_MaxSYNCResendTrys);
            this.groupBox_settings.Controls.Add(this.label_MaxSYNCResendTrys_desc);
            this.groupBox_settings.Controls.Add(this.radioButton_send_ascii);
            this.groupBox_settings.Controls.Add(this.radioButton_send_byte);
            this.groupBox_settings.Controls.Add(this.checkBox_receive_big_endian);
            this.groupBox_settings.Controls.Add(this.checkBox_send_big_endian);
            this.groupBox_settings.Location = new System.Drawing.Point(12, 379);
            this.groupBox_settings.Name = "groupBox_settings";
            this.groupBox_settings.Size = new System.Drawing.Size(313, 157);
            this.groupBox_settings.TabIndex = 26;
            this.groupBox_settings.TabStop = false;
            this.groupBox_settings.Text = "settings";
            // 
            // checkBox_SendOnlyIfConnected
            // 
            this.checkBox_SendOnlyIfConnected.AutoSize = true;
            this.checkBox_SendOnlyIfConnected.Location = new System.Drawing.Point(141, 134);
            this.checkBox_SendOnlyIfConnected.Name = "checkBox_SendOnlyIfConnected";
            this.checkBox_SendOnlyIfConnected.Size = new System.Drawing.Size(130, 17);
            this.checkBox_SendOnlyIfConnected.TabIndex = 20;
            this.checkBox_SendOnlyIfConnected.Text = "SendOnlyIfConnected";
            this.checkBox_SendOnlyIfConnected.UseVisualStyleBackColor = true;
            this.checkBox_SendOnlyIfConnected.CheckedChanged += new System.EventHandler(this.checkBox_SendOnlyIfConnected_CheckedChanged);
            // 
            // checkBox_SendFramesCallback
            // 
            this.checkBox_SendFramesCallback.AutoSize = true;
            this.checkBox_SendFramesCallback.Location = new System.Drawing.Point(9, 134);
            this.checkBox_SendFramesCallback.Name = "checkBox_SendFramesCallback";
            this.checkBox_SendFramesCallback.Size = new System.Drawing.Size(126, 17);
            this.checkBox_SendFramesCallback.TabIndex = 19;
            this.checkBox_SendFramesCallback.Text = "SendFramesCallback";
            this.checkBox_SendFramesCallback.UseVisualStyleBackColor = true;
            this.checkBox_SendFramesCallback.CheckedChanged += new System.EventHandler(this.checkBox_SendFramesCallback_CheckedChanged);
            // 
            // textBox_WATCHDOG_WORK
            // 
            this.textBox_WATCHDOG_WORK.Location = new System.Drawing.Point(126, 102);
            this.textBox_WATCHDOG_WORK.Name = "textBox_WATCHDOG_WORK";
            this.textBox_WATCHDOG_WORK.Size = new System.Drawing.Size(57, 20);
            this.textBox_WATCHDOG_WORK.TabIndex = 18;
            this.textBox_WATCHDOG_WORK.TextChanged += new System.EventHandler(this.textBox_WATCHDOG_WORK_TextChanged);
            // 
            // label_WATCHDOG_WORK
            // 
            this.label_WATCHDOG_WORK.AutoSize = true;
            this.label_WATCHDOG_WORK.Location = new System.Drawing.Point(6, 105);
            this.label_WATCHDOG_WORK.Name = "label_WATCHDOG_WORK";
            this.label_WATCHDOG_WORK.Size = new System.Drawing.Size(114, 13);
            this.label_WATCHDOG_WORK.TabIndex = 17;
            this.label_WATCHDOG_WORK.Text = "WATCHDOG_WORK:";
            // 
            // textBox_MaxSYNCResendTrys
            // 
            this.textBox_MaxSYNCResendTrys.Location = new System.Drawing.Point(128, 67);
            this.textBox_MaxSYNCResendTrys.Name = "textBox_MaxSYNCResendTrys";
            this.textBox_MaxSYNCResendTrys.Size = new System.Drawing.Size(55, 20);
            this.textBox_MaxSYNCResendTrys.TabIndex = 16;
            this.textBox_MaxSYNCResendTrys.TextChanged += new System.EventHandler(this.textBox_MaxSYNCResendTrys_TextChanged);
            // 
            // label_MaxSYNCResendTrys_desc
            // 
            this.label_MaxSYNCResendTrys_desc.AutoSize = true;
            this.label_MaxSYNCResendTrys_desc.Location = new System.Drawing.Point(6, 70);
            this.label_MaxSYNCResendTrys_desc.Name = "label_MaxSYNCResendTrys_desc";
            this.label_MaxSYNCResendTrys_desc.Size = new System.Drawing.Size(116, 13);
            this.label_MaxSYNCResendTrys_desc.TabIndex = 15;
            this.label_MaxSYNCResendTrys_desc.Text = "MaxSYNCResendTrys:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(353, 495);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 27;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmCPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 575);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox_settings);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox_status);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox_client);
            this.Controls.Add(this.groupBox_send_msg);
            this.Controls.Add(this.menuStrip_main);
            this.MainMenuStrip = this.menuStrip_main;
            this.Name = "FrmCPS";
            this.Text = "CpuPcStack - PLCSourceFile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.groupBox_client.ResumeLayout(false);
            this.groupBox_client.PerformLayout();
            this.groupBox_send_msg.ResumeLayout(false);
            this.groupBox_send_msg.PerformLayout();
            this.groupBox_status.ResumeLayout(false);
            this.groupBox_status.PerformLayout();
            this.menuStrip_main.ResumeLayout(false);
            this.menuStrip_main.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox_settings.ResumeLayout(false);
            this.groupBox_settings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.GroupBox groupBox_client;
        private System.Windows.Forms.TextBox textBox_remotePort;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox textBox_remote_ip;
        private System.Windows.Forms.Label label_srv_port;
        private System.Windows.Forms.TextBox textBox_srv_port;
        private System.Windows.Forms.TextBox textBox_send;
        private System.Windows.Forms.Button button_send_request;
        private System.Windows.Forms.ComboBox comboBox_local_ips;
        private System.Windows.Forms.Label label_host_name;
        private System.Windows.Forms.Label label_host_name_desc;
        private System.Windows.Forms.CheckBox checkBox_send_big_endian;
        private System.Windows.Forms.CheckBox checkBox_receive_big_endian;
        private System.Windows.Forms.GroupBox groupBox_send_msg;
        private System.Windows.Forms.TextBox textBox_msg_payload_byte;
        private System.Windows.Forms.TextBox textBox_msg_payload_ASCII;
        private System.Windows.Forms.Label label_payload_ascii;
        private System.Windows.Forms.Label label_payload_byte;
        private System.Windows.Forms.RadioButton radioButton_send_ascii;
        private System.Windows.Forms.RadioButton radioButton_send_byte;
        private System.Windows.Forms.TextBox textBox_send_multiplikator;
        private System.Windows.Forms.Label label_repeat;
        private System.Windows.Forms.Label label_cyclic_desc;
        private System.Windows.Forms.CheckBox checkBox_cyclic;
        private System.Windows.Forms.TextBox textBox_timer_interval;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_send_repeat;
        private System.Windows.Forms.Label label_send_times;
        private System.Windows.Forms.GroupBox groupBox_status;
        private System.Windows.Forms.MenuStrip menuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_msg_payload_int;
        private System.Windows.Forms.ListBox listBox_frameLog;
        private System.Windows.Forms.Label label_frameLog;
        private System.Windows.Forms.Label label_frameMetadata;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem stopServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tssl_server_status;
        private System.Windows.Forms.GroupBox groupBox_settings;
        private System.Windows.Forms.Label label_WATCHDOG_WORK;
        private System.Windows.Forms.TextBox textBox_MaxSYNCResendTrys;
        private System.Windows.Forms.Label label_MaxSYNCResendTrys_desc;
        private System.Windows.Forms.TextBox textBox_WATCHDOG_WORK;
        private System.Windows.Forms.CheckBox checkBox_SendOnlyIfConnected;
        private System.Windows.Forms.CheckBox checkBox_SendFramesCallback;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_SYNC;
        private System.Windows.Forms.CheckBox checkBox_ManagementData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_msg_payload_hex;
        private System.Windows.Forms.CheckBox checkBox_acknowledge;
        private System.Windows.Forms.Label label_frameLog_answer;
    }
}

