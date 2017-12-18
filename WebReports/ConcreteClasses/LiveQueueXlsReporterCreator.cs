using WebReports.Abstractions;

namespace WebReports.ConcreteClasses
{
    public class LiveQueueXlsReporterCreator:AbstractReporterCreator
    {
        public override AbstractReporter CreateReporter()
        {
            return new LiveQueueXlsReporter();
        }
    }
}