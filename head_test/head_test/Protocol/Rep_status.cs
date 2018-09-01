using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test.Protocol
{
    public class Rep_status : MsgBase
    {
        #region Variables

        const uint STATUS_ERR_FLAG_LOW_PD_READING = 0x00000001;
        const uint STATUS_ERR_FLAG_LOW_CURRENT_READING = 0x00000002;
        const uint STATUS_ERR_FLAG_HIGH_PD_READING = 0x00000004;
        const uint STATUS_ERR_FLAG_HIGH_CURRENT_READING = 0x00000008;
        const uint STATUS_ERR_FLAG_UNKNOWN_ERROR = 0x00000010;
        const uint STATUS_ERR_FLAG_CURRENT_OUTSIDE_LIMITS = 0x00000020;
        const uint STATUS_ERR_FLAG_PD_FEEDBACK_OUTSIDE_LIMITS = 0x00000040;
        const uint STATUS_ERR_FLAG_LASER_NC = 0x00000080;
        const uint STATUS_FLAG_LASER_ON = 0x00000100;   /* Indicate if the laser is on */
        const uint STATUS_FLAG_LIDAR_IS_ROTATING = 0x00000200;  /* Indicate if the LIDAR is rotating */
        const uint STATUS_FLAG_LIDAR_AUTO_SCAN = 0x00000400;        /* Lidar set to send scanned data without any command */
        const uint STATUS_FLASH_CRC = 0x00000800;
        const uint STATUS_FLASH_CALIBRATION_CRC = 0x00001000;
        const uint STATUS_FLASH_LASER_POWER_CRC = 0x00002000;
        const uint STATUS_FLASH_LASER_PROFILE_CRC = 0x00004000;


        #endregion

        #region Constructor

        public Rep_status()
        {
            Command = enumCommands.PROT_REPORT_STATUS;
        }

        #endregion

        #region Methods

        public override void Parse(byte[] msg)
        {
            UInt32 flags = BitConverter.ToUInt32(msg, 0);

            Flag_LowPDReading = (flags & STATUS_ERR_FLAG_LOW_PD_READING) != 0;
            Flag_LowCurrentReading = (flags & STATUS_ERR_FLAG_LOW_CURRENT_READING) != 0;
            Flag_HighPDReading = (flags & STATUS_ERR_FLAG_HIGH_PD_READING) != 0;
            Flag_HighCurrentReading = (flags & STATUS_ERR_FLAG_HIGH_CURRENT_READING) != 0;
            Flag_UnknownError = (flags & STATUS_ERR_FLAG_UNKNOWN_ERROR) != 0;
            Flag_CurrentOutsideLimits = (flags & STATUS_ERR_FLAG_CURRENT_OUTSIDE_LIMITS) != 0;
            Flag_PDFeedbackOutsideLimits = (flags & STATUS_ERR_FLAG_PD_FEEDBACK_OUTSIDE_LIMITS) != 0;
            Flag_LaserNC = (flags & STATUS_ERR_FLAG_LASER_NC) != 0;
            Flag_LaserOn = (flags & STATUS_FLAG_LASER_ON) != 0;
            Flag_LidarRotating = (flags & STATUS_FLAG_LIDAR_IS_ROTATING) != 0;
            Flag_FlashCRC = (flags & STATUS_FLASH_CRC) != 0;
            Flag_DistanceCalibrationCRC = (flags & STATUS_FLASH_CALIBRATION_CRC) != 0;
            Flag_LaserControlCRC = (flags & STATUS_FLASH_LASER_POWER_CRC) != 0;
            Flag_LaserProfilingCRC = (flags & STATUS_FLASH_LASER_PROFILE_CRC) != 0;

            LaserCurrent = BitConverter.ToUInt32(msg, 4);
            OperationState = BitConverter.ToUInt32(msg, 8);
            Temperature = BitConverter.ToInt32(msg, 12);
        }

        public static Rep_status Create()
        {
            return new Rep_status();
        }

        public static MsgBase Create(byte [] data)
        {
            MsgBase msg = new Rep_status();
            msg.Parse(data);
            return msg; 
        }

        #endregion

        #region Properties

        public bool Flag_LowPDReading
        { get; set; }
        public bool Flag_LowCurrentReading
        { get; set; }
        public bool Flag_HighPDReading
        { get; set; }
        public bool Flag_HighCurrentReading
        { get; set; }
        public bool Flag_UnknownError
        { get; set; }
        public bool Flag_CurrentOutsideLimits
        { get; set; }
        public bool Flag_PDFeedbackOutsideLimits
        { get; set; }
        public bool Flag_LaserNC
        { get; set; }
        public bool Flag_LaserOn
        { get; set; }
        public bool Flag_LidarRotating
        { get; set; }
        public bool Flag_FlashCRC
        { get; set; }
        public bool Flag_LaserControlCRC
        { get; set; }
        public bool Flag_LaserProfilingCRC
        { get; set; }
        public bool Flag_DistanceCalibrationCRC
        { get; set; }
        public uint LaserCurrent
        { get; set; }
        public uint OperationState
        { get; set; }
        public int Temperature
        { get; set; }

        #endregion

    }
}
