using System.Threading.Tasks;
using WebReports.Abstractions;
using WebReports.ConcreteClasses;
using WebReports.LiveQueueReport;

namespace WebReports.Common
{
    public class LqrThreadExecutor:IThreadExecutor
    {
        public string Ip { get; set; }
        public async Task ExecuteInNewThread()
        {
            var parser = new LiveQueueHtmlParser {Ip = Ip};
            Queue result =  await parser.ParseQueueData();

            AbstractReporterCreator reporterCreator= new LiveQueueXlsReporterCreator();
            AbstractReporter reporter =  reporterCreator.CreateReporter();
            var rep = (LiveQueueXlsReporter) reporter;
            rep.CreateReport(result);
        }
    }
}