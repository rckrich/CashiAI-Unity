using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class DialogueList
{
    public DialogueEntry[] dialogues;
}

[System.Serializable]
public class DialogueEntry
{
    public string text;
    public string facialExpression;
    public string animation;
}
public class Jsoner : MonoBehaviour
{
    public string jsonInput;
        void Start()
    {
        

        // Deserializar el JSON
        DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(jsonInput);

        // Acceder a los datos
        foreach (var dialogue in dialogueList.dialogues)
        {
            Debug.Log($"Text: {dialogue.text}");
            Debug.Log($"Facial Expression: {dialogue.facialExpression}");
            Debug.Log($"Animation: {dialogue.animation}");
        }
    }

}
