using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInterface : MonoBehaviour
{
    public SuggestionBox _parent;
    public TextMeshProUGUI textUI;

    public void OnClickItem(){
        _parent.OnClick_SetQuestion(textUI);
    }

}
