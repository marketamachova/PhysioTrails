using System;
using System.IO;
using UnityEngine;

namespace Analytics
{
    public class Constants
    {
        #if UNITY_EDITOR
        public static string DataPath = Application.dataPath.Replace("/", "\\");
        #elif UNITY_ANDROID
            public static string DataPath = Application.persistentDataPath;
        #endif

        public static string LogDirectoryName = Path.Combine(DataPath, "Logs");
        public static string FileName = "PhysioTrails";
        public static string FormatTXT = "txt";
    }
}
