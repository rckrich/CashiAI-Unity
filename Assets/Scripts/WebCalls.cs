using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class WebCalls : MonoBehaviour
{
    public static IEnumerator CR_GetDefaultMessages()
    {
        string jsonResult = "";
        string url = "http://165.232.151.217/api/v1/default-messages";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Protocol Error or Connection Error");
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Default message: " + jsonResult);
                }
            }
        }
    }

    public static IEnumerator CR_PostConversationStarted(string id)
    {
        string jsonResult = "";
        string url = "http://165.232.151.217/api/v1/conversations/started";

        string jsonBody = JsonUtility.ToJson(new { id = id });

        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, "POST")) {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Protocol Error or Connection Error");
                yield break;
            }
            else
            {
                jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Envio");
            }
        }
    }

}