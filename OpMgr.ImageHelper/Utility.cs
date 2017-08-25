using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.ImageHelper
{
    public class Utility : AbsUtility
    {
        private AbsDataHelper _dataHelper;

        public Utility(AbsDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public override void ChangeImage()
        {
            using(DataTable dtData = _dataHelper.GetDataFromTemp())
            {
                List<Configuration.ImageColumnNameFolder> lstImgColName = Configuration.IMAGE_COLUMN_NAME_FOLDER.ToList();
                //string imageExtn = Configuration.IMAGE_EXTN;
                string templateColName = Configuration.TEMPLATE_COLUMN;

                string folderPath = null;
                string imageNamePattern = null;
                string[] files = null;
                string templateValue = null;

                string[] arrFileNames = null;
                string extn = null;
                string targetPath = null;
                string imageName = null;
                for(int i=0;i<dtData.Rows.Count;i++)
                {
                    try
                    {
                        if (lstImgColName != null)
                        {
                            
                            for (int j = 0; j < lstImgColName.Count; j++)
                            {
                                try
                                {
                                    folderPath = lstImgColName[j].FolderPath;
                                    if(!string.IsNullOrEmpty(folderPath))
                                    {
                                        imageName = dtData.Rows[i][lstImgColName[j].ColumnName].ToString();
                                        if (!string.IsNullOrEmpty(imageName) && !string.IsNullOrWhiteSpace(imageName))
                                        {
                                            imageNamePattern = imageName + ".*";
                                            files = Directory.GetFiles(folderPath, imageNamePattern);
                                            templateValue = dtData.Rows[i][templateColName].ToString().Replace('/', '_').Replace('\\', '_');
                                            foreach (string file in files)
                                            {
                                                arrFileNames = file.Split('.');
                                                extn = arrFileNames[arrFileNames.Length - 1];
                                                targetPath = Path.Combine(folderPath, templateValue + "." + extn);
                                                File.Move(file, targetPath);
                                            }
                                        }
                                    }
                                }
                                finally { }
                            }
                        }
                    }
                    finally { }
                }
            }
        }
    }
}
