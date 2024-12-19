using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxLogic : MonoBehaviour
{
    [SerializeField] private GameObject messageContainer, messageAssistantPrefab, messageUserPrefab, sidebarContainer;
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private OpenAiWebCalls openAi;
    [SerializeField] private SuggestionBox suggestionBox;
    [SerializeField] private ReactController reactController;
    private bool hasSendFirstMessage = false;

    public void Initialize(){
        openAi.CreateNewThreadInterface();
        //suggestionBox.Initialize();
    }

    void Start()
    {
        Initialize();
    }

    public void Onclick_ToggleSidebar(){
        sidebarContainer.SetActive(!sidebarContainer.activeSelf);
    }

    public void OnClick_Send(){
        if(textInput.text == "" || textInput.text == " "){
            return;
        }
        suggestionBox.HideSuggestionBox();
        openAi.SendMessageToThreadInterface(textInput.text);
        CreateUserMessage(textInput.text);
        textInput.text = "";
        if(!hasSendFirstMessage){
            reactController.sendPost();
            hasSendFirstMessage = true;
        }
    }

    private void CreateUserMessage(string messageUser){
        GameObject _prefab;
        _prefab = Instantiate(messageUserPrefab);
        _prefab.transform.SetParent(messageContainer.transform);
        _prefab.GetComponent<TextInterface>().textUI.text = messageUser;
        _prefab.transform.localScale = new Vector3(1,1,1);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_prefab.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer.GetComponent<RectTransform>());

    }

    public void SuggestionBoxEntryPointSend(string text){
        openAi.SendMessageToThreadInterface(text);
        CreateUserMessage(text);
        if(!hasSendFirstMessage){
            reactController.sendPost();
            hasSendFirstMessage = true;
        }
    }

    public void ChatEntryPointMessages(String _newMessage){
        GameObject _prefab;
        _prefab = Instantiate(messageAssistantPrefab);
        _prefab.transform.SetParent(messageContainer.transform);
        _prefab.GetComponent<TextInterface>().textUI.text = _newMessage;
        _prefab.transform.localScale = new Vector3(1,1,1);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_prefab.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer.GetComponent<RectTransform>());
    }



}
