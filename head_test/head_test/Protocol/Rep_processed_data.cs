using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test.Protocol
{
    public class Rep_processed_data : MsgBase
    {
        #region Variables

        protected int mPeak;
        protected int mIntensity;
        protected int mPeak_x;
        protected int mCenter;
        protected int mDistance;
        protected float mBaseline;
        protected float mPreciseCenter;
        protected int mPreciseDistance;


        #endregion

        #region Constructor

        public Rep_processed_data()
        {
            Command = enumCommands.PROT_REPORT_PROCESSED_DATA;
        }

        public Rep_processed_data(byte[] msg)
        {
            Command = enumCommands.PROT_REPORT_PROCESSED_DATA;

            Parse(msg);
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            mPeak = BitConverter.ToInt32(msg, 0);
            mPeak_x = BitConverter.ToInt32(msg, 4);
            mCenter = BitConverter.ToInt32(msg, 8);
            mDistance = BitConverter.ToInt32(msg, 12);
            mBaseline = BitConverter.ToSingle(msg, 16);
            mPreciseCenter = BitConverter.ToSingle(msg, 20);
            mPreciseDistance = BitConverter.ToInt32(msg, 24);
            //mPreciseDistance = BitConverter.ToInt32(msg, 28);
            //mPreciseDistance = BitConverter.ToInt32(msg, 32);
            //mPreciseDistance = BitConverter.ToInt32(msg, 36);
            mIntensity = BitConverter.ToInt32(msg, 40);
        }

        public static Rep_processed_data Create(byte [] data)
        {
            return new Rep_processed_data(data);
        }

        #endregion

        #region Properties

        public int PeakIntensity
        {
            get { return mPeak; }
        }

        public int Peak_x
        {
            get { return mPeak_x; }
        }

        public int Center
        {
            get { return mCenter; }
        }

        public int Distance
        {
            get { return mDistance; }
        }

        public float Baseline
        {
            get { return mBaseline; }
        }

        public float PreciseCenter
        {
            get { return mPreciseCenter; }
        }

        public int PreciseDistance
        {
            get { return mPreciseDistance; }
        }

        public int Intensity
        {
            get { return mIntensity; }
        }

        #endregion

    }
}
