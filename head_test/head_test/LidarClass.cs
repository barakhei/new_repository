using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using head_test.Data;
using head_test.Protocol;
using System.Threading;

namespace head_test
{
    public class LidarClass
    {
        #region Variables

        protected Messaging mMessaging;
        public Com mComPort = null;

       // public delegate void OnImageReceivedDel(RawImageData data);
      //  public delegate void OnScanReceivedDel(ScanData data);
      //  public delegate void OnProcessedDataReceivedDel(ProcessedData data);
        public delegate void OnStatusReceivedDel(Rep_status data);


      //  public event OnImageReceivedDel OnImageReceivedRaw;
      //  public event OnImageReceivedDel OnImageReceivedCompressed;
      //  public event OnScanReceivedDel OnScanReceived;
     //  /public event OnProcessedDataReceivedDel OnProcessedDataReceived;
        public event OnStatusReceivedDel OnStatusReceived;



        public delegate void MsgCallbackDel(MsgBase msg);
        public Dictionary<enumCommands, List<MsgCallbackDel>> mCallbackList;

        public object mLidarLocker = null;

        #endregion

        #region Constructor

        public LidarClass()
        {
            mCallbackList = new Dictionary<enumCommands, List<MsgCallbackDel>>();
            mLidarLocker = new object();
            mMessaging = new Messaging();

           // Cmd_SetConf_Laser_power p = new Cmd_SetConf_Laser_power(22);
        }

        #endregion

        #region Methods

        protected void SendMsg(Protocol.MsgBase msg)
        {
            if (mComPort == null)
                return;

            byte[] data;
            data = msg.GetBytes();

            mComPort.Send(data);
        }

        public void Start(string com, int baudrate)
        {
            if (mComPort != null)
                Stop();

            mComPort = new Com();
            mMessaging.OnMessageReceived += MMessaging_OnMessageReceived;
            mMessaging.Init();
            mComPort.Connect(com, baudrate);
            mComPort.OnDataReceived += MComPort_OnDataReceived;
        }

        public void Stop()
        {
            mMessaging.Close();
            if (mComPort != null)
            {
                mComPort.Disconnect();
                mComPort.OnDataReceived -= MComPort_OnDataReceived;
                mComPort = null;
            }
        }

        public void RegisterCallback(enumCommands cmd, MsgCallbackDel del)
        {
            if (!mCallbackList.ContainsKey(cmd))
            {
                mCallbackList[cmd] = new List<MsgCallbackDel>();
            }

            if (!mCallbackList[cmd].Contains(del))
            {
                mCallbackList[cmd].Add(del);
            }
        }

        public void UnregisterCallback(enumCommands cmd, MsgCallbackDel del)
        {
            if (!mCallbackList.ContainsKey(cmd))
            {
                return;
            }

            if (mCallbackList[cmd].Contains(del))
            {
                mCallbackList[cmd].Remove(del);
            }
        }

        #endregion
/*
        #region API

        public bool Lidar_SaveCalibration()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_SaveCalibration());
            }
            return true;
        }

        public bool Lidar_SetFrameRate(int rate)
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_SetFrameRate((UInt32)rate));
            }
            return true;
        }

        public bool Lidar_SetLaserPower(int power)
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_SetConf_Laser_power(power));
            }
            return true;
        }

        public bool Lidar_StartScan()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_StartScan());
            }
            return true;
        }

        public bool Lidar_StartAutoCalibration()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_StartLaserCalibration());
            }
            return true;
        }

        public bool Lidar_ReadCompressedImage()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_GetCompressedData());
            }
            return true;
        }

        public bool Lidar_ReadRawImage()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_GetRawData());
            }
            return true;
        }

        public bool Lidar_StopScan()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_StopScan());
            }
            return true;
        }

        public void Lidar_GetRawData()
        {
            lock(mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_GetRawData());
            }
        }

        public void Lidar_StartStreamingProcessedData(UInt32 interval)
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_StartProcessedDataStreaming(interval));
            }
        }

        public void Lidar_StopStreamingProcessedData()
        {
            lock (mLidarLocker)
            {
                SendMsg(new Protocol.Cmd_StartProcessedDataStreaming(0));
            }
        }

        #endregion
*/
        #region Events

        private void MMessaging_OnMessageReceived(object obj, Protocol.MsgBase msg)
        {
 
            MessageHandle(msg);

            enumCommands cmd = (enumCommands)msg.Command;
            
            if (mCallbackList.ContainsKey(cmd))
            {
                foreach (MsgCallbackDel del in mCallbackList[cmd])
                {
                    del(msg);
                }

            }
        }

        public void MessageHandle(Protocol.MsgBase msg)
        {
            switch(msg.Command)
            {
                case enumCommands.PROT_REP_SCAN:

                 //   ScanData scan_data = new ScanData(((Rep_Scan)msg).Distances, ((Rep_Scan)msg).Intensities, ((Rep_Scan)msg).Section);

               //     if(OnScanReceived != null)
               //     {
                //        OnScanReceived(scan_data);
                //    }
                    
                //    break;

                case enumCommands.PROT_REPORT_STATUS:

                    if(OnStatusReceived != null)
                    {
                        OnStatusReceived((Rep_status)msg);
                    }
                    break;

                case enumCommands.PROT_REP_ACK:


                    break;
/*
                case enumCommands.PROT_REP_FRAME_RAW:

                    if (OnImageReceivedRaw != null)
                    {
                        RawImageData raw_image_data = new RawImageData()
                        {
                            Samples = ((Rep_RawImage)msg).FrameData
                        };

                        if(OnImageReceivedRaw != null)
                        {
                            OnImageReceivedRaw(raw_image_data);
                        }
                        
                    }
                    break;

                case enumCommands.PROT_REP_FRAME_SCALE:

                    if (OnImageReceivedRaw != null)
                    {
                        RawImageData raw_image_data = new RawImageData()
                        {
                            Samples = ((Rep_RawImage)msg).FrameData
                        };

                        if (OnImageReceivedRaw != null)
                        {
                            OnImageReceivedRaw(raw_image_data);
                        }

                    }
                    break;

                case enumCommands.PROT_REPORT_PROCESSED_DATA:

                    if(OnProcessedDataReceived != null)
                    {
                        ProcessedData processed_data = new ProcessedData()
                        {
                            Baseline = ((Rep_processed_data)msg).Baseline,
                            Center = ((Rep_processed_data)msg).Center,
                            Distance = ((Rep_processed_data)msg).Distance,
                            PeakIntensity = ((Rep_processed_data)msg).PeakIntensity,
                            Peak_x = ((Rep_processed_data)msg).Peak_x,
                            PreciseCenter = ((Rep_processed_data)msg).PreciseCenter,
                            PreciseDistance = ((Rep_processed_data)msg).PreciseDistance,
                            Intensity = ((Rep_processed_data)msg).Intensity
                        };

                        OnProcessedDataReceived(processed_data);
                    }

                    break;
                    */
            }
        }
/*
        private void Lidar_OnProcessedDataReceived(ProcessedData data)
        {
            throw new NotImplementedException();
        }
*/
        private void MComPort_OnDataReceived(object obj, int command, byte[] data)
        {
            mMessaging.Parse(command, data);
        }

        #endregion
    }
}
