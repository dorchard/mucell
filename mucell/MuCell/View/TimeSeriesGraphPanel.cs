using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MuCell.Model;

namespace MuCell.View
{
    /// <summary>
    /// 
    /// </summary>
    /// <owner>Jonathan</owner>
    public partial class TimeSeriesGraphPanel : UserControl
    {
        private List<Model.TimeSeries> allTimeSeries;

        private Pen axesPen;
        private Pen gridLinesPen;
        private Pen[] dataPens;
        private Font labelFont;

        private int selectedSeries = -1;
        private int toolTipSeries = -1;

        private float zoom = 1.0f;
        /// <summary>
        /// This is the number of seconds into the series that the current drawing should be offset by
        /// </summary>
        private float scroll = 0.0f;

        private System.Drawing.Rectangle graphAxes;
        /// <summary>
        /// The area in pixels in which the graph will be drawn relative to the picture box
        /// </summary>
        public System.Drawing.Rectangle GraphAxes { get { return graphAxes; } }

        private MuCell.Model.Rectangle graphLocalArea;
        /// <summary>
        /// The area in graph coordinates from which data will be drawn.
        /// This changes depending on zoom and scroll
        /// Note, the Left and Width values are now based on seconds
        /// </summary>
        public MuCell.Model.Rectangle GraphLocalArea { get { return graphLocalArea; } }

        private Model.Vector2 pixelScale;
        /// <summary>
        /// The scale by which the data points drawn should be multiplied in order to fill the axes.
        /// </summary>
        public Model.Vector2 PixelScale { get { return pixelScale; } }

        private Model.Vector2 gridInterval;
        /// <summary>
        /// The interval in seconds or units between grid lines
        /// </summary>
        public Model.Vector2 GridInterval { get { return gridInterval; } }

        private Model.Vector2 labelInterval;

        /// <summary>
        /// The amount scroll should change by with each move of the scroll bar
        /// </summary>
        private double minTimeInterval = 1.0f;

        #region Initialisation

        public TimeSeriesGraphPanel(List<Model.TimeSeries> timeSeries)
        {
            initialise();
            this.allTimeSeries = timeSeries;
            this.Name = "Group graph";

            foreach (TimeSeries ts in allTimeSeries)
            {
                listOfTimeSeries.Items.Add(ts);
                txtGraphUnits.Text = "Y axis: " + ts.Parameters.Units;
            }
            createPens(allTimeSeries.Count);
            showTimeSeries(0);
            setZoom(1); // calls updategraph
        }

        private void initialise()
        {
            InitializeComponent();
            allTimeSeries = new List<MuCell.Model.TimeSeries>();

            // initialise pens
            axesPen = new Pen(Color.Black, 1.0f);
            gridLinesPen = new Pen(Color.Gray, 1.0f);

            labelFont = new Font(FontFamily.GenericMonospace, 8);

            zoom = 1;
            this.MouseWheel += new MouseEventHandler(TimeSeriesGraphPanel_MouseWheel);

            this.listOfTimeSeries.ContextMenuStrip = menuSetColour;

        }

        #endregion

        #region Zoom

        public void setZoom(float newZoom)
        {
            if (newZoom < 1.0f)
                return;

            zoom = newZoom;
            lblZoomAmount.Text = zoom.ToString() + " x";
            updateGraph();
        }


