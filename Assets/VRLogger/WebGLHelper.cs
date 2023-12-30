using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRLogger
{
    public class WebGLHelper
    {

        public WebGLData GetWebGlData(string customServerUrl)
        {
            var webGlData = new WebGLData();
            
            var hostedUrl = Application.absoluteURL;
            Debug.Log("Hosted URL: " + hostedUrl);
            
            if (customServerUrl != "")
            {
                webGlData.ServerUrl = customServerUrl;
            }
            else
            {
                SetServerUrl(hostedUrl, webGlData);
            }

            SetParameters(hostedUrl, webGlData);

            return webGlData;
        }
        
        // Reading Server URL from hosted URL
        private void SetServerUrl(string url, WebGLData webGLData)
        {
            var hostnameParts = url.Split('/');
        
            webGLData.ServerUrl = hostnameParts[0] + "/" + hostnameParts[1] + "/" + hostnameParts[2];
        
            Debug.Log("Server url: " + webGLData.ServerUrl);
        }


        // Getting url parameters
        private void SetParameters(string url, WebGLData webGLData)
        {
            var parametersString = url.Split("?")[1];
            var parameters = parametersString.Split("&&");
            var paramsDict = new Dictionary<string, string>();
            foreach (var param in parameters)
            {
                Debug.Log("Parameter: " + param);
                var sepParam = param.Split("=");
                paramsDict.Add(sepParam[0], sepParam[1]);
            }

            if (paramsDict.ContainsKey("application_identifier") && paramsDict.ContainsKey("organisation_code") &&
                paramsDict.ContainsKey("activity_id"))
            {
                webGLData.ApplicationIdentifier = paramsDict["application_identifier"];
                webGLData.OrganisationCode = paramsDict["organisation_code"];
                webGLData.ActivityId = paramsDict["activity_id"];
                webGLData.EnvironmentId = paramsDict["environment_id"];
            }
            else
            {
                throw new Exception("Wrong parameters!");
            }
        }
        
    }

    public class WebGLData
    {
        public string ServerUrl;
        public string ApplicationIdentifier;
        public string OrganisationCode;
        public string ActivityId;
        public string EnvironmentId;
        
    }
}