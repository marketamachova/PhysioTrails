using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class FileWriter
    {
        private string _path;
        private string _fileName;

        public FileWriter(string path, string fileName, string format, bool addDate)
        {
            _path = path;
            _fileName = addDate ? $"{fileName}_{GetCurrentDateTime()}.{format}" : $"{fileName}.{format}";
        }

        public void WriteData(string data)
        {
            Debug.Log(_path);

            try
            {
                Directory.CreateDirectory(_path);

                using (FileStream stream = new FileStream(Path.Combine(_path, _fileName), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(data);
                        sw.Close();
                        
                        Debug.Log($"File saved successfully. {_fileName} ({_path})");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Saving data to file failed :( " + e);
            }
        }

        public string ParseList(List<string> stringList)
        {
            StringBuilder sb = new StringBuilder();
    
            foreach (string s in stringList)
            {
                sb.AppendLine(s);
            }
    
            return sb.ToString();
        }

        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }
    }
}
