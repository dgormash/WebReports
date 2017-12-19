using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebReports.LiveQueueReport.Interfaces;

namespace WebReports.LiveQueueReport.Implementation
{
    public class HtmlDataExtractor:IHtmlDataExtractor
    {
        public string GetAuthenticityToken(string pageContent)
        {
            if (string.IsNullOrWhiteSpace(pageContent))
            {
                throw  new ArgumentException("HtmlDataExtractor.GetAuthenticityToken: отсутствует содержание html-страницы.");
            }

            var result = string.Empty;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageContent);

            var inputNodes = htmlDocument.DocumentNode.SelectNodes("//input[contains(@name, 'authenticity_token')]");
            if (inputNodes != null)
            {
                foreach (var node in inputNodes)
                {
                    var autheticityTokenInput = node.Attributes["value"];
                    result = autheticityTokenInput.Value;
                }
            }
            else
            {
                return null;
            }

            return result;

            return result;
        }

        public List<ServiceData> GetServiceData(string pageContent)
        {
            throw new NotImplementedException();
        }

        public List<ServiceData> GetSubServiceData(string pageContent)
        {
            throw new NotImplementedException();
        }
    }
}
