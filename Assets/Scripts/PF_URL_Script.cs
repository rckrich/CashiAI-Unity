using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_URL_Script : MonoBehaviour
{
    public string urlName;
    public string url;

    public void OpenWebsite()
    {
        Application.OpenURL(url);
    }
}
