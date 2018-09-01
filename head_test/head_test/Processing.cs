using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace head_test
{
    public class Processing
    {
        #region Variables

        int[] mSamples;
     //   float center_of_mass;
     //   float center_simple;


        const int BASELINE_FILTER_SIZE = 10;
        const int BASELINE_NUMBER_OF_SECTIONS = 5;
        const int MIN_INTENSITY = 10;
        const int DARK_BASE_LINE = 100;


        #endregion


        #region Constructor

        public Processing(int [] samples)
        {
            mSamples = samples;
        }

        #endregion

        #region Methods

        void BubbleSort(ref int [] a)
        {
            int i, j, temp;
            for (i = 0; i < a.Length - 1 ; ++i)
            {
                for (j = 0; j < a.Length - 1 - i; ++j)
                {
                    if (a[j] > a[j + 1])
                    {
                        temp = a[j + 1];
                        a[j + 1] = a[j];
                        a[j] = temp;
                    }
                }
            }
        }

        public double CalculateBaseLine(int[] a)
        {
            int scan;
            int i;
            int interval;
            int [] sum = new int[BASELINE_NUMBER_OF_SECTIONS];
            int start_addr;
            int min, max;

            double avrg;

            interval = (a.Length - BASELINE_FILTER_SIZE) / BASELINE_NUMBER_OF_SECTIONS;
            min = int.MaxValue;
            max = int.MinValue;
            avrg = 0;

            for (scan = 0; scan < BASELINE_NUMBER_OF_SECTIONS; scan++)
            {
                start_addr = interval * scan;
                for (i = 0, sum[scan] = 0; i < BASELINE_FILTER_SIZE; i++)
                {
                    sum[scan] += a[start_addr++];
                }

                if (sum[scan] > max)
                    max = sum[scan];
                if (sum[scan] < min)
                    min = sum[scan];

                avrg += sum[scan];
            }

            BubbleSort(ref sum);
            
            avrg = avrg - min - max;
            avrg = (avrg / ((double)BASELINE_NUMBER_OF_SECTIONS - 2.0) +
                    sum[BASELINE_NUMBER_OF_SECTIONS - 2] - sum[1]) / (double)BASELINE_FILTER_SIZE;

            return avrg;
        }

        public void scan_processing_process()
        {
            scan_processing_process(mSamples);
        }

        void scan_processing_process(int [] samples)
        {
            int i;
            int peak, peak_x;
            int left_threshold, right_threshold;
            int intensity_threshold, intensity;
            double baseline;

            double distance = 0;
            double precise_distance = 0;
            int intensity_sum = 0;
            double center_real;
            int center;
            double precise_center;

            peak = 0;
            peak_x = 0;

            /* Calculate baseline */
            baseline = CalculateBaseLine(samples);

            /* Copy data to compressed array and find peak intensity */
            for (i = 0; i < samples.Length; i++)
            {
                if (samples[i] > peak)
                {
                    peak = samples[i];
                    peak_x = i;
                }
            }

            /* Find new threshold with new intensity (middle between peak and baseline) */
            intensity_threshold = (int)(baseline + peak) / 2;
            intensity = peak - (int)baseline;

            /* Check if the intensity is too low */
            if (intensity < MIN_INTENSITY)
            {
                distance = 0;
                precise_distance = distance;
                intensity_sum = 0;
            }

            /* Check if the baseline is too high */
            if(baseline > DARK_BASE_LINE * 1.5)
            {
                distance = 0;
                precise_distance = distance;
                intensity_sum = 0;
            }



            right_threshold = peak_x;
            left_threshold = peak_x;

            for (i = 0; i <= peak_x; i++)
            {
                if (samples[i] < intensity_threshold)
                    left_threshold = i;
                else
                    break;
            }

            for (i = samples.Length - 1; i >= peak_x; i--)
            {
                if (samples[i] < intensity_threshold)
                    right_threshold = i;
                else
                    break;
            }

            int sum = 0;
            /* Calculate average between two borders */
            for(i = left_threshold; i < right_threshold; i++)
            {
                sum += samples[i] - (int)intensity_threshold;
            }

            sum = sum / (right_threshold - left_threshold);



            center = (int)((double)(right_threshold + left_threshold) / 2.0 + 0.5);
            center_real = (double)(right_threshold + left_threshold) / 2.0;
            precise_center = CalculateCenterOfMassEqualDistance(samples, center);


            /* Calculate intensity of the pixel */
            intensity_sum = samples[center];
            if (center > 0)
                intensity_sum += samples[center - 1];
            if (center < samples.Length - 1)
                intensity_sum += samples[center + 1];

            intensity_sum /= 16;

            if (distance <= 0)
            {
                distance = 0;
                intensity_sum = 0;
            }
            precise_distance = distance;

        }

        double CalculateCenterOfMassEqualDistance(int [] samples, int center)
        {
            int i;
            int left_border, right_border;
            int samples_sum = 0;
            int samples_momentum = 0;
            int min = int.MaxValue;
            double center_of_mass;
            int window = 40;


            if (center < window)
            {
                left_border = 0;
            }
            else
            {
                left_border = center - window;
            }

            if ((center + window) >= samples.Length)
            {
                right_border = samples.Length - 1;
            }
            else
            {
                right_border = center + window;
            }

            min = samples[left_border];
            if (min > samples[right_border])
            {
                min = samples[right_border];
            }

            samples_sum = 0;
            samples_momentum = 0;

            for (i = left_border; i <= right_border; i++)
            {
                samples_sum += samples[i] - min;
                samples_momentum += i * (samples[i] - min);
            }

            center_of_mass = ((double)samples_momentum) / ((double)samples_sum);

            return center_of_mass;
        }

        #endregion


    }
}
