using System.Collections.Generic;
using System.Windows.Documents.DocumentStructures;

namespace WebReports.LiveQueueReport
{
    public class ServerList
    {
        private List<string> _servers;
        public List<string> Servers { get; set; }

        public ServerList()
        {
            _servers = new List<string>();
        }
    }
}