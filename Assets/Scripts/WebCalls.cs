using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class WebCalls : MonoBehaviour
{

    [System.Serializable]
    public class idRoot
    {
        public int id;
    }


    public SuggestionBox suggestionBox;
    public DefaultMessageRoot jsonProcessed;
    private IEnumerator CR_GetDefaultMessages()
    {
        Debug.Log("aqui");
        string jsonResult = "";
        string url = "http://165.232.151.217/api/v1/default-messages";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Protocol Error or Connection Error");
                suggestionBox.HideSuggestionBox();
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    jsonProcessed = JsonUtility.FromJson<DefaultMessageRoot>(jsonResult);
                    suggestionBox.EntryPointInitializeItemList(jsonProcessed);
                    Debug.Log("Default message: " + jsonResult);
                }
            }
        }
    }

    void Start()
    {
        StartCoroutine(CR_GetDefaultMessages());
    }

    public void PostConversationStarted(int id){
        StartCoroutine(CR_PostConversationStarted(id));
    }

    private IEnumerator CR_PostConversationStarted(int _id)
    {
        string jsonResult = "";
        string url = "http://165.232.151.217/api/v1/conversations/started";

        string jsonBody = JsonUtility.ToJson(new idRoot { id = _id });
        Debug.Log(jsonBody);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonBody,"application/json")) {
            webRequest.SetRequestHeader("Accept", "application/json");
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.result);
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
