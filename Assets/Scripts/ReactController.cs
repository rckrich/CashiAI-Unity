using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReactController : MonoBehaviour
{
    [DllImport("__Internal")]
  private static extern void isLoadedF ();

    public RectTransform chatContainer;


    void Start(){
        CommunicateToReact();
    }

    public void AdjustSize(int num){
        Debug.Log("Adjusted Size");
        chatContainer.offsetMin = new Vector2(200, 0);
        chatContainer.offsetMax = new Vector2(-200, -64);
    }

    public void CommunicateToReact () {
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            isLoadedF ();
        #endif
    }

}