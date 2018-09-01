using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace head_test.Protocol
{
    public class Rep_Scan : MsgBase
    {
        #region Variables

        protected UInt16[] mFrameData;
        protected int[] mIntensity;
        protected int[] mDistance;
        protected int mSectionNumber;

        #endregion

        #region MyRegion

        public Rep_Scan()
        {
            Debug.WriteLine("Created: Rep_Scan");
            Command = enumCommands.PROT_REP_SCAN;
        }

        public Rep_Scan(byte [] data)
        {
            Debug.WriteLine("Created: Rep_Scan");
            Command = enumCommands.PROT_REP_SCAN;

            Parse(data);
        }

        public Rep_Scan(enumCommands scan_number)
        {
            Debug.WriteLine("Created: Rep_Scan");
            Command = scan_number;
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            mFrameData = new UInt16[msg.Length / 2 - 1];
            mIntensity = new int[msg.Length / 2 - 1];
            mDistance = new int[msg.Length / 2 - 1];

            Buffer.BlockCopy(msg, 2, mFrameData, 0, msg.Length - 2);

            for (int i = 0; i < mFrameData.Length; i++)
            {
                mIntensity[i] = (int)((mFrameData[i] >> 10) & 0x3f);
                mDistance[i] = (int)(mFrameData[i] & 0x3ff);
            }

            
            mSectionNumber = BitConverter.ToUInt16(msg, 0);
        }

        public static MsgBase Create()
        {
            return new Rep_Scan();
        }

        public static MsgBase Create(byte [] data)
        {
            return new Rep_Scan(data);
        }

        #endregion

        #region Properties

        public int[] Distances
        {
            get { return mDistance; }
        }

        public int[] Intensities
        {
            get { return mIntensity; }
        }

        public int Section
        {
            get { return mSectionNumber; }
        }

        #endregion




    }
}
