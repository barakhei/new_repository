using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace head_test.Protocol
{
    public class Rep_RawImage: MsgBase
    {
        #region Variables

        protected int [] mFrameData;

        #endregion

        #region MyRegion

        public Rep_RawImage()
        {
            Debug.WriteLine("Created: Rep_RawImage");
            Command = enumCommands.PROT_REP_FRAME_RAW;
        }

        public Rep_RawImage(byte [] data)
        {
            Debug.WriteLine("Created: Rep_RawImage");
            Command = enumCommands.PROT_REP_FRAME_RAW;

            Parse(data);
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            mFrameData = new int[msg.Length / 2];
            for(int i = 0; i < mFrameData.Length; i++)
            {
                mFrameData[i] = BitConverter.ToInt16(msg, i * 2);
            }
        }

        public static MsgBase Create()
        {
            return new Rep_RawImage();
        }

        public static MsgBase Create(byte [] data)
        {
            return new Rep_RawImage(data);
        }

        #endregion

        #region Properties

        public int[] FrameData
        {
            get { return mFrameData; }
        }

        #endregion
    }
}
