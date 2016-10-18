using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

/// <summary>
/// Telegram Structure:
/// 1 byte StructVersion; 1 byte ByteHeaderFlag; 2 byte reserviert; int16 FrameIndex; byte[x] payload
    
namespace cpsLIB
{
    public enum FrameSender { SEND, RCVE, unknown }
    public enum FrameState { ERROR, IS_OK}
    public enum FrameWorkingState { created, inWork, finish, error, warning, received, send}

    //order of the flags must match with the Remote FrameHeaderFlag
    public enum FrameHeaderFlag { containering = 0, SYNC, LogMessage, acknowledge, ProcessData_value, ProcessData_param, ManagementData}
    public enum HeaderFlagManagementData { GetTime = 1, SetTime = 2 } 

    public class FrameRawData
    {
        #region vars
        //frame content
        private FrameHeader header;
        private byte[] FramePayloadByte;

        //public string RemoteIp = null;
        //public int RemotePort;

        // connection depending to frame
        public CpsClient client;

        // frame meta data
        public DateTime TimeCreated; //Zeitstempel an dem das Frame erzeugt wurde

        public FrameSender frameSender = FrameSender.unknown;
        private List<frameLog> ListFrameLog;
        public FrameState frameState = FrameState.IS_OK;
        public int SendTrys; //Wird bei FrameType.SYNC Frames verwendet. Anzahl der wiederholungen bei keiner antwort
        public DateTime LastSendDateTime;
        public TimeSpan TimeRcvAnswer; //wird beim empfangen einer antwort gesetzt
        
        
        // static meta data
        public static bool _RemoteIsBigEndian = true; //PC = Little-Endian, CPU = Big-Endian

        #endregion

        #region construktor
        public FrameRawData(CpsClient _client, byte[] data, FrameSender FS)
        {
            this.client = _client;
            ListFrameLog = new List<frameLog>();
            TimeCreated = DateTime.Now;
            LastSendDateTime = DateTime.Now;
            frameSender = FS;
            //RemoteIp = ip;

            //if (int.TryParse(port, out RemotePort))
            //{
                if (frameSender.Equals(FrameSender.SEND)) {
                    // ++ send Frame to Remote ++
                    //data is only payload, no FrameHeader
                    ChangeState(FrameWorkingState.created, "make new frame to send it later on");
                    header = new FrameHeader();
                    FramePayloadByte = data;
                }
                else if (frameSender.Equals(FrameSender.RCVE)) {
                    // ++ rcv Frame from Remote ++
                    //data includes FrameHeader
                    ChangeState(FrameWorkingState.created, "make new frame from rcv UDP Frame");

                    header = new FrameHeader(data, out FramePayloadByte);
                }
                else
                    ChangeState(FrameWorkingState.error, "FrameSender == unknown");

                if (_RemoteIsBigEndian)
                    FramePayloadByte = changeEndian(FramePayloadByte); 
            //}
            //else
            //    ChangeState(FrameWorkingState.error, "structural defect @send Frame -> port not valid: " + port);
        }
        #endregion

        #region functions 
        public override string ToString()
        {
            return TimeCreated.ToString("HH:mm:ss:fff") + " (" + frameSender.ToString() + "/" + frameState.ToString() + "/" +
                ") [" + client.ToString() + "] " + header.ToString();
        }

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

        public void SetHeaderFlag(FrameHeaderFlag fhf)
        {
            header.SetHeaderFlag(fhf);
        }

        /// <summary>
        /// frame ändert seinen status
        /// änderung wird protokolliert
        /// </summary>
        public FrameRawData ChangeState(FrameWorkingState ws, string msg)
        {
            //log.msg(this, GetDetailedString());
            //wenn error wird globaler ERROR für das Frame gesetzt
            if (ws.Equals(FrameWorkingState.error))
                frameState = FrameState.ERROR;

            ListFrameLog.Add(new frameLog(ws, msg));
            return this;
        }
        #endregion
        public byte[] getPayload()
        {
            return FramePayloadByte;
        }

