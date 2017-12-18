using System.Collections.Generic;

namespace WebReports.LiveQueueReport
{
    public class Queue
    {
        public string Department { get; set; }
        public List<Service> Services { get; set; }
        public List<Service> RegSubServices { get; set; }
        public List<Service>ExamSubServices { get; set; }

        public Queue()
        {
            Department = string.Empty;
            Services = new List<Service>();
            RegSubServices = new List<Service>();
            ExamSubServices = new List<Service>();
        }

        public void SetDepartmentNameByItsIp(string ip)
        {
            switch (ip)
            {
                case "10.196.44.70":
                    Department = "Чебаркуль";
                    break;
                case "10.196.37.160":
                    Department = "Троицк";
                    break;
                case "10.196.17.245":
                    Department = "Коркино";
                    break;
                case "10.196.77.190":
                    Department = "Златоуст";
                    break;
                case "10.196.21.81":
                    Department = "Кыштым";
                    break;
                case "10.196.28.34":
                    Department = "Миасс";
                    break;
                case "10.196.36.180":
                    Department = "Сосновка";
                    break;
                case "10.196.100.167":
                    Department = "Магнитогорск";
                    break;
                //case "10.196.144.124":
                //    Department = "Челябинск";
                //    break;
            }
        }
    }
}