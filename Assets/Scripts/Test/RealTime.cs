 using Unity.WebRTC;
using UnityEngine;

public class RealTime : MonoBehaviour
{
    /*
    private RTCPeerConnection peerConnection;
    private MediaStream localStream;
    public VideoStreamTrack videoTrack;
    public AudioStreamTrack audioTrack;

    // Referencia para mostrar video
    public Renderer remoteVideoRenderer;

    void Start()
    {
        WebRTC.Initialize();
        SetupPeerConnection();
        StartLocalStream();
    }

    void OnDestroy()
    {
        localStream?.Dispose();
        videoTrack?.Dispose();
        audioTrack?.Dispose();
        peerConnection?.Close();
        peerConnection?.Dispose();
        WebRTC.Dispose();
    }

    private void SetupPeerConnection()
    {
        var config = new RTCConfiguration
        {
            iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
        };

        peerConnection = new RTCPeerConnection(ref config);

        peerConnection.OnIceCandidate = candidate =>
        {
            Debug.Log($"New ICE Candidate: {candidate.Candidate}");
            // Envía el candidato ICE al servidor de señalización
        };

        peerConnection.OnTrack = e =>
        {
            if (e.Track is VideoStreamTrack videoTrack)
            {
                var texture = videoTrack.InitializeReceiver(1280, 720);
                remoteVideoRenderer.material.mainTexture = texture;
            }
        };

        peerConnection.OnNegotiationNeeded = async () =>
        {
            var offer = await peerConnection.CreateOffer();
            await peerConnection.SetLocalDescription(ref offer);

            Debug.Log($"Created Offer: {offer.sdp}");
            // Envía la oferta SDP al servidor de señalización
        };
    }

    private void StartLocalStream()
    {
        localStream = new MediaStream();

        // Captura el video de la cámara
        videoTrack = new VideoStreamTrack("localVideo");
        localStream.AddTrack(videoTrack);

        // Captura el audio del micrófono
        audioTrack = new AudioStreamTrack("localAudio");
        localStream.AddTrack(audioTrack);

        foreach (var track in localStream.GetTracks())
        {
            peerConnection.AddTrack(track, localStream);
        }
    }

    public async void OnRemoteAnswer(string sdp)
    {
        var answer = new RTCSessionDescription
        {
            type = RTCSdpType.Answer,
            sdp = sdp
        };
        await peerConnection.SetRemoteDescription(ref answer);
    }

    public async void OnRemoteIceCandidate(string candidate, string sdpMid, int sdpMLineIndex)
    {
        var iceCandidate = new RTCIceCandidate(new RTCIceCandidateInit
        {
            candidate = candidate,
            sdpMid = sdpMid,
            sdpMLineIndex = sdpMLineIndex
        });

        peerConnection.AddIceCandidate(iceCandidate);
    }
    */
}
