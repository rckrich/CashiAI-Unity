using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionBox : MonoBehaviour
{
    public List<GameObject> suggestionsItemList;
    public GameObject suggestionItemPrefab;
    public ChatBoxLogic chatBox;
    public void OnClick_SetQuestion(TextMeshProUGUI textUI){
        chatBox.SuggestionBoxEntryPointSend(textUI.text);
        HideSuggestionBox();
    }

    public void HideSuggestionBox(){
        gameObject.SetActive(false);
    }

    public void Initialize(){

        foreach (GameObject item in suggestionsItemList)
        {
            item.GetComponent<TextInterface>().textUI.text = "";
        }
    }
}
