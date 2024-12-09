using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

[System.Serializable]
public class Dialogue
{
    public string text;
    public string facialExpression;
    public string animation;
}

[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogues;
}
public class Jsoner : MonoBehaviour
{
    public string jsonInput;
    
        void Start (){

            jsonInput = jsonInput.Trim();
            string[] test= jsonInput.Split("```");
            print(test[1]);
            string[] test2= test[1].Split("json");

            Dialogue[] dialogues = JsonUtility.FromJson<DialogueList>("{\"dialogues\":" + test2[1] + "}").dialogues;

        
            foreach (Dialogue dialogue in dialogues)
            {
                Debug.Log($"Text: {dialogue.text}, Facial Expression: {dialogue.facialExpression}, Animation: {dialogue.animation}");
            }

        }


    private Dialogue ProcessJson(string jsonToProcess){
        jsonToProcess = jsonToProcess.Trim();
        string[] Process1= jsonToProcess.Split("```");
        string[] Process2= Process1[1].Split("json");

        Dialogue[] dialogues = JsonUtility.FromJson<DialogueList>("{\"dialogues\":" + Process2[1] + "}").dialogues;
        return dialogues[0];
    }
}
