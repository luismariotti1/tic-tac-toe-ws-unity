using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

public class SocketIOConnection : MonoBehaviour
{
    public SocketIOUnity Socket { get; private set; }
    public static SocketIOConnection Instance { get; private set; }
    private const string URL = "http://localhost/game";

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

        DontDestroyOnLoad(gameObject);

        var uri = new Uri(URL);
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