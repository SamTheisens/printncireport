using System;
using System.Media;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PrintNCIService;
using PrintNCIAgent.Properties;
using TimeoutException=System.ServiceProcess.TimeoutException;

namespace PrintNCIAgent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly ServiceController controller;
        private readonly AlarmScreen notifier;
        private Timer timer;
        private string statusQueue;
        private string tracerQueue;

        private delegate void Notify(string printer, string message);
        private delegate void CloseNotifier();
        public MainWindow()
        {
            notifier = new AlarmScreen();
            InitializeComponent();
            //notifier.Show();
            Hide();
            controller = new ServiceController("PrintNCI");
            timer = new Timer(Check, null, 3000, Settings.Default.PollingFrequency);
        }
        private void Check(object sender)
        {
            var factory = new ChannelFactory<IPrintService>(new WSHttpBinding(),
                                                            new EndpointAddress(
                                                                "http://localhost:8000/PrintNCIService/service"));
            try
            {
                var channel = factory.CreateChannel();
                CheckQueue(channel);
            }
            catch (CommunicationException cx)
            {
                factory.Abort();
            }
            catch (TimeoutException tx)
            {
                factory.Abort();
            }
            catch (Exception ex)
            {
                factory.Abort();
            }
            if (factory.State != CommunicationState.Faulted)
            {
                factory.Close();
            }
        }

        private void CheckQueue(IPrintService channel)
        {
            var newStatusQueue = channel.GetStatusQueue();
            if (newStatusQueue != statusQueue)
            {
                statusQueue = newStatusQueue;
                if (newStatusQueue != string.Empty)
                    notify("Printer Status", statusQueue);
            }
            var newTracerQueue = channel.GetTracerQueue();
            if (newTracerQueue != tracerQueue)
            {
                tracerQueue = newTracerQueue;
                if (newTracerQueue != string.Empty)
                    notify("Printer Tracer", tracerQueue);
            }
            if (tracerQueue == string.Empty && statusQueue == string.Empty)
                notifier.Dispatcher.BeginInvoke(DispatcherPriority.Background, new CloseNotifier(CloseDispatcher));
        }

        private void notify(string printer, string status)
        {
            notifier.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Notify(NotifyDispatcher), printer, status);
        }
        public void CloseDispatcher()
        {
            notifier.Hide();
        }
        public void NotifyDispatcher(string printer, string message)
        {
            notifier.Show();
            notifier.NotifyContent.Add(new NotifyObject(message, printer));
            notifier.Notify();
            var wavPlayer = new SoundPlayer { SoundLocation = "alert.wav" };
            wavPlayer.PlaySync();
            
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            //controller.Stop();
            
            
         //   MessageBox.Show(string.Format("Status Queue: {0}", channel.GetStatusQueue()));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //controller.Start();
            
            //MessageBox.Show(string.Format("Tracer Queue: {0}", channel.GetTracerQueue()));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotifyIcon_MouseClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
