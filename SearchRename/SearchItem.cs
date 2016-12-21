using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.SearchRename
{
    public class SearchItem
    {
        bool WasFound = false;
        string FileName = "";
        string FilePath = "";
        string FileConversion = "";
        string FileExtension = "";

        public SearchItem()
        {

        }

        public SearchItem( string filePath, string fileName, bool wasFound )
        {
            setFilePath(filePath);
            setFileName(fileName);
            setWasFound(wasFound);
            setExtension();
        }

        private void setFilePath( string input )
        {
            FilePath = input;
        }

        private void setFileName( string input )
        {
            FileName = input;
        }

        private void setWasFound( bool input )
        {
            WasFound = input;
        }

        private void setExtension()
        {
            if (FilePath != "")
            {
                string[] strArray = FilePath.Split('.');

                if (strArray.Length == 2)
                {
                    FileExtension = "." + strArray[1];
                }
            }
        }

        public string getFilePath()
        {
            return FilePath;
        }

        public string getFileName()
        {
            return FileName;
        }

        public string getFileConversion()
        {
            return FileConversion;
        }

        public bool getWasFound()
        {
            return WasFound;
        }

        public string getFileExtension()
        {
            return FileExtension;
        }
    }
}
