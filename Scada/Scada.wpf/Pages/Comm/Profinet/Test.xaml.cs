#define Diper

using Scada.core;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Scada.wpf.Pages.Windows;

namespace Scada.wpf.Pages.Comm.Profinet
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        #region [Definations]

        Thread readTestThread;
        int DBNumber, StartAddress;
        string StrAddress;
        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;

        #endregion

        #region [Constructure]

        public Test()
        {
            InitializeComponent();
            TestStop(); //- Start on stop status
        }

        #endregion

        #region [UserControl]

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

           
        }

        #endregion

        #region [Connection Control]

        private void btn_TestConnect_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btn_TestDisconnect_Click(object sender, RoutedEventArgs e)
        {
          
        }

        #endregion

 
        /// <summary>
        /// Toggle Start/Stop Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TestStart_Click(object sender, RoutedEventArgs e)
        {


        }

        /// <summary>
        /// Test Start Method
        /// </summary>
        private void TestStart()
        {
            readTestThread = new Thread(new ThreadStart(ReadTest));
            readTestThread.IsBackground = true;
            //if (readTestThread != null && !readTestThread.IsAlive) 
            
            btn_TestStart.Content = "Read Stop";
            notifyCall = new NotificationPanelCall("Test", "Test started. Address : [" + StrAddress + "]", StatusColor.Success, 3);
            readTestThread.Start();
        }

        /// <summary>
        /// Test Stop Method
        /// </summary>
        private void TestStop()
        {
            if (readTestThread != null && readTestThread.IsAlive) readTestThread.Abort();
            btn_TestStart.Content = "Read Start";
          
        }

        /// <summary>
        /// Thread Method
        /// </summary>
        private void ReadTest()
        {
            try
            {
                bool threadRun = true;
                while (threadRun)
                {
                    try
                    {
                        Lbl_TestResult.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                                notifyCall = new NotificationPanelCall("Test", "Unable to read data from [" + StrAddress + "]", StatusColor.Error, 3);
                                threadRun = false;
                                TestStop();
                            
                        });
                        Thread.Sleep(200);
                    }
                    catch (ThreadAbortException)
                    {
                        threadRun = false;
                    }
                }
            }
            finally
            {

            }

        }

    }
}
