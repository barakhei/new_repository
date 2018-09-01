using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using head_test.Protocol;
//using Lidar.Data;
using head_test;

namespace head_test.Presenter
{
    public class LidarPresenter
    {
        #region Variables
        
      //  public delegate void OnRawDataReceivedDel(object obj, RawImageData data);
      //  public OnRawDataReceivedDel OnRawDataReceivedCallback;
        protected head_test.LidarClass mLidar;
/*
        public delegate void RawDataShowDel(object obj, RawImageData data);
        public event RawDataShowDel RawDataShow;

        public delegate void ScannedDataShowDel(object obj, ScanData data);
        public event ScannedDataShowDel ScannedDataShow;

        public delegate void ProcessedDataShowDel(object obj, ProcessedData data);
        public event ProcessedDataShowDel ProcessedDataShow;
        */
        public delegate void StatusShowDel(object obj, Rep_status data);
        public event StatusShowDel StatusShow;


        int NumberOfFramesRecorded = 0;
        int NumberOfFramesToRecord = 0;

       // RawImageData LastImage;

        #endregion

        #region Constructor

        public LidarPresenter()
        {
        }



        #endregion

        #region Methods

        internal void Init(head_test.LidarClass lidar)
        {
            mLidar = lidar;
         //   mLidar.OnImageReceivedRaw += MLidar_OnImageReceivedRaw;
       //     mLidar.OnScanReceived += MLidar_OnScanReceived;
         //   mLidar.OnProcessedDataReceived += MLidar_OnProcessedDataReceived;
            mLidar.OnStatusReceived += MLidar_OnStatusReceived;
        }

       

        public void Close()
        {
           // mLidar.OnImageReceivedRaw -= MLidar_OnImageReceivedRaw;
           // mLidar.OnImageReceivedRaw -= MLidar_OnImageReceivedRaw;
        }
/*
        public void SetFrameRate(int rate)
        {
            mLidar.Lidar_SetFrameRate(rate);
        }

        public void ReadRawImage()
        {
            mLidar.Lidar_ReadRawImage();
        }

        public void ReadCompressedImage()
        {
            mLidar.Lidar_ReadCompressedImage();
        }

        void ToCSV(string path, RawImageData data)
        {

            var result = string.Join("," , data.Samples);

            try
            {

                //System.IO.File.AppendAllText(path, result);
                System.IO.File.AppendAllText(path, data.AverageCenter.ToString());
                System.IO.File.AppendAllText(path, Environment.NewLine);
            }
            catch (Exception) { }
        }

        void ToCSV_New_frame(string path, RawImageData data)
        {

            var result = string.Join(",", data.Samples);

            try
            {
                System.IO.File.AppendAllText(path, Environment.NewLine);


                
                //System.IO.File.AppendAllText(path, result);
                System.IO.File.AppendAllText(path, result);
                
            }
            catch (Exception) { }
        }
*/
        public void StartRecordN(int n)
        {
            NumberOfFramesRecorded = 0;
            NumberOfFramesToRecord = n;


        }
/*
        public void StartScan()
        {
            mLidar.Lidar_StartScan();
        }

        public void SaveCalibration()
        {
            mLidar.Lidar_SaveCalibration();
        }

        public void SetLaserPower(int power)
        {
            mLidar.Lidar_SetLaserPower(power);
        }

        public void StopScan()
        {
            mLidar.Lidar_StopScan();
        }

        public void StartStreamingProcessedData()
        {
            mLidar.Lidar_StartStreamingProcessedData(20);
        }

        public void StopStreamingProcessedData()
        {
            mLidar.Lidar_StopStreamingProcessedData();
        }

        public void StartLaserAutoCalibration()
        {
            mLidar.Lidar_StartAutoCalibration();
        }
*/
        #endregion

        #region Events

        private void MLidar_OnStatusReceived(Rep_status data)
        {
            if(StatusShow != null)
            {
                StatusShow(this, data);
            }
        }
/*
        private void MLidar_OnImageReceivedRaw(RawImageData data)
        {
            if(RawDataShow != null)
            {
                RawDataShow(this, data);

                if((NumberOfFramesToRecord > NumberOfFramesRecorded) && (NumberOfFramesToRecord != 0))
                {
                    ToCSV_New_frame("for_efi.csv", data);
                    ToCSV("avarages_5.4m.csv", data);
                    NumberOfFramesRecorded++;
                }
                else
                {
                    NumberOfFramesRecorded = 0;
                    NumberOfFramesToRecord = 0;
                }

                lock(this)
                {
                    LastImage = data;
                }
            }
        }
*/
        public void SaveToCSV(string path)
        {
            
            lock (this)
            {
          //      LastImage.ToCSV(path);
            }
        }
/*
        public void OpenCsv(string path)
        {
            LastImage = new RawImageData();
            LastImage.OpenCSV(path);
           RawDataShow(this, LastImage);
        }

        private void MLidar_OnScanReceived(ScanData data)
        {
            if(ScannedDataShow != null)
            {
                ScannedDataShow(this, data);
            }
        }

        private void MLidar_OnProcessedDataReceived(ProcessedData data)
        {
            if(ProcessedDataShow != null)
            {
                ProcessedDataShow(this, data);
            }
        }
*/
        #endregion


    }
}
