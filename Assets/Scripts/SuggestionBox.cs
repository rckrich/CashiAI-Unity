using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionBox : MonoBehaviour
{
    [SerializeField] private GameObject suggestionItemPrefab;
    [SerializeField] private ChatBoxLogic chatBox;

    public void OnClick_SetQuestion(TextMeshProUGUI textUI){
        chatBox.SuggestionBoxEntryPointSend(textUI.text);
        HideSuggestionBox();
    }

    public void HideSuggestionBox(){
        gameObject.SetActive(false);
    }

    public void EntryPointInitializeItemList(DefaultMessageRoot _json){
        if(_json.defaultMessages.Count == 0){
            HideSuggestionBox();
        }else{
            foreach (DefaultMessage item in _json.defaultMessages)
            {
                Debug.Log(item.content);
                GameObject _prefab;
                    _prefab = Instantiate(suggestionItemPrefab);
                    _prefab.transform.SetParent(gameObject.transform);
                    _prefab.GetComponent<TextInterface>().textUI.text = item.content;
                    _prefab.transform.localScale = new Vector3(1,1,1);
                    _prefab.GetComponent<TextInterface>()._parent = this;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_prefab.GetComponent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
            }
        }
        
    }
}
