using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBoxLogic : MonoBehaviour
{
    [SerializeField] private GameObject messageContainer, messageAssistantPrefab, messageUserPrefab;
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private OpenAiWebCalls openAi;


    public void Initialize(){
        openAi.CreateNewThreadInterface();
    }

    void Start()
    {
        Initialize();
    }

    public void OnClick_Send(){
        openAi.SendMessageToThreadInterface(textInput.text);
        textInput.text = "Envia un mensaje a Cashimiro";
    }

    public void OnClick_ClearText(){
        if(textInput.text == "Envia un mensaje a Cashimiro"){
            textInput.text = " ";
        }
    }

    public void ChatEntryPointMessages(OpenAiWebCalls.Message _newMessage){
        GameObject _prefab;
        if(_newMessage.role == "user"){
            _prefab = Instantiate(messageUserPrefab);
        }else{
            _prefab = Instantiate(messageAssistantPrefab);
        }
        _prefab.transform.SetParent(messageContainer.transform);
        _prefab.GetComponent<MessageInterface>().textUI.text = _newMessage.content;
    }



}
