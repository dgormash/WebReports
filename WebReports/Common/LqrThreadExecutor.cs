using System.Collections.Generic;
using System.Threading.Tasks;
using WebReports.Abstractions;
using WebReports.ConcreteClasses;
using WebReports.LiveQueueReport;

namespace WebReports.Common
{
    public class LqrThreadExecutor:IThreadExecutor
    {
        public string Ip { get; set; }
        //public AbstractReporterCreator ReporterCreator { get; set; }
        private readonly LiveQueueHtmlParser _parser;
        private readonly AbstractReporter _reporter;
        private readonly List<Queue> _queues; 

        public LqrThreadExecutor(AbstractReporterCreator reporterCreator)
        {
            _parser = new LiveQueueHtmlParser();
            _reporter = reporterCreator.CreateReporter();
            _queues = new List<Queue>();
        }

        public async Task ExecuteInNewThread()
        {
            _parser.Ip = Ip;
            _queues.Add(await _parser.ParseQueueData());
            
        }

        public void CreateReport()
        {
            var rep = (LiveQueueXlsReporter)_reporter;
            rep.CreateReport(_queues);
        }

    }
}