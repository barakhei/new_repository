using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

using head_test;
//using Lidar.Data;
//using RangeFinder;
//using TargetCart;

namespace head_test.Presenter
{
    public class DecimationTableCalculation
    {
        #region Variables

      //  private CartClass mCart;
        private LidarClass mLidar;
        //private RangeFinderClass mRange;


        private float last_received_range;
        private object range_lock;

        private ManualResetEvent RangeResetEvent;
        private ManualResetEvent FrameResetEvent;

        private const double OFFSET_DISTANCE = 0;
        private const double MAX_DISTANCE = 5.5;
        private const double MIN_DISTANCE = 0.4;

        private const int LINEAR_RANGE_SIZE = 240;
        private const int POLINOMIAL_RANGE_SIZE = 70;
        private const int NUMBER_OF_PIXELS = 2000;


        LinkedList<RawImahgeStatisticsClass> ProcessDataList = new LinkedList<RawImahgeStatisticsClass>();

        Thread mProcessThread = null;


        #endregion

        #region Constructor

        public DecimationTableCalculation()
        {
            range_lock = new object();

            last_received_range = -1;
            RangeResetEvent = new ManualResetEvent(false);
            FrameResetEvent = new ManualResetEvent(false);
        }

        #endregion

        #region Methods

        public void Init(LidarClass lidar)
        {
            mLidar = lidar;

            if (mLidar != null)
            { 
               // mLidar.OnImageReceivedRaw += MLidar_OnImageReceivedRaw;
            }


        }

    //    private void MLidar_OnImageReceivedRaw(RawImageData data)
     //   {

    //        ProcessDataList.AddFirst(ProcessFrameImage(data));
    //        FrameResetEvent.Set();
    //    }

        private void MRange_OnRangeReceived(float range)
        {
            lock (range_lock)
            {
                last_received_range = range;
                Debug.WriteLine(range.ToString());
            }
            RangeResetEvent.Set();
        }

        public void StartCalibration()
        {
            mProcessThread = new Thread(new ThreadStart(ProcessThread));
            mProcessThread.Start();
        }
/*
        private RawImahgeStatisticsClass ProcessFrameImage(RawImageData data)
        {
            float range;

            lock(range_lock)
            {
                range = last_received_range;
            }

            if (range < 0)
                return null;

            RawImahgeStatisticsClass stat = new RawImahgeStatisticsClass();
            stat.Distance = range;

            for (int i = 0; i < data.Samples.Length; i++)
            {
                if(data.Samples[i] > stat.PeakIntensity)
                {
                    stat.PeakIntensity = data.Samples[i];
                    stat.PeakIntensityPixel = i;
                }
            }

            stat.Baseline = (int)data.Baseline;
            stat.MidHeight = (stat.PeakIntensity + stat.Baseline) / 2;



            stat.LeftBellEnterPixel = 0;
            stat.RightBellEnterPixel = data.Samples.Length - 1;

            for (int i = stat.LeftBellEnterPixel; i <= stat.PeakIntensityPixel; i++)
            {
                if(data.Samples[i] < stat.MidHeight)
                {
                    stat.LeftBellEnterPixel = i;
                }
            }
            
            for (int i = stat.RightBellEnterPixel; i >= stat.PeakIntensityPixel; i--)
            {
                if (data.Samples[i] < stat.MidHeight)
                {
                    stat.RightBellEnterPixel = i;
                }
            }

            stat.AverageBellPixel = (stat.RightBellEnterPixel + stat.LeftBellEnterPixel) / 2;

            return stat;
        }

        */
        void ProcessThread()
        {
            float range;
            ProcessDataList.Clear();

            /* Return to farest */
            while (true)
            { 
                RangeResetEvent.Reset();
                if (RangeResetEvent.WaitOne(3000) == false)
                {
              //      mCart.SetSpeed(0);
                    return;
                }

                lock (range_lock)
                {
                    range = last_received_range;
                }

                if (range > MAX_DISTANCE)
                {
              //      mCart.SetSpeed(0);
                    break;
                }
                else
                {
              //      mCart.SetSpeed(10);
                }

            }
           // mCart.SetSpeed(0);

          //  mCart.SetSpeed(-3);
            /* Move the cart and collect data */
            while (true)
            {
                RangeResetEvent.Reset();
                if (RangeResetEvent.WaitOne(3000) == false)
                {
              //      mCart.SetSpeed(0);
                    return;
                }

                FrameResetEvent.Reset();
                lock (range_lock)
                {
                    if(Math.Abs(last_received_range - range) > 0.05)
                    {
                        range = last_received_range;
                  ///      mLidar.Lidar_ReadRawImage(); 
                    }
                    else
                    {
                        continue;
                    }
                }

                if(FrameResetEvent.WaitOne(2000) == false)
                {
            //        mCart.SetSpeed(0);
                    return;
                }

                if (range < MIN_DISTANCE)
                {
                //    mCart.SetSpeed(0);
                    break;
                }
                else
                {
              //      mCart.SetSpeed(-2);
                }
            }

            /* Generate table */
            SaveTableToCSV("test.csv");

        }


