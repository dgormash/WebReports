using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReports.LiveQueueReport.Interfaces
{
    public interface IHtmlDataExtractor
    {
        string GetAuthenticityToken(string pageContent);
        List<ServiceData> GetServiceData(string pageContent);
        List<ServiceData> GetSubServiceData(string pageContent);
    }
}
