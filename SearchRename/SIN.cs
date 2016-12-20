using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Search_and_Rename
{
    class SIN
    {
        private String[] SINParts;
        private String FileNamePath = "";
        private String StockItemNumber = "";
        private String ActualPath = "";
        private String CorrectPath = "";
        private Boolean IsGood = false;

        public SIN()
        { }

        public SIN(String Input, String Directory)
        {
            if (Input != "")
            {
                setFileNamePath(Input);
                setSIN(Input);
                setCorrectPath(Directory);
                setIsGood();
            }
        }

        private void setFileNamePath(String input)
        {
            this.FileNamePath = input;  
        }

        private void setSIN(String input)
        {
            this.StockItemNumber = Path.GetFileName(input);
            this.StockItemNumber = this.StockItemNumber.Replace("\\", "");

            if ((this.StockItemNumber.Contains("-")) || (this.StockItemNumber.Contains("_")))
            {
                this.StockItemNumber.Replace("-", "_");
                this.SINParts = this.StockItemNumber.Split('_');
            }
        }

        public void setActualPath(String input)
        {
            this.ActualPath = Path.GetDirectoryName(input);
        }

        private void setCorrectPath(String Directory)
        {
            CorrectPath = Directory;
            CorrectPath += "\\";

            if ((StockItemNumber.Contains("ADJ")) || (StockItemNumber.Contains("ADY")) || (StockItemNumber.Contains("ADI")) || (StockItemNumber.Contains("ADW")))
            {
                CorrectPath += "ADJUSTABLE";
            }
            //648 658 668 678 700 718 728 738 748 758 768 778 800
            else if ((StockItemNumber.Contains("648")) || (StockItemNumber.Contains("658")) || (StockItemNumber.Contains("668")) ||
                     (StockItemNumber.Contains("678")) || (StockItemNumber.Contains("700")) || (StockItemNumber.Contains("718")) ||
                     (StockItemNumber.Contains("728")) || (StockItemNumber.Contains("738")) || (StockItemNumber.Contains("748")) ||
                     (StockItemNumber.Contains("758")) || (StockItemNumber.Contains("768")) || (StockItemNumber.Contains("778")) ||
                     (StockItemNumber.Contains("800")))
            {
                CorrectPath += "FITTED";
            }
            else if (StockItemNumber.Contains("FTX"))
            {
                CorrectPath += "FITX";
            }
            else if ((StockItemNumber.Contains("_UKT_")) || (StockItemNumber.Contains("-UKT-")) || (StockItemNumber.Contains("_CKT_")) || (StockItemNumber.Contains("-CKT-")) ||
                    (StockItemNumber.Contains("_CKY_")) || (StockItemNumber.Contains("-CKY-")) || (StockItemNumber.Contains("_KTY_")) || (StockItemNumber.Contains("-KTY-")) ||
                    (StockItemNumber.Contains("-BOG-")) || (StockItemNumber.Contains("_BOG_")) || (StockItemNumber.Contains("_YBG_")) || (StockItemNumber.Contains("-YBG-")) ||
                    (StockItemNumber.Contains("-YRK-")) || (StockItemNumber.Contains("_YRK_")) || (StockItemNumber.Contains("_KTI_")) || (StockItemNumber.Contains("-KTI-")) ||
                    (StockItemNumber.Contains("_RVR_")) || (StockItemNumber.Contains("-RVR-")) || (StockItemNumber.Contains("_SCF_")) || (StockItemNumber.Contains("-SCF-")))
            {
                CorrectPath += "KNITS";
            }
            //One Fits with FITX are saved elsewhere.
            else if (((StockItemNumber.Contains("1FT")) || (StockItemNumber.Contains("_BUK_") || (StockItemNumber.Contains("-BUK-") || (StockItemNumber.Contains("1FL")) || (StockItemNumber.Contains("1FY")) || (StockItemNumber.Contains("1FI")))
                     && (!(StockItemNumber.Contains("FITX"))))))
            {
                CorrectPath += "ONE FIT";
            }
            else if (((StockItemNumber.Contains("1FT")) || (StockItemNumber.Contains("1FL")) || (StockItemNumber.Contains("1FY")) || (StockItemNumber.Contains("1FI")))
                     && (StockItemNumber.Contains("FITX")))
            {
                CorrectPath += "FITX";
            }
            else if ((StockItemNumber.Contains("-VSR-")) || (StockItemNumber.Contains("_VSR_")) || (StockItemNumber.Contains("-VSY-")) || (StockItemNumber.Contains("_VSY_")))
            {
                CorrectPath += "VISOR";
            }
            else
            {
                CorrectPath += "OTHER";
            }

            CorrectPath += "\\";

            if (SINParts != null)
            {
                CorrectPath += SINParts[0];
                CorrectPath += "\\";
            }

            if ((StockItemNumber.Contains("_BK") || (StockItemNumber.Contains("-BK")) || (StockItemNumber.Contains("-bk")) || (StockItemNumber.Contains("_bk")) ||
                (StockItemNumber.Contains("-bK")) || (StockItemNumber.Contains("_bK"))))
            {
                CorrectPath += "BACK";
            }
            else
            {
                CorrectPath += "FRONT";
            }
        }

        public void setIsGood()
        {
            if (ActualPath == CorrectPath)
            {
                this.IsGood = true;
            }
        }

        public String getSIN()
        {
            return this.StockItemNumber;
        }

        public String getActualPath()
        {
            return this.ActualPath;
        }

        public String getCorrectPath()
        {
            return this.CorrectPath;
        }

        public Boolean SINIsGood()
        {
            return this.IsGood;
        }
    }
}
