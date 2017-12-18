using System;
using System.IO;
using System.Windows;

namespace WebReports.Common
{
    public class DirectoryCreator:IFileSystemManager
    {
        public string ResultMessage { get; set; }

        public DirectoryCreator()
        {
            ResultMessage = "Ok";
        }
        public void Create(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
               ResultMessage = e.Message;
            }
        }
    }
}