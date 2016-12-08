using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cpsLIB
{
    public partial class FrmStatusLog : Form
    {
        BindingList<log> ListLogFrontend;
        List<log> ListLogBackend;

        #region var
        private bool AutoScrollonUpdate = true;
        private bool UpdateGUIonNewEvent = true;
        #endregion

        public DataGridViewCellEventHandler dataGridView1_CellClick { get; set; }

        #region construktor
        public FrmStatusLog()
        {
            InitializeComponent();
            ListLogFrontend = new BindingList<log>();
            ListLogBackend = new List<log>();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = ListLogFrontend;

            //dGV_Log.Dock = DockStyle.Bottom;
            dGV_Log.AutoGenerateColumns = false;
            AddColumns();

            dGV_Log.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dGV_Log.DataSource = bindingSource;
        }

        private void AddColumns()
        {
            DataGridViewTextBoxColumn DGVtbtimestamp = new DataGridViewTextBoxColumn();
            DGVtbtimestamp.Name = "Time";
            DGVtbtimestamp.DataPropertyName = "Timestamp";
            DGVtbtimestamp.ReadOnly = true;
            //DGVtbtimestamp.ValueType = typeof(DateTime);
            dGV_Log.Columns.Add(DGVtbtimestamp);
            
            DataGridViewTextBoxColumn DGVtbcPrio = new DataGridViewTextBoxColumn();
            DGVtbcPrio.Name = "Level";
            DGVtbcPrio.DataPropertyName = "Prio";
            DGVtbcPrio.ReadOnly = true;
            dGV_Log.Columns.Add(DGVtbcPrio);

            DataGridViewTextBoxColumn DGVtbMsg = new DataGridViewTextBoxColumn();
            DGVtbMsg.Name = "Message";
            DGVtbMsg.DataPropertyName = "Msg";
            //DGVtbMsg.ValueType = typeof(string);
            dGV_Log.Columns.Add(DGVtbMsg);

            DataGridViewTextBoxColumn DGVtbcKey = new DataGridViewTextBoxColumn();
            DGVtbcKey.Name = "IP:INDEX";
            DGVtbcKey.DataPropertyName = "Key";
            DGVtbcKey.ReadOnly = true;
            dGV_Log.Columns.Add(DGVtbcKey);

            DataGridViewTextBoxColumn DGVtbcF = new DataGridViewTextBoxColumn();
            DGVtbcF.Name = "Frame";
            DGVtbcF.DataPropertyName = "F";
            DGVtbcF.ReadOnly = true;
            dGV_Log.Columns.Add(DGVtbcF);
            
            DataGridViewTextBoxColumn DGVtbcHeader = new DataGridViewTextBoxColumn();
            DGVtbcHeader.Name = "header";
            DGVtbcHeader.DataPropertyName = "Header";
            DGVtbcHeader.ReadOnly = true;
            dGV_Log.Columns.Add(DGVtbcHeader);

            DataGridViewTextBoxColumn DGVtbcPayload = new DataGridViewTextBoxColumn();
            DGVtbcPayload.Name = "payload";
            DGVtbcPayload.DataPropertyName = "Payload";
            DGVtbcPayload.ReadOnly = true;
            dGV_Log.Columns.Add(DGVtbcPayload);
        }
#endregion

        private void FrmStatusLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        #region menue
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListLogFrontend.Clear();
        }
        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListLogFrontend.Clear();
            foreach (log l in ListLogBackend)
                ListLogFrontend.Add(l);
        }

        private void autoScrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoScrollToolStripMenuItem.Text == "auto scroll [on]")
            {
                AutoScrollonUpdate = false;
                autoScrollToolStripMenuItem.Text = "auto scroll [off]";
            }
            else
            {
                AutoScrollonUpdate = true;
                autoScrollToolStripMenuItem.Text = "auto scroll [on]";
            }
        }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (freezeToolStripMenuItem.Text == "freeze [on]")
            {
                UpdateGUIonNewEvent = false;
                freezeToolStripMenuItem.Text = "freeze [off]";
            }
            else
            {
                UpdateGUIonNewEvent = true;
                freezeToolStripMenuItem.Text = "freeze [on]";
            }
        }
        #endregion

        /// <summary>
        /// log/error messages from udp server
        /// </summary>
        /// <param name="s"></param>
        private delegate void CpsLogCallback(log msg);
        public void AddLog(log msg)
        {
            try
            {
                this.Invoke(new CpsLogCallback(this.logMsg), new object[] { msg });
            }
            catch (Exception e)
            {
                //form closing throws exeption -> TODO catch
                //log.exception(this, "srv_msgCallback: writing to GUI failed", e);
                MessageBox.Show("CpsLogCallback: " + e.Message, "writing to GUI failed");
            }
        }

        private void logMsg(log _log)
        {
            //+++++++++++++++++++ richTextBox ++++++++++++++++++++++++
            //string msg = _log.Timestamp + " [" + _log.Prio.ToString() + "] ";
            //if (_log.Msg != null)
            //    msg += _log.Msg;
            //if (_log.F != null)
            //    msg += _log.F;
            //rTB_log_msg.AppendText(msg + Environment.NewLine);

            //+++++++++++++++++++ DataGridView ++++++++++++++++++++++++
            ListLogBackend.Add(_log);

            if (UpdateGUIonNewEvent)
                AddLogFilter(_log);
            
        }

        private void AddLogFilter(log l) {

            //if(!cBshowInfo.Checked && !l.Prio.Equals(prio.info))
                ListLogFrontend.Add(l);

                if (AutoScrollonUpdate)
                    dGV_Log.FirstDisplayedScrollingRowIndex = dGV_Log.RowCount - 1;
        }





    }
}
