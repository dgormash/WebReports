using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
namespace WebReports.LiveQueueReport
{
    public class QueuesXmlReader
    {
        private List<WebServer> _ipList;

        public List<WebServer> GetIpListFromXml(string path)
        {
            if (!File.Exists(path))
            {
                //todo обработка ситуации с отсутствующим файлом
            }
            else
            {
                var document = new HtmlDocument();
                document.Load(path);

                _ipList = new List<WebServer>();
                foreach (var ip in document.DocumentNode.SelectNodes("servers/ip"))
                {
                    _ipList.Add(new WebServer {Ip = ip.InnerText});
                }
            }

            return _ipList;
        }
    }
}