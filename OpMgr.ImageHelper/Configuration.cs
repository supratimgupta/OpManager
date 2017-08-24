using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OpMgr.ImageHelper
{
    public static class Configuration
    {
        public class ImageColumnNameFolder
        {
            public string ColumnName { get; set; }

            public string FolderPath { get; set; }
        }

        public static string TEMP_TABLE_QUERY
        {
            get
            {
                return ConfigurationManager.AppSettings["TEMP_TABLE_QUERY"];
            }
        }

        public static string TEMPLATE_COLUMN
        {
            get
            {
                return ConfigurationManager.AppSettings["TEMPLATE_COLUMN"];
            }
        }

        public static string IMAGE_EXTN
        {
            get
            {
                return ConfigurationManager.AppSettings["IMAGE_EXTN"];
            }
        }

        public static IEnumerable<ImageColumnNameFolder> IMAGE_COLUMN_NAME_FOLDER
        {
            get
            {
                string[] imgColNames = ConfigurationManager.AppSettings["IMAGE_COLUMN_NAME_FOLDER"].Split(',');
                foreach(string imgColName in imgColNames)
                {
                    ImageColumnNameFolder oImgColName = new ImageColumnNameFolder();
                    oImgColName.ColumnName = imgColName.Split('~')[0];
                    oImgColName.FolderPath = imgColName.Split('~')[1];
                    yield return oImgColName;
                }
            }
        }
    }
}
