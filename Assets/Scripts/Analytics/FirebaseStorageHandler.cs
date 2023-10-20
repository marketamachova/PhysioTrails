using System;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using Utils;

namespace Analytics
{
    public class FirebaseStorageHandler
    {
        public async void UploadFile(List<string> data, string sceneName)
        {
            var fileWriter = new FileWriter(Constants.FileName, Constants.FormatTXT, true, Constants.LogDirectoryName, sceneName);
            var path = fileWriter.WriteData(fileWriter.StringifyLogsHeader(), fileWriter.StringifyList(data));
            Debug.Log("path " + path);

            // Create storage reference
            var storage = FirebaseStorage.DefaultInstance;
            var fileRef = storage.GetReference($"/hmd-tracking/{fileWriter.FileName}");
            
            Uri uri = new Uri(path);
            string uriPath = uri.AbsoluteUri;

            //var uri = new System.Uri(path).ToString();
            
            // Upload the file to Firebase Storage
            await fileRef.PutFileAsync(uriPath).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to upload file to Firebase Storage:((((((((( " + task.Exception);
                    return;
                }
            
                Debug.Log("Firebase Storage: File uploaded successfully!!!!!!!!!");
            });
        }
    }
}
