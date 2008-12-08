using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MuCell.Model;
using System.IO;

namespace MuCell.View
{
    /// <summary>
    /// A non-modal window to be used to display raw time series data and allow exporting this data to a file
    /// </summary>
    /// <owner>Jonathan</owner>
    public partial class TimeSeriesRawDataViewer : Form
    {
        /// <summary>
        /// Creates the window and fills the text box with the data from the time series provided
        /// </summary>
        /// <param name="timeSeries">A list of time series to be displayed as columns in the window</param>
        public TimeSeriesRawDataViewer(List<TimeSeries> timeSeries)
        {
            InitializeComponent();

            if (timeSeries.Count > 0)
            {
                // find the time interval that should be used for each row in the data
                // it must be common to all of the time series
                decimal minCommonTimeInterval = (decimal)timeSeries[0].Parameters.TimeInterval;

                for (int i = 1; i < timeSeries.Count; i++)
                {
                    minCommonTimeInterval = gcd((decimal)timeSeries[i].Parameters.TimeInterval, minCommonTimeInterval);
                }

                int[] timeIntervalMultiples = new int[timeSeries.Count];
                double maxSeconds = 0;

                for (int i = 0; i < timeSeries.Count; i++)
                {
                    // find the number of times each time series' time interval divides into the common interval
                    // this will be used to offset each data point by the correct number of rows later
                    timeIntervalMultiples[i] = (int)Math.Floor(decimal.Divide((decimal)timeSeries[i].Parameters.TimeInterval, minCommonTimeInterval));

                    // find the length of the run from the maximum of all the time series
                    if (timeSeries[i].Series.Count * timeSeries[i].Parameters.TimeInterval > maxSeconds)
                    {
                        maxSeconds = timeSeries[i].Series.Count * timeSeries[i].Parameters.TimeInterval;
                    }
                }

                if (maxSeconds > 0)
                {
                    // there is data to be drawn

                    int maxDataPoints = (int)Math.Floor(decimal.Divide((decimal)maxSeconds, minCommonTimeInterval));
                    string[] data = new string[maxDataPoints + 2 + timeSeries.Count];

                    // write the formulas used in the first few lines
                    for (int i = 0; i < timeSeries.Count; i++)
                    {
                        data[i] = timeSeries[i].Parameters.Name +
                            " = " +
                            timeSeries[i].Parameters.FunctionString +
                            " (" + timeSeries[i].Parameters.Units + ")";
                    }

                    int dataOffset = timeSeries.Count + 2;

                    // create the first column using the minimum time interval
                    data[dataOffset - 1] = "Time";
                    for (int i = 0; i < maxDataPoints; i++)
                    {
                        data[i + dataOffset] = string.Format("{0}", i * minCommonTimeInterval);
                    }

                    // create the series data for each time series column
                    for (int ts = 0; ts < timeSeries.Count; ts++)
                    {
                        // column header
                        data[dataOffset - 1] += "\t" + timeSeries[ts].Parameters.Name;

                        // iterate through each of the data values in the time series
                        for (int i = 0; i < timeSeries[ts].Series.Count; i++)
                        {
                            // write the value in the row according to how many times
                            // this time series' time interval divides into the common interval
                            data[(i * timeIntervalMultiples[ts]) + dataOffset] += string.Format("\t{0:0.0000000000}", timeSeries[ts].Series[i]);

                            // fill the remaining rows with dashes
                            for (int j = 1; j < timeIntervalMultiples[ts]; j++)
                            {
                                data[((i * timeIntervalMultiples[ts]) + j) + dataOffset] += "\t- - - - - -";
                            }
                        }

                        // if the data array is longer than the time series, fill the remaining rows with blanks
                        for (int i = timeSeries[ts].Series.Count; i < maxDataPoints; i++)
                        {
                            data[i + dataOffset] += "\t";
                        }
                    }

                    textBox1.Lines = data;
                }
                else
                {
                    textBox1.Text = "No data";
                }
            } else {

                textBox1.Text = "No data";
            }
        }

        /// <summary>
        /// The greatest common divisor of two decimals - dividing decimals is more accurate than dividing doubles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private decimal gcd(decimal a, decimal b)
        {
            if (a == b)
                return a;

            decimal r = -1;

            while (r != 0)
            {
                r = decimal.Remainder(a, b);
                a = b;
                b = r;
            }

            return a;
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            saveDataDialog.ShowDialog();
            if (saveDataDialog.FileName != "")
            {
                // user chose a file to write

                TextWriter fileWriter = new StreamWriter(saveDataDialog.FileName);

                // iterate through data rows in text box to write to the file
                foreach (string line in textBox1.Lines)
                {
                    if (saveDataDialog.FilterIndex == 1)
                    {
                        // chosen comma separated variable file
                        // replace tabs with commas
                        fileWriter.WriteLine(line.Replace('\t', ','));
                    }
                    else
                    {
                        fileWriter.WriteLine(line);
                    }
                }

                fileWriter.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}