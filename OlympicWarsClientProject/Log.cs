using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace OlympicWarsClientProject
{
    public class Log
    {
        private static string path;

        public Log()
        {
            path = GetPath();
        }

        public void Add(string message)
        {

            CreateDirectory();
            string nameFile = GetNameFile();
            string stringLog = "";

            stringLog += DateTime.Now + " - " + message + Environment.NewLine;

            StreamWriter streamWriter = new StreamWriter(path + "/" + nameFile, true);
            streamWriter.Write(stringLog);
            streamWriter.Close();

        }

        private string GetNameFile()
        {
            string nameFile;

            nameFile = "log_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".txt";

            return nameFile;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetPath()
        {
            string currentDirectory = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\" + @"Exceptions\")));

            return directory.ToString();
        }
    }
}
