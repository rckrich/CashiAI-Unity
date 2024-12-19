using UnityEngine;
using System;
using System.Text;
using Newtonsoft.Json; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;


public class Realtime2 : MonoBehaviour
{

    private const string url = "wss://api.openai.com/v1/realtime?model=gpt-4o-realtime-preview-2024-10-01";
    public string open_ai_key = "";
    [SerializeField] public RootEphimeralToken test;
    private string AccessToken = "";
    private ClientWebSocket webSocket;
    private IEnumerator GetEpihemeralToken()
    {
        string url = "https://api.openai.com/v1/realtime/sessions";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", $"Bearer {open_ai_key}");
        request.SetRequestHeader("Content-Type", "application/json");
        string jsonBody = JsonUtility.ToJson(new TokenPayload { model = "gpt-4o-realtime-preview-2024-12-17", modalities = new[] { "audio", "text" }, instructions= "You are a friendly assistant." });
        Debug.Log(jsonBody); 
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            RootEphimeralToken responseJson = JsonUtility.FromJson<RootEphimeralToken>(responseText);
            AccessToken = responseJson.client_secret.value;
            Debug.Log(AccessToken);
            Initialize();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private async void Initialize(){
        webSocket = new ClientWebSocket();

        webSocket.Options.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        webSocket.Options.SetRequestHeader("OpenAI-Beta", "realtime=v1");

        try
        {

            Debug.Log("Connecting to WebSocket server...");
            
            await webSocket.ConnectAsync(new Uri(url), System.Threading.CancellationToken.None);
            Debug.Log("Connected to WebSocket server.");
            ResponseData temp = new ResponseData();
            string[] modes = {"audio", "text"};
            temp.modalities = modes;
            temp.instructions = "Give me a haiku about code.";

            string jsonBody = JsonUtility.ToJson(new MessagePayload { type = "response.create", response = temp });   
            Debug.Log(jsonBody);
            await SendMessageAsync(jsonBody);
            _ = ReceiveMessagesAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"WebSocket connection failed: {ex.Message}");
        }
    }

    private async Task SendMessageAsync(string message)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            var encodedMessage = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(encodedMessage);

            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Debug.Log($"Message sent: {message}");
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected. Unable to send message.");
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    Debug.Log("WebSocket connection closed by server.");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log($"Message received: {message}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while receiving WebSocket message: {ex.Message}");
                break;
            }
        }
    }


    void Start()
    {
        StartCoroutine(GetEpihemeralToken());
        
    }

    async void OnDestroy()
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            Debug.Log("WebSocket closed.");
        }
    }
}

[System.Serializable]
public class MessagePayload
{
    public string type;
    public ResponseData response;
}


[System.Serializable]
public class ClientEvent
{
    public string type;
    public ResponseData response;
}

[System.Serializable]
public class ResponseData
{
    public string[] modalities;
    public string instructions;
}

[System.Serializable]
public class ServerEvent
{
    public string type;
    public dynamic data; 
}

[Serializable]
public class TokenPayload
{
    public string model;
    public string[] modalities;
    public string instructions; 
}

[Serializable] 
public class ClientSecret
    {
        public string value;
        public int expires_at;
    }
[Serializable] 
    public class InputAudioTranscription
    {
        public string model;
    }
[Serializable] 
    public class RootEphimeralToken
    {
        public string id;
        public string @object;
        public string model;
        public List<string> modalities;
        public string instructions;
        public string voice;
        public string input_audio_format;
        public string output_audio_format;        
        public InputAudioTranscription input_audio_transcription;
        public object turn_detection;
        public List<object> tools;
        public string tool_choice;
        public double temperature;
        public int max_response_output_tokens;
        public ClientSecret client_secret;
    }
