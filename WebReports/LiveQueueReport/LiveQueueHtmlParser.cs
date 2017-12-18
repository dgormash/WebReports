using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebReports.LiveQueueReport
{
    public class LiveQueueHtmlParser
    {
        public string Ip { get; set; }

        private  HttpClient _client;
        private string _authenticityToken;
        private  string _pageContent;
        private readonly Queue _liveQueue = new Queue();
        public async Task<Queue> ParseQueueData()
        {
            _client = new HttpClient();
            _pageContent = await GetPageContentGetRq($"http://{Ip}/login");

            _authenticityToken = GetAuthenticityToken(_pageContent);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("utf8", char.ToString('\u2713')),
                new KeyValuePair<string, string>("authenticity_token", _authenticityToken),
                new KeyValuePair<string, string>("user[email]", "monitoring_roio@admin.ru"),
                new KeyValuePair<string, string>("user[password]", "monitoring_roiomonitoring_roio"),
                new KeyValuePair<string, string>("user[remember_me]", "0")
            });

            _pageContent = await GetPageContentPostRq(formContent, $"http://{Ip}/login");

            _authenticityToken = GetAuthenticityToken(_pageContent);

            formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("utf8", char.ToString('\u2713')),
                new KeyValuePair<string, string>("authenticity_token", _authenticityToken),
                new KeyValuePair<string, string>("counter_id", "admin"),
                new KeyValuePair<string, string>("counter_busy", "0")
            });

            _pageContent = await GetPageContentPostRq(formContent, $"http://{Ip}/operator/confirm_counter");

            _pageContent = await GetPageContentGetRq($"http://{Ip}/admin/services");

            _liveQueue.SetDepartmentNameByItsIp(Ip);

            var services = GetServiceData(_pageContent);
            //AddLiveQueueInfo(GetServiceData(_pageContent));
            foreach (var service in services)
            {
                _liveQueue.Services.Add(new Service
                {
                    Id = service.ServiceId,
                    Code = service.ServiceCode,
                    Name = service.ServiceName,
                    State = service.SerivceStatus
                });
            }


            foreach (var service in _liveQueue.Services)
            {
                _pageContent = await GetPageContentGetRq($"http://{Ip}/admin/services/{service.Id}");
                switch (service.Code)
                {
                    //Экзамены и ВУ
                    case "10000001193":
                        var exams = GetSubServiceData(_pageContent);
                        foreach (var examServices in exams)
                        {
                            _liveQueue.ExamSubServices.Add(new Service
                            {
                                Id = examServices.ServiceId,
                                Code = examServices.ServiceCode,
                                Name = examServices.ServiceName,
                                State = examServices.SerivceStatus
                            });
                        }
                        
                        break;
                    //Регистрация транспорта
                    case "10000004558":
                        var regs = GetSubServiceData(_pageContent);
                        foreach (var regServices in regs)
                        {
                            _liveQueue.RegSubServices.Add(new Service
                            {
                                Id = regServices.ServiceId,
                                Code = regServices.ServiceCode,
                                Name = regServices.ServiceName,
                                State = regServices.SerivceStatus
                            });
                        }
                        break;
                }

            }

            _client.Dispose();
            return _liveQueue;
        }


        private  async Task<string> GetPageContentGetRq(string url)
        {
            var httpResponseMessage = await _client.GetAsync(url);

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            httpResponseMessage.Dispose();

            return result;
        }

        private  async Task<string> GetPageContentPostRq(FormUrlEncodedContent form, string url)
        {

            var httpResponseMessage = await _client.PostAsync(url, form);

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            httpResponseMessage.Dispose();

            return result;
        }

        private  string GetAuthenticityToken(string httpPageContent)
        {
            var result = string.Empty;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(httpPageContent);

            var inputNodes = htmlDocument.DocumentNode.SelectNodes("//input[contains(@name, 'authenticity_token')]");
            foreach (var node in inputNodes)
            {
                var autheticityTokenInput = node.Attributes["value"];
                result = autheticityTokenInput.Value;
            }

            return result;
        }

        private List<ServiceData> GetServiceData(string httpPageContent)
        {
            var result = new List<ServiceData>();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(httpPageContent);

            foreach (var table in htmlDocument.DocumentNode.SelectNodes("//table"))
            {
                foreach (var row in table.SelectNodes("//tr[contains(@class, 'success')]"))
                {
                    var name = row.SelectSingleNode("td[3]").InnerText;
                    var state = row.SelectSingleNode("td[4]/div/a[contains(@class, 'active')]/span").InnerText == "Вкл"
                        ? "+"
                        : "-";
                    var id = row.SelectSingleNode("td[1]").InnerText;
                    var code = row.SelectSingleNode("td[2]").InnerText;
                    result.Add(new ServiceData
                    {
                        ServiceId = id,
                        ServiceCode = code,
                        ServiceName = name,
                        SerivceStatus = state
                    });
                }
            }

            return result;
        }

        private  List<ServiceData> GetSubServiceData(string httpPageContent)
        {
            var result = new List<ServiceData>();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(httpPageContent);

            foreach (var table in htmlDocument.DocumentNode.SelectNodes("//table"))
            {
                foreach (var row in table.SelectNodes("//tr[contains(@class, 'success')]"))
                {
                    var name = row.SelectSingleNode("td[4]").InnerText;
                    var state = row.SelectSingleNode("td[5]/div/a[contains(@class, 'active')]/span").InnerText == "Вкл"
                        ? "+"
                        : "-";
                    var id = row.SelectSingleNode("td[2]").InnerText;
                    var code = row.SelectSingleNode("td[3]").InnerText;
                    result.Add(new ServiceData
                    {
                        ServiceId = id,
                        ServiceCode = code,
                        ServiceName = name,
                        SerivceStatus = state
                    });
                }
            }

            return result;
        }
    }
}