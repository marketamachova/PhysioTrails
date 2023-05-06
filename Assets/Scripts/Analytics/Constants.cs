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
        
        public enum LogHeader
        {
            Time = 0,
            HeadPositionX,
            HeadPositionY,
            HeadPositionZ,
            HeadRotationX,
            HeadRotationY,
            HeadRotationZ,
            LeftHandPositionX,
            LeftHandPositionY,
            LeftHandPositionZ,
            LeftHandRotationX,
            LeftHandRotationY,
            LeftHandRotationZ,
            RightHandPositionX,
            RightHandPositionY,
            RightHandPositionZ,
            RightHandRotationX,
            RightHandRotationY,
            RightHandRotationZ
        }
    }
}
