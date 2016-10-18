ChangeFrameStruct
<h1>CpuPcStack</h1>
udp schnittstelle zwischen pc und cpu

<h2>Beschreibung</h2>
<p>
Dieses Tool ist eine Schnittstelle zwischen PCs und Siemens CPUs (12xx) via Ethernet.
Die Schnittstelle setzt auf udp/ip auf.
Ziel dieses Projekts ist es eine generische Schnittstelle zwischen PCs und CPUs zu realisieren.
</p>

<h2>PC Seite</h2>
<p>
Auf PC Seite ist die Schnittstelle als Windows.Forms .NET Applikation realisiert.</br>
In überlagerten Applikationen soll diese Schnittstelle einfach integrierbar sein.</br>
Referenz auf cpsLIB anlegen</br>
Interface verwenden: public partial class FrmMain : Form, IcpsLIB{}</br>
Instanz erstellen: UdpPlcLIB.net_udp net_udp = new UdpPlcLIB.net_udp(this);</br>
                                                                                </br>
        private delegate void srv_msgCallback(string s);                             </br>
        void IcpsLIB.logMsg(string msg)                                              </br>
        {                                                                            </br>
            try                                                                      </br>
            {                                                                        </br>
                this.Invoke(new srv_msgCallback(this.srv_msg_funkt), new object[] { msg });</br>
            }                                                                         </br>
            catch (Exception e)                                                       </br>
            {                                                                         </br>
                MessageBox.Show("srv_msgCallback: " + e.Message, "writing to GUI failed"); </br>
            }                                                                         </br>
        }                                                                             </br>
        private void srv_msg_funkt(string s)                                          </br>
        {                                                                             </br>
            tssl_server_status.Text = s;                                              </br>
        }                                                                             </br>
        #endregion                                                                    </br>
                                                                                      </br>
        #region interprete_frame                                                      </br>
        private delegate void interprete_frameCallback(object f);                     </br>
        void IcpsLIB.interprete_frame(object o)                                       </br>
        {                                                                             </br>
            try                                                                       </br>
            {                                                                         </br>
                if (this.InvokeRequired)                                              </br>
                    this.Invoke(new interprete_frameCallback(this.interprete_frame_fkt), new object[] { o });</br>
                else                                                                  </br>
                    interprete_frame_fkt(o);                                          </br>
            }                                                                         </br>
            catch (Exception e)                                                       </br>
            {                                                                         </br>
                MessageBox.Show("interprete_frameCallback: " + e.Message, "writing to GUI failed");</br>
            }                                                                         </br>
        }                                                                             </br>
</p>                                                                                  

<h2>CPU Seite</h2>
<p>
In der CPU gibt es zwei Global DBs (ein Empfangs- und ein Sendepuffer) die aus dem Anwenderprogramm angesprochen werden.
Im Ordner plc_sourcen sind die notwendigen CPU Programmbausteine als SCL Code hinterlegt.
Diese Dateien müssen mittels des Tools "TIA Portal" projektiert und in die CPU geladen werden.
</p>

