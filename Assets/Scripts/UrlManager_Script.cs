using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager_Script : MonoBehaviour
{
    GameObject urlPrefab;
    public Transform instanceParent;

    private (string name, string url)[] urlData = {
        ("Google", "https://www.google.com"),
        ("Unity", "https://unity.com"),
        ("OpenAI", "https://openai.com")
    };
    
    private void Instance()
    {
        foreach (var data in urlData) { 
            GameObject instance = Instantiate(urlPrefab, instanceParent);
            PF_URL_Script urlScript = instance.GetComponent<PF_URL_Script>();
            if (urlScript != null) { 
                urlScript.urlName = data.name;
                urlScript.url = data.url;
                instance.name = "Url:" + data.name;
            }
            else
            {
                Debug.LogWarning("PF_URL_Scripts no encontrado");
            }
        }
    } 


}