        void SaveTableToCSV(string path)
        {
            string fullPath = GeneralData.GetExecutablePath() + "\\" + path;

            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            var csv = new StringBuilder();

            foreach(RawImahgeStatisticsClass stat in ProcessDataList)
            {
                var newLine = string.Format("{0},{1}", stat.Distance, stat.AverageBellPixel);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(GeneralData.GetExecutablePath() + "\\" + path, csv.ToString());

            GenerateMaskTable("test.csv", "out.txt");
        }

        public void GenerateMaskTable(string path_full_table, string path_out_table)
        {
            LinkedList<RawImahgeStatisticsClass> linked_list = ProcessDataList;
            string fullReadPath = GeneralData.GetExecutablePath() + "\\" + path_full_table;
            string fullWritePath = GeneralData.GetExecutablePath() + "\\" + path_out_table;

            if (path_full_table != null)
            {
                linked_list = new LinkedList<RawImahgeStatisticsClass>();

                string[] lines = System.IO.File.ReadAllLines(fullReadPath);

                foreach(string line in lines)
                {
                    string[] val = line.Split(',');
                    var item = new RawImahgeStatisticsClass();
                    item.Distance = float.Parse(val[0]);
                    item.AverageBellPixel = int.Parse(val[1]);
                    linked_list.AddLast(item);
                }
            }
            
            /* The last Item should be with lower pixel count */
            RawImahgeStatisticsClass last_item = linked_list.Last();

        /* 
            The masking table should be fixed length (310 points).
            The first 240 points are linear
            Last 70 points polinomial order 2, derivative at point 240 should be 1
        */
        int[] mask_table = new int[LINEAR_RANGE_SIZE + POLINOMIAL_RANGE_SIZE];

            int min_pixel = last_item.AverageBellPixel - 50;

            /* Calculate the linear range */
            for (int i = 0; i < LINEAR_RANGE_SIZE; i++)
            {
                mask_table[i] = min_pixel + i;
            }

            /* Calculate polinomial range */
            double a = ((double)(NUMBER_OF_PIXELS - 1 - mask_table[LINEAR_RANGE_SIZE - 1] - POLINOMIAL_RANGE_SIZE)) /
                ((double)(POLINOMIAL_RANGE_SIZE * POLINOMIAL_RANGE_SIZE));

            double b = 1 - 2 * a * (double)(LINEAR_RANGE_SIZE - 1);

            double c = mask_table[LINEAR_RANGE_SIZE - 1] - Math.Pow(LINEAR_RANGE_SIZE - 1, 2) * a - (LINEAR_RANGE_SIZE - 1) * b;

            
            for (int i = LINEAR_RANGE_SIZE - 1; i < (POLINOMIAL_RANGE_SIZE + LINEAR_RANGE_SIZE); i++)
            {
                mask_table[i] = (int)(a * ((double)(i * i)) + b * (double)i + c);
            }

            generate_mask_table_class(fullWritePath, mask_table);
        }

        void generate_mask_table_class(string path, int[] arr)
        {
            string fullPath = GeneralData.GetExecutablePath() + "\\" + path;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            var txt = new StringBuilder();

            txt.AppendLine(@"#define COMPRESSED_SCAN_DATA   " + arr.Length.ToString());
            txt.AppendLine(@"static uint32_t mask_table[COMPRESSED_SCAN_DATA] = {");
            for (int i = 0; i < (arr.Length - 1); i++)
            {
                txt.Append(string.Format("{0},", arr[i]));
            }
            txt.AppendLine(string.Format("{0}", arr[arr.Length - 1]));
            txt.AppendLine(@"};");

            File.WriteAllText(path, txt.ToString());
        }



        #endregion

        #region Events

        #endregion

        #region Class

        class RawImahgeStatisticsClass
        {
            public RawImahgeStatisticsClass()
            {
                PeakIntensity = 0;
                LeftBellEnterPixel = 0;
                RightBellEnterPixel = 1999;
            }

            public float Distance;                /* Distance of the sample image */
            public int PeakIntensityPixel;      /* Pixel at top point of the sample image */
            public int PeakIntensity;           /* Pixel at top point of the sample image */
            public int Baseline;                /* Offset level of the samples */
            public int MidHeight;               /* Height where left and right enter to the bell calculated */
            public int LeftBellEnterPixel;      /* Left enter to the bell pixel */
            public int RightBellEnterPixel;     /* Right enter to the bell pixel */
            public int AverageBellPixel;        /* Mid pixel betwin left and right pixel */
        }

        #endregion
    }

}
