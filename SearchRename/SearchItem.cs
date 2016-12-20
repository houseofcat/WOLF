using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.SearchRename
{
    public class SearchItem
    {
        Boolean WasFound = false;
        String FileName = "";
        String FilePath = "";
        String FileConversion = "";
        String FileExtension = "";

        public SearchItem()
        {

        }

        public SearchItem(String filePath, String fileName, Boolean wasFound)
        {
            setFilePath(filePath);
            setFileName(fileName);
            setWasFound(wasFound);
            setExtension();
        }

        private void setFilePath(String input)
        {
            this.FilePath = input;
        }

        private void setFileName(String input)
        {
            this.FileName = input;
        }

        private void setWasFound(Boolean input)
        {
            this.WasFound = input;
        }

        private void setExtension()
        {
            if (FilePath != "")
            {
                String[] strArray = FilePath.Split('.');

                if (strArray.Length == 2)
                {
                    this.FileExtension = "." + strArray[1];
                }
            }
        }

        public String getFilePath()
        {
            return this.FilePath;
        }

        public String getFileName()
        {
            return this.FileName;
        }

        public String getFileConversion()
        {
            return this.FileConversion;
        }

        public Boolean getWasFound()
        {
            return this.WasFound;
        }

        public String getFileExtension()
        {
            return this.FileExtension;
        }
    }
}
