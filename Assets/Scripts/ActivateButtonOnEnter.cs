using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButtonOnEnter : MonoBehaviour
{
    public Button sendButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            sendButton.onClick.Invoke();
            if (sendButton != null)
            {
                sendButton.onClick.Invoke();
            }
        }
    }
}