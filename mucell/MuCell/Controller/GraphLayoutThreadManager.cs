using System;
using System.Collections.Generic;
using System.Text;
using MuCell.View;
using System.Threading;
using System.Windows.Forms;

namespace MuCell.Controller
{
    /// <summary>
    /// A singleton class to run and monitor a graph rearrangement thread
    /// </summary>
    class GraphLayoutThreadManager
    {
        class RearrangeThread
        {
            private GraphLayoutManager rearranger;
            private Model.SBML.Model model;
            private IDrawingInterface drawable;
            private Thread thread;

            public RearrangeThread(GraphLayoutManager rearranger, Model.SBML.Model model, IDrawingInterface drawable)
            {
                this.rearranger = rearranger;
                this.model = model;
                this.drawable = drawable;
            }
            public Thread getThread()
            {
                return thread;
            }
            public void setThread(Thread thread)
            {
                this.thread = thread;
            }

            public void run()
            {
                MacroCommand rearrangeCommand=rearranger.rearrangeGraphFromModel(model, drawable);
                getGraphLayoutThreadManager().rearrangeThreadFinished(rearrangeCommand);
            }
            public void stop()
            {
                if (rearranger.isRunning())
                {
                    rearranger.stopRunning();
                }
            }
        }

        private static GraphLayoutThreadManager threadManager;

        public static GraphLayoutThreadManager getGraphLayoutThreadManager()
        {
            if (threadManager == null)
            {
                threadManager = new GraphLayoutThreadManager();
            }
            return threadManager;
        }



        private RearrangeThread thread;
        private bool finishing;
        private IGraphRearrangementListener listener;
        public GraphLayoutThreadManager()
        {
            thread = null;
            finishing = false;
        }
        public void setListener(IGraphRearrangementListener listener)
        {
            this.listener = listener;
        }
        public void stopRearrangeThread()
        {
            if (thread != null)
            {
                thread.stop();
            }
        }
        /// <summary>
        /// Ask to finish the thread for clean up when closing the window.  Returns true if it will call back later.
        /// </summary>
        /// <returns></returns>
        public bool cleanUpThreads()
        {
            finishing = true;
            if (thread != null)
            {
                thread.stop();
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void rearrangeThreadFinished(MacroCommand command)
        {
            if (listener != null)
            {
                listener.rearrangementComplete(command);
            }
            thread = null;
            Console.WriteLine("Rearrange thread finished");
            if (finishing)
            {
                Console.WriteLine("Rearrange thread cleaned up");
                Application.Exit();
            }
        }
        public void rearrangeGraphFromModel(Model.SBML.Model model, IDrawingInterface drawable)
        {
            if (!finishing)
            {
                if (thread != null)
                {
                    thread.stop();
                    Console.WriteLine("Rearrange thread already running");
                }

                GraphLayoutManager rearranger = new GraphLayoutManager_SpringEmbedder();

                thread = new RearrangeThread(rearranger, model, drawable);

                Thread newThread = new Thread(new ThreadStart(thread.run));

                thread.setThread(newThread);

                newThread.Start();
            }
        }
    }
}
