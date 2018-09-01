using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace head_test.Protocol
{
    public class Rep_Ack: MsgBase
    {
        #region MyRegion

        public Rep_Ack()
        {
            Debug.WriteLine("Created: Rep_Ack");
            Command = enumCommands.PROT_REP_ACK;
        }

        #endregion

        #region Methods

        public static MsgBase Create()
        {
            return new Rep_Ack();
        }

        public static MsgBase Create(byte [] data)
        {
            return new Rep_Ack();
        }

        #endregion
    }
}
