using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReactController : MonoBehaviour
{
    [DllImport("__Internal")]
  private static extern void isLoadedF ();

    [SerializeField] private WebCalls webInterface;
    [SerializeField] private RectTransform chatContainer;
    [SerializeField] private RectTransform suggestionBox;
    private string userId = "";

    void Awake () {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    WebGLInput.captureAllKeyboardInput = false;
#endif
  }

    void Start(){
        CommunicateToReact();
    }

    public void reactWebCall(string id){
        userId = id;
    }

    public void sendPost(){
        if(userId != ""){
            webInterface.PostConversationStarted(userId);
        }
    }

    public void AdjustSize(int num){
        Debug.Log("Adjusted Size");
        suggestionBox.offsetMin = new Vector2(50, 91);
        suggestionBox.offsetMax = new Vector2(-50, 0);
        chatContainer.offsetMin = new Vector2(200, 0);
        chatContainer.offsetMax = new Vector2(-200, -64);
    }

    public void CommunicateToReact () {
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            isLoadedF ();
        #endif
    }

}
