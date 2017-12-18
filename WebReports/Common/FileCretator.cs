namespace WebReports.Common
{
    public class FileCretator:IFileSystemManager
    {
        public string ResultMessage { get; set; }

        public FileCretator()
        {
            ResultMessage = "Ok";
        }
        public void Create(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}