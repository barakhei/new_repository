using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using head_test.Protocol;

namespace head_test
{
    public class Messaging
    {
        #region Variables


        public delegate void OnMessageReceivedDel(object obj, MsgBase msg);
        public event OnMessageReceivedDel OnMessageReceived;

        private delegate MsgBase GenerateMsgDel(byte[] data);
        private Dictionary<enumCommands, GenerateMsgDel> mGenerateMsgDict = null;

        #endregion

        #region Constructor

        public Messaging()
        {
            mGenerateMsgDict = new Dictionary<enumCommands, GenerateMsgDel>();
        }

        #endregion

        #region Methods


        public void Close()
        {
            mGenerateMsgDict.Clear();
        }

        public void Init()
        {
            mGenerateMsgDict.Add(enumCommands.PROT_REP_ACK, Rep_Ack.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REP_FRAME_RAW, Rep_RawImage.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REP_FRAME_SCALE, Rep_CompressedImage.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REP_SCAN, Rep_Scan.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REPORT_PROCESSED_DATA, Rep_processed_data.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REPORT_STATUS, Rep_status.Create);
            mGenerateMsgDict.Add(enumCommands.PROT_REP_VERSION, Rep_Version.Create);

        }




        public void Parse(int command, byte [] data)
        {
            enumCommands cmd = (enumCommands)command;

            if (mGenerateMsgDict.ContainsKey(cmd) == false)
                return;

            MsgBase msg = mGenerateMsgDict[cmd](data);

            if(msg != null)
            {
                msg.Parse(data);

                /* Broadcast the message */
                if(OnMessageReceived != null)
                {
                    OnMessageReceived(this, msg);
                }
            }
        }

        public MsgBase GenerateMsg(int cmd, byte[] data)
        {
            MsgBase msg = null;
            try
            {
                msg = mGenerateMsgDict[(enumCommands)cmd](data);
            }
            catch (Exception) { }

            return msg;
        }

        #endregion

        #region Interface

        public interface IMessaging
        {
            void OnMessageReceived(MsgBase msg);
        }
        #endregion
    }
}
