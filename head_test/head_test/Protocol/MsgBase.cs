using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test.Protocol
{
    public class MsgBase
    {
        #region Variables

        protected byte[] payload = null;
        public static int COMMAND = 4;
        
        

        #endregion

        #region Constructor

        protected MsgBase()
        {

        }

        #endregion

        #region Methods

        public virtual void Parse(byte[] msg) { }

        public byte [] GetBytes()
        {
            return MessageParser.BuildMessage((int)Command, payload);
        }

        #endregion

        #region Properties
        public enumCommands Command
        {
            get; set;
        }

        #endregion
    }
}
