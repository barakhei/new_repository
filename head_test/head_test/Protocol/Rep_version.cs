using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace head_test.Protocol
{
    public class Rep_Version : MsgBase
    {
        #region Variables

        protected string mVersion = null;

        #endregion

        #region MyRegion

        public Rep_Version()
        {
            Debug.WriteLine("Created: Rep_Version");
            Command = enumCommands.PROT_REP_VERSION;
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            mVersion = System.Text.Encoding.Default.GetString(msg);
        }

        public override string ToString()
        {
            return mVersion;
        }

        public string GetVersion()
        {
            return mVersion;
        }

        public static MsgBase Create(byte [] data)
        {
            MsgBase msg = new Rep_Version();
            msg.Parse(data);
            return msg;
        }

        #endregion

        #region Properties

        public string Version
        {
            get { return mVersion; }
        }

        #endregion
    }
}
