using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PF_URL_Script : MonoBehaviour
{
    public string urlName;
    public string url;
    public TMP_Text urlNameText;

    public void OpenWebsite()
    {
        Application.OpenURL(url);
    }
}
