namespace WebReports.Common
{
    public interface IFileSystemManager
    {
        string ResultMessage { get; set; }
        void Create(string path);
    }
}