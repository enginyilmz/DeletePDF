
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DeletePDF
{
    public class FileManager
    {
        SimpleLogger logger = new SimpleLogger(true);
        public FileManager()
        {
        }
        public void FindAndDelete()
        {
            PathContainer.Prepare();
            DateTime checkDay = GetCheckDay();
            foreach (string itemPath in PathContainer.pathContainer)
            {
                logger.Info("Market :" + itemPath);
                string[] fileList = Directory.GetFiles(itemPath).Where(x => new FileInfo(x).CreationTime.Date < checkDay).ToArray();
                DeleteFile(fileList);
            }
        }
        DateTime GetCheckDay()
        {
            int AddDay = Convert.ToInt32(string.IsNullOrEmpty(ConfigurationManager.AppSettings["AddDay"].ToString()) ? "0" : ConfigurationManager.AppSettings["AddDay"].ToString());
            return DateTime.Now.AddDays(-1 * AddDay);
        }
        public void DeleteFile(string[] fileList)
        {
            foreach (var item in fileList)
            {
                string curFilePath = item;
                if (File.Exists(curFilePath))
                {
                    try
                    {
                        logger.Info("Deleted :" + curFilePath);
                        File.Delete(curFilePath);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Deleted Exception :{0}; \n{1}; \n{2}", curFilePath, ex.Message, ex.InnerException.Message));
                    }
                    
                }
            }
        }
    }

    public class PathContainer
    {
        public static List<string> pathContainer = new List<string>();
        public PathContainer()
        {
        }

        public static void Prepare()
        {
            pathContainer.Clear();

            PreparePathContainer();
        }

        private static void AddPath(string path)
        {
            pathContainer.Add(path);
        }

        private static void PreparePathContainer()
        {
            string[] PathArray = 
            { 
                ConfigurationManager.AppSettings["PathFi"].ToString(), 
                ConfigurationManager.AppSettings["PathDe"].ToString(), 
                ConfigurationManager.AppSettings["PathNo"].ToString(), 
                ConfigurationManager.AppSettings["PathSe"].ToString(), 
                ConfigurationManager.AppSettings["Test"].ToString() 
            };
            foreach (var item in PathArray)
            {
                if (Directory.Exists(item))
                {
                    AddPath(item);
                }
            }
        }
    }
}
