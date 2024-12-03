using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;




public class OpenAiManager : MonoBehaviour
{

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
        public string content;
        public string created_at;
        
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

    private string open_ai_key = "";
    private string assistant_id = "";
    private string activeThread = "";
    private List<Message> messageList = new List<Message>();

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
        string url = $"https://api.openai.com/v1/threads/{activeThread}/messages?limit=100";

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

                SetMessageList(messages);
            }
        }
        else
        {
            Debug.LogError("Error fetching messages: " + request.error);
        }
    }

    private void SetMessageList(List<Message> messages)
    {
        messageList = messages;
        Debug.Log("Messages set: " + messages.Count);
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

    

}

