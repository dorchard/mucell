using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;

namespace UnitTests.View
{
    [TestFixture]
    public class TestTimeSeriesGraphPanel
    {
        private MuCell.View.TimeSeriesGraphPanel graphPanel;

        [TestFixtureSetUp]
        public void initialiseGraphPanel()
        {
            List<MuCell.Model.TimeSeries> tsList = new List<MuCell.Model.TimeSeries>();
            MuCell.Model.TimeSeries timeSeries = new MuCell.Model.TimeSeries("Test series", 0.1);
            timeSeries.addDataPoint(-1d);
            timeSeries.addDataPoint(-0.5d);
            timeSeries.addDataPoint(0.25d);
            timeSeries.addDataPoint(1d);
            timeSeries.addDataPoint(3d);
            timeSeries.addDataPoint(5d);
            timeSeries.addDataPoint(3d);
            timeSeries.addDataPoint(1d);
            timeSeries.addDataPoint(0.25d);
            timeSeries.addDataPoint(-0.5d);
            timeSeries.addDataPoint(-1d);
            tsList.Add(timeSeries);

            graphPanel = new MuCell.View.TimeSeriesGraphPanel(tsList);
            graphPanel.showTimeSeries(0);
        }

        [Test]
        public void zoomedOut()
        {
            // check that the graph area covers the whole graph when fully zoomed out
            graphPanel.setZoom(1.0f);
            graphPanel.setScroll(0.0f);

            // data points from 0 to n-1 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, 1.0, 0.001);

            // y values go from -1 to 5
            Assert.AreEqual(graphPanel.GraphLocalArea.Bottom, -1.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Top, 5.0, 0.001);

            // the panel size is 450x230, so the graph area is 400x200, pixel scale is
            // n=11 points, n-1=10 lines, time interval of each line = 0.1, pixel width = 400
            // ymax - ymin = 5 - -1 = 6, pixel height = 33.3
            Assert.AreEqual(graphPanel.PixelScale.x, (graphPanel.GraphAxes.Width / 10f) / 0.1f);
            Assert.AreEqual(graphPanel.PixelScale.y, (graphPanel.GraphAxes.Height / 6f), 0.2f);

            //Console.Out.WriteLine(graphPanel.GraphLocalArea.ToString());
            //Console.Out.WriteLine(string.Format("Local Area: {0}\nPixel scale: {1}", graphPanel.GraphLocalArea.ToString(), graphPanel.PixelScale.ToString()));
        }

        [Test]
        public void zoomed2x()
        {
            // check that the graph area X only goes up to half the length of the time series
            graphPanel.setZoom(2.0f);
            graphPanel.setScroll(0.0f);

            // data points from 0 to n-1 / 2 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, 0.5, 0.001);

            // y values go from -1 to 5
            Assert.AreEqual(graphPanel.GraphLocalArea.Bottom, -1.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Top, 5.0, 0.001);

            // the panel size is 450x230, so the graph area is 400x200, pixel scale is
            // n=11 points, (n-1)/2=5 lines, time interval of each line = 0.1, pixel width = 800
            // ymax - ymin = 5 - -1 = 6, pixel height = 33.3
            Assert.AreEqual(graphPanel.PixelScale.x, (graphPanel.GraphAxes.Width / 5f) / 0.1f);
            Assert.AreEqual(graphPanel.PixelScale.y, (graphPanel.GraphAxes.Height / 6f), 0.2f);
        }

        [Test]
        public void zoomed2xScroll()
        {
            // check that the graph area width is half the length of the time series
            graphPanel.setZoom(2.0f);
            graphPanel.setScroll(0.2f);

            // data points from 2 to n-1 / 2 + 2 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.2, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, 0.7, 0.001);

            // y values go from -1 to 5 - this doesn't change with zooming and scrolling
            Assert.AreEqual(graphPanel.GraphLocalArea.Bottom, -1.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Top, 5.0, 0.001);

            // the panel size is 450x230, so the graph area is 400x200, pixel scale is
            // n=11 points, (n-1)/2=5 lines, time interval of each line = 0.1, pixel width = 800
            // ymax - ymin = 5 - -1 = 6, pixel height = 33.3
            Assert.AreEqual(graphPanel.PixelScale.x, (graphPanel.GraphAxes.Width / 5f) / 0.1f);
            Assert.AreEqual(graphPanel.PixelScale.y, (graphPanel.GraphAxes.Height / 6f), 0.2);
        }

        [Test]
        public void zoomFloatAmount()
        {
            // check that the graph area is correct when using a float value to zoom
            graphPanel.setZoom(3.5f);
            graphPanel.setScroll(0.0f);

            // data points from 0 to n-1 / 3.5 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, 1.0 / 3.5, 0.001);

            // y values go from -1 to 5 - this doesn't change with zooming and scrolling
            Assert.AreEqual(graphPanel.GraphLocalArea.Bottom, -1.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Top, 5.0, 0.001);

            // the panel size is 450x230, so the graph area is 400x200, pixel scale is
            // n=11 points, (n-1)/3.5=2.86 lines, time interval of each line = 0.1, pixel width = 140
            // ymax - ymin = 5 - -1 = 6, pixel height = 33.3
            Assert.AreEqual(graphPanel.PixelScale.x, (graphPanel.GraphAxes.Width / (10f / 3.5f)) / 0.1f, 0.2f);
            Assert.AreEqual(graphPanel.PixelScale.y, (graphPanel.GraphAxes.Height / 6f), 0.2f);
        }

        [Test]
        public void zoomScrollFloatAmounts()
        {
            // check that the graph area is correct when using float values to zoom and scroll
            graphPanel.setZoom(3.5f);
            graphPanel.setScroll(0.175f);

            // data points from 2 to n-1 / 3.5 + 2 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.175, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, (1.0 / 3.5) + 0.175, 0.001);

            // y values go from -1 to 5 - this doesn't change with zooming and scrolling
            Assert.AreEqual(graphPanel.GraphLocalArea.Bottom, -1.0, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Top, 5.0, 0.001);

            // the panel size is 450x230, so the graph area is 400x200, pixel scale is
            // n=11 points, (n-1)/3.5=2.86 lines, time interval of each line = 0.1, pixel width = 140
            // ymax - ymin = 5 - -1 = 6, pixel height = 33.3
            Assert.AreEqual(graphPanel.PixelScale.x, (graphPanel.GraphAxes.Width / (10f / 3.5f)) / 0.1f, 0.2f);
            Assert.AreEqual(graphPanel.PixelScale.y, (graphPanel.GraphAxes.Height / 6f), 0.2);
        }

        [Test]
        public void noScrollBeyondRightSide()
        {
            // check that you can't set a scroll value greater than n-1 - (n-1 / zoom)
            // in this case, 10 - (10 / 2) = 5
            graphPanel.setZoom(2.0f);
            graphPanel.setScroll(0.7f);

            // data points from 5 to n-1 / 2 + 5 are included
            Assert.AreEqual(graphPanel.GraphLocalArea.Left, 0.5, 0.001);
            Assert.AreEqual(graphPanel.GraphLocalArea.Right, 1.0, 0.001);
        }

        /*
        [Test]
        public void gridLinesYAxis()
        {
            // the y data goes between -1 and 5, so to get a reasonable number of grid lines
            // an interval of 1 should be used
            Assert.AreEqual(graphPanel.GridInterval.y, 1);
        }

        [Test]
        public void gridLinesXAxis()
        {
            // the x data goes between 0 and 1 second, so to get a reasonable number of grid lines
            // an interval of 
            Assert.AreEqual(graphPanel.GridInterval.x, 1);
        }
         */
    }
}
