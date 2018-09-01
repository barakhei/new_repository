using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using head_test;
//using Lidar.Data;
using head_test.Presenter;

namespace head_test
{
    public partial class StatusControl : UserControl
    {
        #region Variable

        LidarPresenter mPresenter;

        #endregion

        #region Constructor

        public StatusControl()
        {
            InitializeComponent();
            mPresenter = new LidarPresenter();
            head_test.Protocol.Rep_status st = new head_test.Protocol.Rep_status();
            UpdateUI(st);

        }



        #endregion

        #region Methods

        public void Init(head_test.LidarClass lidar)
        {
            mPresenter.Init(lidar);
            mPresenter.StatusShow += MPresenter_StatusShow;

        }

        public void Close()
        {
            mPresenter.Close();
        }

        public void UpdateUI(head_test.Protocol.Rep_status status)
        {
            StatusLabel.Text = String.Empty;

            StatusLabel.Text += "Flag_LowPDReading:                       " + status.Flag_LowPDReading.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LowCurrentReading:                " + status.Flag_LowCurrentReading.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_HighPDReading:                    " + status.Flag_HighPDReading.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_HighCurrentReading:               " + status.Flag_HighCurrentReading.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_UnknownError:                     " + status.Flag_UnknownError.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_CurrentOutsideLimits:             " + status.Flag_CurrentOutsideLimits.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_PDFeedbackOutsideLimits:          " + status.Flag_PDFeedbackOutsideLimits.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LaserNC:                          " + status.Flag_LaserNC.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LaserOn:                          " + status.Flag_LaserOn.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LidarRotating:                    " + status.Flag_LidarRotating.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_FlashCRC:                         " + status.Flag_FlashCRC.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_DistanceCalibrationCRC:           " + status.Flag_DistanceCalibrationCRC.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LaserControlCRC:                  " + status.Flag_LaserControlCRC.ToString() + Environment.NewLine;
            StatusLabel.Text += "Flag_LaserProfilingCRC:                         " + status.Flag_LaserProfilingCRC.ToString() + Environment.NewLine;
            StatusLabel.Text += "LaserCurrent:                          " + status.LaserCurrent.ToString() + Environment.NewLine;
            StatusLabel.Text += "OperationState:                       " + status.OperationState.ToString() + Environment.NewLine;
            StatusLabel.Text += "Temperature:                           " + status.Temperature.ToString() + Environment.NewLine;
        }

        #endregion

        #region Events

     //   private void SetFrameRateButton_Click(object sender, EventArgs e)
     //   {
     //       mPresenter.SetFrameRate((int)FrameRateNumeric.Value);
     //   }

        private void MPresenter_StatusShow(object obj, head_test.Protocol.Rep_status data)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    UpdateUI(data);
                });
            }
            else
            {
                UpdateUI(data);
            }
        }



        #endregion


    }
}