        public bool IsEqual(Frame f)
        {
            if ((this.FramePayloadByte == f.FramePayloadByte) && (this.header == f.header))
                return true;
            else
                return false;
        }
        #region getter
        private Int16[] GetIntArr(byte[] data)
        {
            Int16[] intData = new Int16[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
                intData[i] = BitConverter.ToInt16(data, i * 2);
            return intData;
        }

        //TODO: für IBS um an der GUI darzustellen. später evtl rausnehmen
        public string getPayloadByte() {
            string s = string.Empty;

            for (int i = 0; i < FramePayloadByte.Length; i++)
                s += FramePayloadByte[i].ToString() + ", ";
            return s;
        }
        public string getPayloadHex()
        {
            return BitConverter.ToString(FramePayloadByte);
        }
        //TODO: für IBS um an der GUI darzustellen. später evtl rausnehmen
        public string getPayloadInt()
        {
            string s = string.Empty;

            Int16 []_FramePayloadInt = GetIntArr(FramePayloadByte);

            for (int i = 0; i < _FramePayloadInt.Length; i++)
                s += _FramePayloadInt[i].ToString() + ", ";
            return s;
        }
        //TODO: für IBS um an der GUI darzustellen. später evtl rausnehmen
        public string getPayloadASCII()
        {
            return new string(Encoding.ASCII.GetString(FramePayloadByte).ToCharArray());
        }



        /// <summary>
        /// Returns Frame Header + Frame Content to send it over stream
        /// </summary>
        /// <returns>Total Frame as Byte Array</returns>
        public byte[] GetByteArray()
        {
            byte[] RetVal = new byte[header.GetSendBytes().Length + FramePayloadByte.Length];
            System.Buffer.BlockCopy(header.GetSendBytes(), 0, RetVal, 0, header.GetSendBytes().Length);
            System.Buffer.BlockCopy(FramePayloadByte, 0, RetVal, header.GetSendBytes().Length, FramePayloadByte.Length);
            return RetVal;
        }
        
        public string GetLog()
        {
            string s = "";
            
            foreach (frameLog fl in ListFrameLog)
                s += fl.ToString() + Environment.NewLine;
            return s;
        }

        public Int16 GetIndex() {
            return header.FrameIndex;
        }

        public bool GetHeaderFlag(FrameHeaderFlag fhf) {
            return header.GetHeaderFlag(fhf);
        }

        public static int GetCountSendFrames() {
            return FrameHeader.SendFramesCount;
        }
        public static int GetCountRcvFrames() {
            return FrameHeader.RcvFramesCount;
        }
        #endregion

        class FrameHeader
        {
            /// Telegram Structure:
            /// 1 byte StructVersion; 1 byte ByteHeaderFlag; 2 byte reserviert; int16 FrameIndex; byte[x] payload
            public static Int16 SendFramesCount = 0;
            public static Int16 RcvFramesCount = 0;

            public const int FrameHeaderByteLength = 6; //Länge des Headers in Byte
            byte StructVersion = 1; //Versionskennung dieser Frame Struktur. Damit wäre es später möglich die Struktur zu verändern      

            byte ByteHeaderFlag = 0x00; //Sammelbyte das den Zustand der folgenden flags entspricht
            /*
            bool containering = false; //Erweiterung für später -> udp frame besteht nicht aus einer sondern x nachrichten. 
            //diese müssen auf der client seite eingepackt und auf der server seite ausgepackt werden

            bool _SYNC = false; //Frame zum Verbindungsaufbau bzw zum Verbindungs Status ermitteln
            public bool SYNC
            {
                set {
                _SYNC = value;
                ByteHeaderFlag = SetBit(ByteHeaderFlag, 2);
            } }

            //LOG Message -> interpretiert payload als ascii
            bool LogMessage = false;

            //Aufforderung an den server ein Empfangsbestätigung des frames zu senden
            bool acknowledge = false;

            //austausch von prozessdaten
            bool ProcessData = false;

            //management frame für cpu
            bool ManagementData = false;
             */

            //reserviert für Erweiterungen
            byte resByteI = 0xff;
            byte resByteII = 0xff;

            //16Bit FrameIndex -> wird vom client für jedes neue frame inkrementiert. server sendet als antwort frame mit dem gleichen index
            private Int16 _FrameIndex;
            public Int16 FrameIndex
            {
                get { return _FrameIndex; }
            }

            #region construktor
            //received Frame from remote
            //the payload from the frame is extractet
            public FrameHeader(byte[] ByteArray, out byte[] extractet_payload)
            {
                RcvFramesCount++;

                if (ByteArray.Length >= FrameHeaderByteLength)
                {
                    StructVersion = ByteArray[0];
                    ByteHeaderFlag = ByteArray[1];
                    resByteI = ByteArray[2];
                    resByteII = ByteArray[3];
                    _FrameIndex = BitConverter.ToInt16(ByteArray, 4);

                    extractet_payload = new byte[ByteArray.Length - FrameHeaderByteLength];
                    System.Buffer.BlockCopy(ByteArray, FrameHeaderByteLength, extractet_payload, 0, extractet_payload.Length);
                }
                else
                    throw new Exception("Exception making FrameHeader: ByteArray.Length < " + FrameHeaderByteLength.ToString());
            }
            //Frame to send at remote
            public FrameHeader()
            {
                _FrameIndex = SendFramesCount++;
            }
            #endregion


            #region functions
            public void SetHeaderFlag(FrameHeaderFlag fhf)
            {
                ByteHeaderFlag = SetBit(ByteHeaderFlag, (int)fhf);
            }
            public bool GetHeaderFlag(FrameHeaderFlag fhf)
            {
                return (ByteHeaderFlag & (1 << (int)fhf)) != 0;
            }
            public byte[] GetSendBytes()
            {
                byte[] bytearr = new byte[FrameHeaderByteLength];
                bytearr[0] = StructVersion;
                bytearr[1] = ByteHeaderFlag;
                bytearr[2] = resByteI;
                bytearr[3] = resByteII;
                System.Buffer.BlockCopy(BitConverter.GetBytes(_FrameIndex), 0, bytearr, 4, 2); //TODO LittleBigEndian hier auch beachten
                return bytearr;
            }

            /// <summary>
            /// Setzt ein bestimmtes Bit in einem Byte.
            /// </summary>
            /// <param name="b">Byte, welches bearbeitet werden soll.</param>
            /// <param name="BitNumber">Das zu setzende Bit (0 bis 7).</param>
            /// <returns>Ergebnis - Byte</returns>
            private static byte SetBit(byte b, int BitNumber)
            {
                if (BitNumber < 8 && BitNumber > -1)
                    return (byte)(b | (byte)(0x01 << BitNumber));
                return 0;
            }

            public override string ToString()
            {/*
            return "{Struct: " + StructVersion.ToString() + " Type: " + Convert.ToString(ByteHeaderFlag, 2) + 
                " res: " + resByteI.ToString() + "," + resByteII.ToString() + " Index: " + _FrameIndex.ToString() + "} ";
          * */
                return "{flag: " + Convert.ToString(ByteHeaderFlag, 2) + " Idx: " + _FrameIndex.ToString() + "} ";
            }
            #endregion
        }

        class frameLog
        {
            DateTime timestamp;
            FrameWorkingState ws;
            string msg;

            public frameLog(FrameWorkingState ws, string msg)
            {
                this.ws = ws;
                this.msg = msg;
                timestamp = DateTime.Now;
            }
            public override string ToString()
            {
                return timestamp.ToString("HH:mm:ss:ffff") + " (" + ws.ToString() + ") " + msg;
            }

        }
    }

   
    public class  Frame : FrameRawData{
        public FrameRawData AnswerFrame = null;



