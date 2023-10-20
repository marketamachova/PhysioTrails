using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Analytics;
using UnityEngine;

namespace Utils
{
    public class FileWriter
    {
        private string _path;
        private string _fileName;

        public FileWriter(string fileName, string format, bool addDate, string path = null, string sceneName = "")
        {
            _path = path;
            _fileName = addDate ? $"{fileName}_{sceneName}_{GetCurrentDateTime()}.{format}" : $"{fileName}_{sceneName}_.{format}";
        }

        public string WriteData(string header, string data)
        {
            Debug.Log(_path);

            try
            {
                Directory.CreateDirectory(_path);
                var fullPath = Path.Combine(_path, _fileName);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(header);
                        sw.Write(data);
                        sw.Close();
                        
                        Debug.Log($"File saved successfully. {_fileName} ({_path})");
                        return fullPath;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Saving data to file failed :( " + e);
                return "FileWriter Invalid path";
            }
        }

        public string WriteToTemporaryFile(string data)
        {
            string filePath = Path.Combine(Application.temporaryCachePath, _fileName);
            File.WriteAllText(filePath, data);

            return filePath;
        }

        public string StringifyList(List<string> stringList)
        {
            StringBuilder sb = new StringBuilder();
    
            foreach (string s in stringList)
            {
                sb.AppendLine(s);
            }
    
            return sb.ToString();
        }

        public string StringifyLogsHeader()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Constants.LogHeader header in Enum.GetValues(typeof(Constants.LogHeader)))
            {
                sb.Append(header.ToString()).Append(" ");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();

            return sb.ToString();
        }

        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        public string FileName => _fileName;
    }
}
