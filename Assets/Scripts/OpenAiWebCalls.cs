using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.IO;



public class OpenAiWebCalls : MonoBehaviour
{

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

    [System.Serializable]
    private class SpeechPayload
    {
        public string model;
        public string input;
        public string voice;
    }

    [System.Serializable]
    private class MessageResponse
    {
        public Message[] data;
    }

    [System.Serializable]
    public class Message
    {
        public string id;
        public string role;
        public content[] content;
        public string created_at;
        
    }

    [System.Serializable]
    public class content
    {
        public text text;
    }

    [System.Serializable]
    public class text
    {
        public String value;
        public object[] annotations;
    }

    [Serializable]
    public class chatData
    {
        public string text;
        public string facialExpression;
        public string animation;
    }

    [System.Serializable]
    public class value
    {
        public string messageValue;
    }

    [System.Serializable]
    private class RunStatusResponse
    {
        public string status;
    }

[System.Serializable]
    private class MessagePayload
    {
        public string role;
        public string content;
    }

[System.Serializable]
    private class RunPayload
    {
        public string assistant_id;
        public string model;
    }

[System.Serializable]
    private class RunResponse
    {
        public string id;
    }

     [SerializeField] private ChatBoxLogic chat;

    private string open_ai_key = "";
    private string assistant_id = "asst_f1JMDYqGUigh02vCutxfkRue";
    public AudioSource audioSource;
    public List<chatData> _test;
    public string activeThread = "";
    public SelectAnimationScript selectAnimationScript;
    public List<Message> messageList = new List<Message>();

    private IEnumerator CreateNewThread()
    {
        string url = "https://api.openai.com/v1/threads";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v2");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            RunResponse response = JsonUtility.FromJson<RunResponse>(responseText);
            if (response != null && !string.IsNullOrEmpty(response.id))
            {
                activeThread = response.id;
            }
            Debug.Log("Response: " + responseText);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator SendMessageToThread(string newMessage)
    {
        if(activeThread == ""){
            yield return StartCoroutine(CreateNewThread());
        }
        string url = $"https://api.openai.com/v1/threads/{activeThread}/messages";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v2");

        string jsonBody = JsonUtility.ToJson(new MessagePayload { role = "user", content = newMessage });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            StartCoroutine(SendRunRequest());
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator SendRunRequest()
    {
        string url = $"https://api.openai.com/v1/threads/{activeThread}/runs";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v2");

        string jsonBody = JsonUtility.ToJson(new RunPayload
        {
            assistant_id = assistant_id,
            model = "gpt-4o-mini"
        });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            RunResponse response = JsonUtility.FromJson<RunResponse>(responseText);
            if (response != null && !string.IsNullOrEmpty(response.id))
            {
                StartCoroutine(CheckRunStatusCoroutine(response.id));
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator CheckRunStatusCoroutine(string runId)
    {
        string url = $"https://api.openai.com/v1/threads/{activeThread}/runs/{runId}";
        bool isRunning = true;

        while (isRunning)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("OpenAI-Beta", "assistants=v2");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                RunStatusResponse response = JsonUtility.FromJson<RunStatusResponse>(responseText);

                if (response != null)
                {
                    if (string.IsNullOrEmpty(response.status))
                    {
                        isRunning = false; 
                    }
                    else if (response.status == "completed")
                    {
                        isRunning = false; 
                        StartCoroutine(FetchMessages()); 
                    }
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                isRunning = false; 
            }

            yield return new WaitForSeconds(3f); 
        }
    }

    private IEnumerator FetchMessages()
    {
        string url = $"https://api.openai.com/v1/threads/{activeThread}/messages?limit=20";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v2");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Messages Response: " + responseText);

            MessageResponse response = JsonUtility.FromJson<MessageResponse>(responseText);

            if (response != null && response.data != null && response.data.Length > 0)
            {
                List<Message> messages = new List<Message>(response.data);
                messages.Reverse(); 

                StartCoroutine(ProcessMessages(messages));
            }
        }
        else
        {
            Debug.LogError("Error fetching messages: " + request.error);
        }
    }

    private IEnumerator TextToSpeech(string speechText)
    {
        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/audio/speech", "POST");

        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");

        string jsonBody = JsonUtility.ToJson(new SpeechPayload
        {
            model = "tts-1",
            input = speechText,
            voice = "alloy"
        });

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        request.downloadHandler = new DownloadHandlerFile(Path.Combine(Application.persistentDataPath, "speech.mp3"));

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Audio generado con éxito. Archivo guardado en: " + Path.Combine(Application.persistentDataPath, "speech.mp3"));
            SelectAnimation();
            StartCoroutine(LoadAudio(Path.Combine(Application.persistentDataPath, "speech.mp3")));
            
        }
        else
        {
            Debug.LogError("Error al generar el audio: " + request.error);
        }
    }

    IEnumerator ProcessMessages(List<Message> messages){
        messageList = messages;

        Dialogue textProcessed = ProcessJson(messageList[messageList.Count-1].content[0].text.value);
        yield return StartCoroutine(TextToSpeech(textProcessed.text));
        chat.ChatEntryPointMessages(textProcessed.text);

    }

    public void CreateNewThreadInterface(){
        StartCoroutine(CreateNewThread());
    }

    public void SendMessageToThreadInterface(string _newMessage){
        StartCoroutine(SendMessageToThread(_newMessage));
    }

    public List<Message> FetchMessagesInterface(){
        if(activeThread != ""){
            StartCoroutine(FetchMessages());
        }
        return messageList;
    }

    private Dialogue ProcessJson(string jsonToProcess){
        jsonToProcess = jsonToProcess.Trim();
        string[] Process1= jsonToProcess.Split("```");
        string[] Process2= Process1[1].Split("json");

        Dialogue[] dialogues = JsonUtility.FromJson<DialogueList>("{\"dialogues\":" + Process2[1] + "}").dialogues;
        return dialogues[0];
    }

    public void SelectAnimation()
    {
        Dialogue textProcessed = ProcessJson(messageList[messageList.Count - 1].content[0].text.value);
        selectAnimationScript.OnRetrieveAnimation(textProcessed.animation);
        selectAnimationScript.ReatrieveEmote(textProcessed.facialExpression);
    }

    private IEnumerator LoadAudio(string audiopath)
    {
        string pathWithPrefix = "file://" + audiopath;
        print("Audio path: " + pathWithPrefix);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(pathWithPrefix, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError($"Error al cargar el audio desde {audiopath}: {www.error}");
            }
        }
    }
}


