using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test
{
    public class MessageParser
    {
        #region Variables

        private MessageState state;
        private int message_length;
        private int bytes_received;
        public int section;
        private byte [] length = new byte[2];
        public byte[] msg;
        public int[] points;
        private byte cs;

        private int command;


        public delegate void OnMessageReceivedDel(object sender, int command, byte [] msg);
        public event OnMessageReceivedDel OnMessageReceived;

        #endregion

        #region Constructor

        public MessageParser()
        {
            state = MessageState.Sync1;
        }

        #endregion

        #region Enums

        public enum MessageState
        {
            Sync1, Sync2, Sync3, Sync4, Command, Len1, Len2, Data, CS
        }

        #endregion

        #region Methods

        public void InsertByte(byte data)
        {
            cs += data;
            switch(state)
            {
                case MessageState.Sync1:
                    if (data == 0xa5)
                        state = MessageState.Sync2;
                    cs = data;
                    break;

                case MessageState.Sync2:
                    if (data == 0x5a)
                        state = MessageState.Sync3;
                    break;

                case MessageState.Sync3:
                    if (data == 0xa5)
                        state = MessageState.Sync4;
                    break;

                case MessageState.Sync4:
                    if (data == 0x5a)
                        state = MessageState.Command;
                    break;

                case MessageState.Command:
                    command = data;
                    state = MessageState.Len1;
                    break;

                case MessageState.Len1:
                    state = MessageState.Len2;
                    length[0] = data;
                    break;

                case MessageState.Len2:

                    length[1] = data;
                    message_length = BitConverter.ToUInt16(length, 0);
                    bytes_received = 0;
                    msg = new byte[message_length];
                    points = new int[message_length / 2];
                    
                    if(message_length == 0)
                    {
                        state = MessageState.CS;
                    }
                    else
                    {
                        state = MessageState.Data;
                    }
                    break;

                case MessageState.Data:

                    msg[bytes_received] = data;
                    bytes_received++;

                    if((bytes_received % 2) == 0)
                    {
                        points[bytes_received / 2 - 1] =  BitConverter.ToInt16(msg, bytes_received - 2);
                    }

                    if(bytes_received >= message_length)
                    {
                        state = MessageState.CS;
                    }
                    
                    break;

                case MessageState.CS:
                    state = MessageState.Sync1;
                    if(cs == 0)
                    {
                        if (OnMessageReceived != null)
                        {
                            OnMessageReceived(this, command, msg);
                        }
                    }
                    break;
            }
        }

        public static byte[] BuildMessage(int OpCode, byte[] payload)
        {
            byte[] data;
            int length = 0;
            byte cs = 0;

            if (payload != null)
                length = payload.Length;

            data = new byte[8 + length];

            data[0] = 0xa5;
            data[1] = 0x5a;
            data[2] = 0xa5;
            data[3] = 0x5a;
            data[4] = (byte)OpCode;
            data[5] = (byte)(length % 256);
            data[6] = (byte)(length / 256);
            for (int i = 0; i < length; i++)
            {
                data[7 + i] = payload[i];
                cs += payload[i];
            }

            for (int i = 0; i < 7; i++)
                cs += data[i];

            data[7 + length] = (byte)(-cs);

            return data;
        }

        #endregion
    }
}