        void TimeSeriesGraphPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomIn();
            }
            if (e.Delta < 0)
            {
                zoomOut();
            }
        }

        private void zoomIn()
        {
            setZoom(zoom + 0.5f);
        }

        private void zoomOut()
        {
            setZoom(zoom - 0.5f);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            zoomIn();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            zoomOut();
        }

        #endregion

        #region Scroll
        // --------------------------------------------------------------------

        private void graphScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            setScroll(graphScrollBar.Value * (float)minTimeInterval);
        }


        public void setScroll(float newScroll)
        {
            if (newScroll < 0.0f)
                return;

            scroll = newScroll;
            //console.WriteLine("setScroll: " + scroll);
            //lblCoordinates.Text = scroll.ToString();
            updateGraph();
        }


        #endregion

        #region Data controls


        private void listOfTimeSeries_MouseDown(object sender, MouseEventArgs e)
        {
            selectedSeries = listOfTimeSeries.IndexFromPoint(e.X, e.Y);
        }

        private void listOfTimeSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGraph();
        }

        public void showTimeSeries(int index)
        {
            if (index < listOfTimeSeries.Items.Count && !listOfTimeSeries.SelectedIndices.Contains(index))
            {
                listOfTimeSeries.SetSelected(index, true);
            }
        }

        private void listOfTimeSeries_MouseMove(object sender, MouseEventArgs e)
        {
            toolTipSeries = listOfTimeSeries.IndexFromPoint(e.X, e.Y);
        }

        private void listOfTimeSeries_MouseHover(object sender, EventArgs e)
        {
            if (toolTipSeries >= 0)
            {
                toolTipNameTooLong.SetToolTip(listOfTimeSeries, listOfTimeSeries.Items[toolTipSeries].ToString());
            }
            else
            {
                toolTipNameTooLong.SetToolTip(listOfTimeSeries, "");
            }

        }


        private void picGraphBox_MouseMove(object sender, MouseEventArgs e)
        {
            // TODO get this to calculate y coordinate to the nearest time series
            if (graphAxes.Contains(e.Location) && pixelScale.x > 0 && pixelScale.y > 0)
            {
                double actualX = ((e.Location.X - graphAxes.Left) / pixelScale.x) + graphLocalArea.Left;
                double actualY = ((graphAxes.Bottom - e.Location.Y) / pixelScale.y) + graphLocalArea.Bottom;
                lblXCoordinate.Text = "X: " + actualX.ToString();
                lblYCoordinate.Text = "Y: " + actualY.ToString();
            }
            else
            {
                lblXCoordinate.Text = "X:";
                lblYCoordinate.Text = "Y:";
            }
        }


        private void btnShowData_Click(object sender, EventArgs e)
        {
            List<TimeSeries> timeSeries = new List<TimeSeries>();
            foreach (TimeSeries ts in listOfTimeSeries.SelectedItems)
            {
                timeSeries.Add(ts);
            }
            TimeSeriesRawDataViewer dataViewer = new TimeSeriesRawDataViewer(timeSeries);
            dataViewer.Show();
        }


        #endregion

        #region Colours

        private void createPens(int numPens)
        {
            Color[] defaultColours = { Color.FromArgb(255, 0, 0), // red
                Color.FromArgb(0, 255, 255),    // cyan
                Color.FromArgb(0, 255, 0),      // green
                Color.FromArgb(0, 0, 255),      // blue
                Color.FromArgb(255, 128, 64),   // orange
                Color.FromArgb(255, 0, 255),    // pink
                Color.FromArgb(192, 192, 192),  // grey
                Color.FromArgb(128, 128, 64),   // khaki
                Color.FromArgb(128, 0, 0),      // dark red
                Color.FromArgb(0, 128, 128),    // dark cyan
                Color.FromArgb(0, 128, 0),      // dark green
                Color.FromArgb(0, 0, 128),      // dark blue
                Color.FromArgb(0, 0, 0),        // black
                Color.FromArgb(128, 128, 0),    // purple
                Color.FromArgb(128, 128, 128),  // dark grey
                Color.FromArgb(128, 0, 255)     // violet
            };

            dataPens = new Pen[numPens];

            int i = 0;
            Random r = new Random();

            while (i < numPens && i < defaultColours.Length)
            {
                // add default pens
                dataPens[i] = new Pen(defaultColours[i], 1f);
                i++;
            }

            while (i < numPens)
            {
                // add random pens, but not too light
                dataPens[i] = new Pen(Color.FromArgb(r.Next(200), r.Next(200), r.Next(200)), 1f);
                i++;
            }
        }

        private void setColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.Color = Color.Beige;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                dataPens[selectedSeries].Color = colorDialog.Color;
                updateGraph();
            }
        }

        private void menuSetColour_Opening(object sender, CancelEventArgs e)
        {
            if (selectedSeries < 0)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region Updating and drawing the graph


        public void updateGraph()
        {
            double maxSeconds = 0;
            double aggregateTimeSeriesMin = double.MaxValue;
            double aggregateTimeSeriesMax = double.MinValue;
            minTimeInterval = 1.0;

            if (listOfTimeSeries.SelectedItems.Count > 0)
            {
                // calculate the maxima for all the visible time series, so the graph will be drawn
                // at the size of the largest one.
                foreach (TimeSeries timeSeries in listOfTimeSeries.SelectedItems)
                {
                    if ((timeSeries.Series.Count - 1) * timeSeries.Parameters.TimeInterval > maxSeconds)
                        maxSeconds = (timeSeries.Series.Count - 1) * timeSeries.Parameters.TimeInterval;

                    if (timeSeries.MaxValue > aggregateTimeSeriesMax)
                        aggregateTimeSeriesMax = timeSeries.MaxValue;

                    if (timeSeries.MinValue < aggregateTimeSeriesMin)
                        aggregateTimeSeriesMin = timeSeries.MinValue;

                    if (timeSeries.Parameters.TimeInterval < minTimeInterval)
                        minTimeInterval = timeSeries.Parameters.TimeInterval;
                }
                // stop divide by zero errors by having some height if all values are the same
                if (aggregateTimeSeriesMax == aggregateTimeSeriesMin)
                {
                    aggregateTimeSeriesMin -= 0.005;
                    aggregateTimeSeriesMax += 0.005;
                }

                // this is the visible drawing area, based on the size of the picture box
                graphAxes = new System.Drawing.Rectangle(70, 10, picGraphBox.Width - 80, picGraphBox.Height - 30);

                if (maxSeconds > 0)
                {
                    // constrain scroll to be less than (maxSeconds - (maxSeconds / zoom))
                    // because maxSeconds / zoom is the width of the view on screen
                    // and you can't scroll further to the right than this.
                    double maxScroll = maxSeconds - (maxSeconds / zoom);

                    // scroll bar changes by one per scroll, so scroll should change by minTimeInterval
                    // and maximum should be maxScroll / minTimeInterval

                    graphScrollBar.Maximum = (int)Math.Floor(maxScroll / minTimeInterval) + graphScrollBar.LargeChange;
                    if (scroll > maxScroll)
                    {
                        //scroll = (float)Math.Floor((maxSeconds - (maxSeconds / zoom)) / minTimeInterval) * (float)minTimeInterval;
                        scroll = (float)maxScroll;
                        graphScrollBar.Value = (int)(scroll / minTimeInterval);
                    }

                    graphLocalArea = new MuCell.Model.Rectangle(
                        scroll, aggregateTimeSeriesMin,
                        (maxSeconds / zoom), aggregateTimeSeriesMax - aggregateTimeSeriesMin);
                    pixelScale = new MuCell.Model.Vector2((float)(graphAxes.Width / graphLocalArea.Width), (float)(graphAxes.Height / graphLocalArea.Height));

                    // change the gridline interval so there aren't too many or too few on the graph
                    gridInterval = new MuCell.Model.Vector2(1.0f, 1.0f);

                    gridInterval.x = (float)Math.Pow(10, Math.Floor(Math.Log10(graphLocalArea.Width)));
                    if (graphLocalArea.Width < gridInterval.x * 5)
                    {
                        if (graphLocalArea.Width < gridInterval.x * 1.5)
                        {
                            gridInterval.x /= 4f;
                        }
                        else
                        {
                            gridInterval.x /= 2f;
                        }
                    }

                    gridInterval.y = (float)Math.Pow(10, Math.Floor(Math.Log10(graphLocalArea.Height)));
                    if (graphLocalArea.Height < gridInterval.y * 5)
                    {
                        if (graphLocalArea.Height < gridInterval.y * 1.5)
                        {
                            gridInterval.y /= 4f;
                        }
                        else
                        {
                            gridInterval.y /= 2f;
                        }
                    }

                    labelInterval = new MuCell.Model.Vector2(gridInterval.x, gridInterval.y);
                }
                else
                {
                    graphLocalArea = new MuCell.Model.Rectangle(0, 0, 0, 0);
                }
            }
            picGraphBox.Refresh();
        }

        private void picGraphBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (listOfTimeSeries.SelectedItems.Count > 0)
            {

                if (graphLocalArea.Width > 0 && pixelScale.x > 0)
                {

                    // draw gridlines

                    // sometimes the precision of doubles means an infinite loop is generated while drawing gridlines
                    int errorLineCount = 0;

                    // Y axis lines
                    for (float gridPoint = (float)(Math.Ceiling(graphLocalArea.Bottom / gridInterval.y) * gridInterval.y); gridPoint <= graphLocalArea.Top; gridPoint += gridInterval.y)
                    {
                        errorLineCount++;
                        if (errorLineCount > 200)
                            return;

                        g.DrawLine(gridLinesPen,
                            (float)(graphAxes.Left),
                            (float)(graphAxes.Bottom - ((gridPoint - graphLocalArea.Bottom) * pixelScale.y)),
                            (float)(graphAxes.Right),
                            (float)(graphAxes.Bottom - ((gridPoint - graphLocalArea.Bottom) * pixelScale.y)));
                    }

                    // Y axis labels
                    for (float gridPoint = (float)(Math.Ceiling(graphLocalArea.Bottom / labelInterval.y) * labelInterval.y); gridPoint <= graphLocalArea.Top; gridPoint += labelInterval.y)
                    {
                        errorLineCount++;
                        if (errorLineCount > 200)
                            return;
                        
                        // TODO make the label round correctly
                        double timeLabel = gridPoint;
                        string label;
                        if (Math.Abs(timeLabel) >= 10000 || (Math.Abs(timeLabel) < 0.001 && timeLabel != 0))
                        {
                            label = string.Format("{0:e2}", timeLabel);
                        }
                        else
                        {
                            label = string.Format("{0:#.###}", timeLabel);
                        }
                        g.DrawString(label, labelFont, Brushes.Black,
                            (float)(graphAxes.Left - g.MeasureString(label, labelFont).Width),
                            (float)(graphAxes.Bottom - ((gridPoint - graphLocalArea.Bottom) * pixelScale.y) - (g.MeasureString(label, labelFont).Height * 0.5f)));
                    }

                    // X axis lines
                    for (float gridPoint = (float)(Math.Ceiling(graphLocalArea.Left / gridInterval.x) * gridInterval.x); gridPoint <= graphLocalArea.Right; gridPoint += gridInterval.x)
                    {
                        errorLineCount++;
                        if (errorLineCount > 200)
                            return;

                        g.DrawLine(gridLinesPen,
                            (float)(graphAxes.Left + ((gridPoint - graphLocalArea.Left) * pixelScale.x)),
                            (float)(graphAxes.Top),
                            (float)(graphAxes.Left + ((gridPoint - graphLocalArea.Left) * pixelScale.x)),
                            (float)(graphAxes.Bottom));
                    }

                    // X axis labels
                    for (float gridPoint = (float)(Math.Ceiling(graphLocalArea.Left / labelInterval.x) * labelInterval.x); gridPoint <= graphLocalArea.Right; gridPoint += labelInterval.x)
                    {
                        errorLineCount++;
                        if (errorLineCount > 200)
                            return;

                        // TODO make the label round correctly
                        double timeLabel = gridPoint; // Math.Round(gridPoint, 3);
                        string label;
                        if (Math.Abs(timeLabel) >= 10000 || (Math.Abs(timeLabel) < 0.001 && timeLabel != 0))
                        {
                            label = string.Format("{0:0.##e+##}", timeLabel);
                        }
                        else
                        {
                            label = string.Format("{0:0.###}", timeLabel);
                        }
                        g.DrawString(label, labelFont, Brushes.Black,
                            (float)(graphAxes.Left + ((gridPoint - graphLocalArea.Left) * pixelScale.x) - (g.MeasureString(label, labelFont).Width * 0.5f)),
                            (float)(graphAxes.Bottom));
                    }

                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // draw data
                    for (int i = 0; i < allTimeSeries.Count; i++)
                    {
                        TimeSeries timeSeries = allTimeSeries[i];
                        if (listOfTimeSeries.SelectedItems.Contains(timeSeries))
                        {
                            double timeSeriesLocalAreaRight = Math.Min(graphLocalArea.Right, (timeSeries.Series.Count - 1) * timeSeries.Parameters.TimeInterval);
                            double timeSeriesLocalAreaLeft = Math.Min(graphLocalArea.Left, (timeSeries.Series.Count - 1) * timeSeries.Parameters.TimeInterval);

                            // use these variables to accumulate the line width
                            // to avoid drawing lines thinner than two pixels
                            int dataPointBegin = (int)Math.Ceiling(graphLocalArea.Left / timeSeries.Parameters.TimeInterval);
                            float leftXPoint = (float)((dataPointBegin * timeSeries.Parameters.TimeInterval) - graphLocalArea.Left) * pixelScale.x;
                            float leftYValue = (float)(timeSeries.Series[dataPointBegin] - graphLocalArea.Bottom) * pixelScale.y;
                            float rightXPoint = -1;
                            float rightYValue = 0;

                            // draw data lines
                            for (int dataPoint = (int)Math.Ceiling(graphLocalArea.Left / timeSeries.Parameters.TimeInterval) + 1; dataPoint <= (int)Math.Floor(timeSeriesLocalAreaRight / timeSeries.Parameters.TimeInterval); dataPoint++)
                            {
                                try
                                {

                                    rightXPoint = (float)((dataPoint * timeSeries.Parameters.TimeInterval) - graphLocalArea.Left) * pixelScale.x;
                                    rightYValue = (float)(timeSeries.Series[dataPoint] - graphLocalArea.Bottom) * pixelScale.y;

                                    if (rightXPoint - leftXPoint >= 2)
                                    {
                                        // only draw the line if the accumulated width is greater than 2 pixels
                                        g.DrawLine(dataPens[i % dataPens.Length],
                                            graphAxes.Left + leftXPoint,
                                            graphAxes.Bottom - leftYValue,
                                            graphAxes.Left + rightXPoint,
                                            graphAxes.Bottom - rightYValue);

                                        // move the left hand point of the next line to match the end point of this one
                                        leftXPoint = rightXPoint;
                                        leftYValue = rightYValue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Warning while drawing: " + ex.Message);
                                }
                            }

                            if (rightXPoint > leftXPoint)
                            {
                                // draw any line at the end which hasn't been finished
                                g.DrawLine(dataPens[i % dataPens.Length],
                                    (float)(graphAxes.Left + leftXPoint),
                                    (float)(graphAxes.Bottom - leftYValue),
                                    (float)(graphAxes.Left + rightXPoint),
                                    (float)(graphAxes.Bottom - rightYValue));
                            }

                            // if scroll isn't an integer, there will be line fragments to the left and right that need drawing
                            if (graphLocalArea.Left < Math.Ceiling(graphLocalArea.Left / timeSeries.Parameters.TimeInterval) * timeSeries.Parameters.TimeInterval)
                            {
                                double localAreaLeft = graphLocalArea.Left / timeSeries.Parameters.TimeInterval;

                                if (localAreaLeft < timeSeries.Series.Count - 1)
                                {
                                    double dataYLeft = timeSeries.Series[(int)Math.Floor(localAreaLeft)];
                                    double dataYRight = timeSeries.Series[(int)Math.Ceiling(localAreaLeft)];
                                    double dataYIntersect = dataYLeft + ((dataYRight - dataYLeft) * (localAreaLeft - Math.Floor(localAreaLeft)));

                                    // draw from the intersect with the left edge to the leftmost data point
                                    g.DrawLine(dataPens[i % dataPens.Length],
                                        (float)(graphAxes.Left),
                                        (float)(graphAxes.Bottom - (((float)dataYIntersect - graphLocalArea.Bottom) * pixelScale.y)),
                                        (float)(graphAxes.Left +
                                            ((((int)Math.Ceiling(localAreaLeft) * timeSeries.Parameters.TimeInterval) - graphLocalArea.Left) * pixelScale.x)),
                                        (float)(graphAxes.Bottom - (((float)dataYRight - graphLocalArea.Bottom) * pixelScale.y)));
                                }
                            }


                            if (timeSeriesLocalAreaRight > Math.Floor(timeSeriesLocalAreaRight / timeSeries.Parameters.TimeInterval) * timeSeries.Parameters.TimeInterval)
                            {
                                double localAreaRight = timeSeriesLocalAreaRight / timeSeries.Parameters.TimeInterval;
                                double dataYLeft = timeSeries.Series[(int)Math.Floor(localAreaRight)];
                                double dataYRight = timeSeries.Series[(int)Math.Ceiling(localAreaRight)];
                                double dataYIntersect = dataYLeft + ((dataYRight - dataYLeft) * (localAreaRight - Math.Floor(localAreaRight)));

                                // draw from the rightmost data point to the intersection with the right edge

                                g.DrawLine(dataPens[i % dataPens.Length],
                                    (float)(graphAxes.Left +
                                        ((((int)Math.Floor(localAreaRight) * timeSeries.Parameters.TimeInterval) - graphLocalArea.Left) * pixelScale.x)),
                                    (float)(graphAxes.Bottom - (((float)dataYLeft - graphLocalArea.Bottom) * pixelScale.y)),

                                    (float)(graphAxes.Right),
                                    (float)(graphAxes.Bottom - (((float)dataYIntersect - graphLocalArea.Bottom) * pixelScale.y)));

                            }
                        }
                    }
                }
            }

            // draw graph axes
            g.DrawLine(axesPen, graphAxes.Left, graphAxes.Top, graphAxes.Left, graphAxes.Bottom);
            g.DrawLine(axesPen, graphAxes.Left, graphAxes.Bottom, graphAxes.Right, graphAxes.Bottom);

        }


        private void picGraphBox_Resize(object sender, EventArgs e)
        {
            updateGraph();
        }



        #endregion

 






    }
}
