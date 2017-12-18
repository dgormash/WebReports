using System.Threading.Tasks;

namespace WebReports.Common
{
    public interface IThreadExecutor
    {
        Task ExecuteInNewThread();
    }
}