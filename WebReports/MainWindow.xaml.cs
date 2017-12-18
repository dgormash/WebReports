using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WebReports.Common;
using WebReports.ConcreteClasses;
using WebReports.LiveQueueReport;

namespace WebReports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void liveQueueReport_Click(object sender, RoutedEventArgs e)
        {
            //var envChecker = new LqrPreStartOperations();
            //await envChecker.CheckEnviroment();

            var ipReader = new QueuesXmlReader();

            var servers = ipReader.GetIpListFromXml($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\LiveQueueReport\\queues.xml");

            var threadExecutor = new LqrThreadExecutor(new LiveQueueXlsReporterCreator());

            foreach (var server in servers) //Цикл выполняется 8 раз
            {
                threadExecutor.Ip = server.Ip;
                 await threadExecutor.ExecuteInNewThread();
            }

            threadExecutor.CreateReport();
            MessageBox.Show("Отчёт по живой очереди сформирован.");

        }
    }
}
