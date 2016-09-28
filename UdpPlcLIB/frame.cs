using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace UdpPlcLIB
{
    public enum FrameType { DEMO, SYNC}
    public enum FrameWorkingState { created, inWork, done, error, error_double, error_sendTrysLimit, error_send, warning_resend, received, send}
    /// <summary>
    /// Telegram;
    /// 4*Char [type]; 1*Int16 [index]; x*byte [payload]
    /// 
    /// type => art des frames. z.b. SYNC zum verifizieren einer verbindung
    /// </summary>
    public class FrameRawData
    {
        /// <summary>
        /// frame content
        /// </summary>
        public string _type;
        public Int16 _index;
        private byte[] FrameData; //the total frame data (including type and index)
        private byte[] _FrameData {
            get { return FrameData; }
            set {
                FrameData = value;
                _FramePayloadByte = new byte[FrameData.Length - (TYPE_LENGTH + INDEX_LENGTH)];
                System.Buffer.BlockCopy(FrameData, TYPE_LENGTH + INDEX_LENGTH, _FramePayloadByte, 0, _FramePayloadByte.Length);
                _FramePayload = GetIntArr(_FramePayloadByte);
                _FrameLength = FrameData.Length;
            }
        }
        private Int16[] _FramePayload; //frame data as INT without type/index
        private byte[] _FramePayloadByte;
        private int _FrameLength;

        /// <summary>
        /// frame content description
        /// </summary>
        private const int TYPE_LENGTH = 4; //Bytes used for ascii _type
        private const int INDEX_LENGTH = 2; //Bytes used for index (int)
        public static bool SendBigEndian = false; //PC = Little-Endian, CPU = Big-Endian
        public static bool ReceiveBigEndian = false;

        /// <summary>
        /// frame meta data
        /// </summary>
        public string RemoteIp = null;
        public int RemotePort;
        public DateTime TimeCreated; //Zeitstempel an dem das Frame erzeugt wurde
        private FrameWorkingState WorkingState;
        private string WorkingStateMessage; //Log Messages zu dem Frame
        public DateTime LastSendDateTime; //Zeitstempel an dem das Frame zuletzt versendet wurde
        public int SendTrys = 0;
        public int index_send;

        /// <summary>
        /// static meta data
        /// </summary>
        public static int CountSendFrames = 0;
        public static int CountRcvFrames = 0;
              
        #region frames_payload NOT_USED
        public static Int16[] GET_STATE(int index) { return new Int16[] { Convert.ToInt16(index), 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] GET_PARAM(int index) { return new Int16[] { Convert.ToInt16(index), 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] SET_STATE(int index, string position, string angle) { return new Int16[] { Convert.ToInt16(index), 2, Convert.ToInt16(position), Convert.ToInt16(angle), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] SET_STATE(int index, bool state_switch) { return new Int16[] { Convert.ToInt16(index), 2, Convert.ToInt16(state_switch), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Frame FRAME_SYNC(Int16 index, string ip, string port) {
            return new Frame(FrameType.SYNC.ToString(), index, ip, port);
            /*
             Frame f = new Frame(FrameType.SYNC.ToString(), check_trys, textBox_remote_ip.Text, textBox_remotePort.Text);
             */ 
        }
        #endregion

        #region construktor
        /// <summary>
        /// make new frame object from rcv UDP Frame
        /// </summary>
        /// <param name="data">actual message</param>
        /// <param name="ip">ip from remote sender</param>
        /// <param name="port">port from remote sender</param>
        public FrameRawData(byte[] data, string ip, string port)
        {
            CountRcvFrames++;
            TimeCreated = DateTime.Now;
            WorkingState = FrameWorkingState.created;
            RemoteIp = ip;
            if(int.TryParse(port, out RemotePort))
            {
                if (ReceiveBigEndian)
                    _FrameData = changeEndian(data);
                else
                    _FrameData = data;

                if (_FrameData.Length >= TYPE_LENGTH)
                    _type = Encoding.ASCII.GetString(_FrameData.Take<byte>(TYPE_LENGTH).ToArray());
                else
                    WorkingState = FrameWorkingState.error;

                if (_FrameData.Length >= TYPE_LENGTH + INDEX_LENGTH)
                    _index = BitConverter.ToInt16(_FrameData.Skip<byte>(TYPE_LENGTH).Take<byte>(INDEX_LENGTH).ToArray(), 0);
                else
                    WorkingState = FrameWorkingState.error;
            }
            else
                WorkingState = FrameWorkingState.error;
        }


        /// <summary>
        /// make new frame object to send it later on 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public FrameRawData(string type, Int16 index, byte[] bdata, string ip, string port)
        {
            CountSendFrames++;
            index_send = CountSendFrames;
            TimeCreated = DateTime.Now;
            WorkingState = FrameWorkingState.created;
            RemoteIp = ip;
            _type = type; //zwischenspeichern bisher nicht notwendig
            _index = index; //zwischenspeichern bisher nicht notwendig        

            if (int.TryParse(port, out RemotePort))
            {
                byte[] btype = Encoding.ASCII.GetBytes(type);
                byte[] bindex = BitConverter.GetBytes(index);

                byte[] rv = new byte[btype.Length + bindex.Length + bdata.Length];
                System.Buffer.BlockCopy(btype, 0, rv, 0, btype.Length);
                System.Buffer.BlockCopy(bindex, 0, rv, btype.Length, bindex.Length);
                System.Buffer.BlockCopy(bdata, 0, rv, bindex.Length + btype.Length, bdata.Length);

                if (SendBigEndian)
                    _FrameData = changeEndian(rv);
                else
                    _FrameData = rv;
            }
            else
                WorkingState = FrameWorkingState.error;
        }
        #endregion

        #region functions
        private byte[] changeEndian(byte[] data)
        {
            byte[] newData = new byte[data.Length];
            for (int i = 0; i < newData.Length / 2; i++)
            {
                newData[i * 2] = data[(i * 2) + 1];
                newData[(i * 2) + 1] = data[i * 2];
            }
            return newData;
        }

        //private Int16[] changeEndian(Int16[] data)
        //{
        //    Int16[] tmp_data = new Int16[data.Length];
        //    for (int i = 0; i < data.Length; i++)
        //        tmp_data[i] = IPAddress.NetworkToHostOrder(data[i]);
        //    return tmp_data;
        //}

        private Int16[] GetIntArr(byte[] data)
        {
            Int16[] intData = new Int16[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
                intData[i] = BitConverter.ToInt16(data, i * 2);
            return intData;
        }

        public byte[] bytes()
        {
            return _FrameData;
        }

        public int length()
        {
            return _FrameData.Length;
        }

        public string getPayload() {
            string s = string.Empty;
            
            for (int i = 0; i < _FramePayload.Length; i++)
                s += _FramePayload[i].ToString() + ", ";
            return s;
        }

        public string getPayloadASCII()
        {
            return new string(Encoding.ASCII.GetString(_FramePayloadByte).ToCharArray());
        }

        public override string ToString()
        {
            string s = string.Empty;
            Int16[] data = GetIntArr(_FrameData);
            for (int i = 0; i < data.Length; i++)
                s += data[i].ToString() + ", ";
            return s;
        }

        public string GetDetailedString() {
            return "[" + RemoteIp + ":" + RemotePort + " " + TimeCreated.ToString("HH:mm:ss:ffffff") + " " + _type + " " + _index + "] " +
                " (" + WorkingState.ToString() + ") " + WorkingStateMessage + " {" + this.ToString() + "}";
        }

        /// <summary>
        /// vergleicht die rohdaten zweier frames
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        //public bool isEqual(FrameRawData f)
        //{
        //    for (int i = 0; i < _FrameData.Length; i++)
        //        if (_FrameData[i] != f._FrameData[i])
        //            return false;
        //    return true;
        //}

        /// <summary>
        /// vergleicht den type und payload zweier frames auf gleichheit.
        /// </summary>
        /// <param name="f"></param>
        /// <returns>bei gleich TRUE; bei unterschiedlich FALSE</returns>
        public bool isEqualExeptIndex(Frame f)
        {
            log.msg(this, "isEqualExeptIndex: " + f.GetDetailedString());
            //if (f.RemoteIp.Equals(RemoteIp) && f._type.Equals(_type))//Beide Frames haben gleichen Type und gleiche Remote IP Adresse
            if (f.RemoteIp.Equals(RemoteIp))
            {
                if (f._type.Equals(_type))
                {
                    if (_FramePayloadByte != null)
                    {
                        if (_FramePayloadByte.Length == f._FramePayloadByte.Length)
                        {
                            for (int i = 0; i < _FramePayloadByte.Length; i++)
                                if (_FramePayloadByte[i] != f._FramePayloadByte[i])
                                    return false;
                            return true;
                        }
                        else
                        {
                            log.msg(this, "length payload <>");
                            return false;
                        }
                    }
                    else if (f._FramePayloadByte == null)//Beide Frames haben kein Payload
                        return true;
                    else
                    {
                        log.msg(this, "payload == null, f.payload != null");
                        return false;
                    }
                }
                log.msg(this, "unterschiedlicher type (" + _type + ")");
                return false;
            }
            log.msg(this, "unterschiedliche IP ("+ RemoteIp + ")");
            return false;
        }

        /// <summary>
        /// frame ändert seinen status
        /// änderung wird protokolliert
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public FrameRawData ChangeState(FrameWorkingState ws, string msg) {
            WorkingState = ws;
            WorkingStateMessage = msg;
            
            //schreibe in log datei
            log.msg(this, GetDetailedString());

            //TODO: schreibe in GUI
            //oder besser in eine liste des frames mit - id, timestamp, WorkingState, WorkingStateMessage 
            return this;
        }
        #endregion

        #region funktions unused
        /// <summary>
        /// returns Frame als ByteArray in BigEndian (PLC Order)
        /// </summary>
        /// <returns></returns>
        /*
        public byte[] getNetworkOrderFrame()
        {
            try
            {
                byte[] network_byte_array = new byte[_data_HostOrder.Length * 2];

                for (int i = 0; i < _data_HostOrder.Length; i++)
                {
                    byte[] tmp_bytes = BitConverter.GetBytes(_data_HostOrder[i]);
                    network_byte_array[i * 2] = tmp_bytes[1];
                    network_byte_array[(i * 2) + 1] = tmp_bytes[0];
                }

                return network_byte_array;
            }
            catch (Exception e)
            {
                throw new Exception("Exception getNetworkOrderFrame: " + e.Message);
            }

        public bool isIndex(int index)
        {
            if (index == _data_HostOrder[0])
                return true;
            else
                return false;
        }

        public int getJob()
        {
            return _data_HostOrder[1];
        }

        public int getPayload(int index)
        {
            if (index < _data_HostOrder.Length)
                return _data_HostOrder[index]; //############################## [index + 2]
            else
                return -1;
        }



        public bool isEmptyString()
        {
            foreach (Int16 i in _data_HostOrder)
                if (i != 0x2020)
                    return false;
            return true;
        }

        public bool isEqual(Frame f)
        {
            for (int i = 0; i < _data_HostOrder.Length; i++)
                if (_data_HostOrder[i] != f._data_HostOrder[i])
                    return false;
            return true;
        }
         * */
        #endregion
    }

    public enum FrameSender { client, server , unknown}
    public class  Frame : FrameRawData{
        public FrameSender sender = FrameSender.unknown;
        /// <summary>
        /// sync frame ohne content
        /// </summary>
        public Frame(string type, Int16 index, string ip, string port) : 
            base(type, index, new byte []{}, ip, port){}

        /// <summary>
        /// normale frames die versendet werden
        /// </summary>
        public Frame(string type, Int16 index, Int16[] data, string ip, string port) :
            base(type, index, getByteArray(data), ip, port) { }

        public Frame(string type, Int16 index, char[] data, string ip, string port) :
            base(type, index, getByteArray(data), ip, port){}

        /// <summary>
        /// frame das von stream über udp empfangen wird
        /// </summary>
        public Frame(byte[] data, string ip, string port) :
            base(data, ip, port) { }


        /// <summary>
        /// funktionen
        /// </summary>
        private static byte[] getByteArray(char[] data)
        {
            byte[] bdata = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                bdata[i] = Convert.ToByte(data[i]);

            return bdata;
        }

        private static byte[] getByteArray(Int16[] data)
        {
            byte[] byte_array = new byte[data.Length * 2];

            for (int i = 0; i < data.Length; i++)
            {
                byte[] tmp_bytes = BitConverter.GetBytes(data[i]);
                byte_array[i * 2] = tmp_bytes[0];
                byte_array[(i * 2) + 1] = tmp_bytes[1];
            }
            return byte_array;
        }

        public void _sendBigEndian(bool _BigEndian) { 
            Frame.SendBigEndian = _BigEndian;
        }
     

    }
}
