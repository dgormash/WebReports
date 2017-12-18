using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using WebReports.Common;

namespace WebReports.LiveQueueReport
{
    public class LqrPreStartOperations
    {
        public Task CheckEnviroment()
        {
            return Task.Run(() =>
            {
                var executableDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var dirCreator = new DirectoryCreator();

                if (!Directory.Exists($"{executableDir}\\LiveQueueReport"))
                {
                    dirCreator.Create($"{executableDir}\\LiveQueueReport");
                }

                if (dirCreator.ResultMessage == "Ok")
                {
                    if (!File.Exists($"{executableDir}\\LiveQueueReport\\LqrSettings.xml"))
                    {
                        //todo Создать файл LqrSettings.xml в каталоге LiveQueueReport
                    }
                }
                else
                {
                    MessageBox.Show(dirCreator.ResultMessage, "Viva la Resistance!!!");
                }
            });
        }
    }
}