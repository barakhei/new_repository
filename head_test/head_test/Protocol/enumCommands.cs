using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test.Protocol
{
    public enum enumCommands
    {
        PROT_CMD_GET_VERSION                        = 0X01,
        PROT_CMD_RESTART                            = 0X02,
        PROT_CMD_GET_FRAME_RAW                      = 0X10,
        PROT_CMD_GET_FRAME_SCALED                   = 0X11,
        PROT_CMD_GET_SCAN                           = 0X12,
        PROT_CMD_START_SCAN                         = 0X13,
        PROT_CMD_STOP_SCAN                          = 0X14,
        PROT_CMD_START_PROCESSED_DATA_STREAM        = 0X15,

        PROT_CMD_UPLOAD_DATA                        = 0X30,

        PROT_CMD_SAVE_CALIBRATION                   = 0X31,
        PROT_CMD_START_LASER_AUTO_CALIBRATION       = 0X32,
        PROT_CMD_SET_FRAME_RATE                     = 0X33,

        PROT_REP_VERSION                            = 0X80,
        PROT_REPORT_RESTART_ACK                     = 0X81,
        PROT_REPORT_PROCESSED_DATA                  = 0X82,
        PROT_REPORT_THERMISTOR_RAW                  = 0X83,
        PROT_REP_FRAME_RAW                          = 0X90,
        PROT_REP_FRAME_SCALE                        = 0X91,
        PROT_REP_SCAN                               = 0Xa0,
        PROT_REPORT_STATUS                          = 0Xb0,


        PROT_REP_ACK                                = 0Xff
    };

}
