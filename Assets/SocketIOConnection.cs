using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

public class SocketIOConnection : MonoBehaviour
{
    // create SocketIO client
    public SocketIOUnity Socket { get; private set; }
    private string url = "http://localhost/game";
    public static SocketIOConnection Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // DontDestroyOnLoad(gameObject);

        var uri = new Uri(url);
        Socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                { "token", "UNITY" }
            },
            EIO = 4,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        Socket.JsonSerializer = new NewtonsoftJsonSerializer();

        Socket.unityThreadScope = SocketIOUnity.UnityThreadScope.Update;

        Socket.OnConnected += (sender, e) => { Debug.Log("Connected"); };

        Debug.Log("Connecting...");
        Socket.Connect();
    }
}