        #region construktor
        /// <summary>
        /// sync frame ohne content das versendet wird
        /// </summary>
        //public Frame(string ip, string port) :
        //    base(ip, port, new byte[] { }, FrameSender.SEND) { }
        public Frame(CpsClient cc) :
            base(cc, new byte[] { }, FrameSender.SEND) { }

        /// <summary>
        /// normale frames die versendet werden
        /// </summary>
        //public Frame(string ip, string port, Int16[] data) :
        //    base(ip, port, getByteArray(data), FrameSender.SEND) { }
        public Frame(CpsClient cc, Int16[] data) :
            base(cc, getByteArray(data), FrameSender.SEND) { }

        //public Frame(string ip, string port, char[] data) :
        //    base(ip, port, getByteArray(data), FrameSender.SEND) { }
        public Frame(CpsClient cc, char[] data) :
            base(cc, getByteArray(data), FrameSender.SEND) { }

        //Frame das Empfangen wurde
        public Frame(CpsClient cc, byte[] data) :
            base(cc, data, FrameSender.RCVE) { }
        #endregion

        #region frames_payload NOT_USED
        public static Int16[] GET_STATE(int index) { return new Int16[] { Convert.ToInt16(index), 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] GET_PARAM(int index) { return new Int16[] { Convert.ToInt16(index), 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] SET_STATE(int index, string position, string angle) { return new Int16[] { Convert.ToInt16(index), 2, Convert.ToInt16(position), Convert.ToInt16(angle), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        public static Int16[] SET_STATE(int index, bool state_switch) { return new Int16[] { Convert.ToInt16(index), 2, Convert.ToInt16(state_switch), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
        //public static Frame FRAME_SYNC(Int16 index, string ip, string port) {
        //    return new Frame(ip, port, FrameType.SYNC.ToString(), index);
        //    /*
        //     Frame f = new Frame(FrameType.SYNC.ToString(), check_trys, textBox_remote_ip.Text, textBox_remotePort.Text);
        //     */ 
        //}
        #endregion

        #region  funktionen
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
        #endregion
    }
}

#region unused
//private Int16[] changeEndian(Int16[] data)
//{
//    Int16[] tmp_data = new Int16[data.Length];
//    for (int i = 0; i < data.Length; i++)
//        tmp_data[i] = IPAddress.NetworkToHostOrder(data[i]);
//    return tmp_data;
//}

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


public bool isEmptyString()
{
    foreach (Int16 i in _data_HostOrder)
        if (i != 0x2020)
            return false;
    return true;
}
 * */
#endregion