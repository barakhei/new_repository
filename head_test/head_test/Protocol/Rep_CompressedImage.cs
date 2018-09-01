using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace head_test.Protocol
{
    public class Rep_CompressedImage : MsgBase
    {
        #region Variables

        protected int[] mFrameData;

        #endregion

        #region MyRegion

        public Rep_CompressedImage()
        {
            Debug.WriteLine("Created: Rep_CompressedImage");
            Command = enumCommands.PROT_REP_FRAME_SCALE;
        }

        public Rep_CompressedImage(byte [] data)
        {
            Debug.WriteLine("Created: Rep_CompressedImage");
            Command = enumCommands.PROT_REP_FRAME_SCALE;

            Parse(data);
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            mFrameData = new int[msg.Length / 2];
            Buffer.BlockCopy(msg, 0, mFrameData, 0, msg.Length);
        }

        public static MsgBase Create()
        {
            return new Rep_CompressedImage();
        }

        public static MsgBase Create(byte [] data)
        {
            return new Rep_CompressedImage(data);
        }

        #endregion
    }
}
