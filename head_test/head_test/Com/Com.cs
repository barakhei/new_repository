using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTSerialCom;

namespace head_test
{
    public class Com
    {
        #region Variables

        private SerialClient mCom = null;
        private MessageParser mParser = null;

        public delegate void OnDataReceivedDel(object obj, int command, byte[] data);
        public event OnDataReceivedDel OnDataReceived;


        #endregion

        #region Constructor

        public Com()
        {
            mParser = new MessageParser();
            mParser.OnMessageReceived += MParser_OnMessageReceived;
        }

  

        #endregion

        #region Methods

        public bool Connect(string port, int baud)
        {
            if (mCom != null)
            {
                Disconnect();
            }

            mCom = new SerialClient(port, baud);
            mCom.OnReceiving += new EventHandler<DataStreamEventArgs>(MCom_OnReceiving);
            if (!mCom.OpenConn())
            {
                return false;
            }

            return true; 
        }



        public void Disconnect()
        {
            mCom.CloseConn();
            mCom.OnReceiving -= new EventHandler<DataStreamEventArgs>(MCom_OnReceiving);
            mCom.Dispose();
        }

        public void Send(byte [] data)
        {
            mCom.Transmit(data);
        }

        #endregion

        #region Events

        private void MParser_OnMessageReceived(object sender, int command, byte[] msg)
        {
            if(OnDataReceived != null)
            {
                OnDataReceived(this, command, msg);
            }
        }

        private void MCom_OnReceiving(object sender, DataStreamEventArgs e)
        {
            int i;

            for(i = 0; i < e.Response.Length; i++)
            {
                mParser.InsertByte(e.Response[i]);
            }
        }

        #endregion

    }
}
