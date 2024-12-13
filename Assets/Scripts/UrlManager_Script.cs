using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UrlData
{
    public string name;
    public string url;
}
public class UrlManager_Script : MonoBehaviour
{
    public GameObject urlPrefab;
    public Transform instanceParent;

    [SerializeField]
    private UrlData[] urlData = {
        new UrlData { name = "Google", url = "https://www.google.com" },
        new UrlData { name = "Unity", url = "https://unity.com" },
        new UrlData { name = "OpenAI", url = "https://openai.com" }
    };

    void Start()
    {
        Instance();
    }
    private void Instance()
    {
        foreach (var data in urlData) { 
            GameObject instance = Instantiate(urlPrefab, instanceParent);
            PF_URL_Script urlScript = instance.GetComponent<PF_URL_Script>();
            if (urlScript != null) {
                urlScript.urlNameText.text = data.name;
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
