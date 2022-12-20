using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

public class GameConnection : MonoBehaviour
{
    // create SocketIO client
    public SocketIOUnity Socket { get; private set; }
    private string url = "http://localhost/game";

    public static GameConnection Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
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

        Debug.Log("Connecting...");
        
        Socket.On("connected", (response) =>
        {
            Debug.Log(response.GetValue().ToString());
        });
        
        Socket.Connect();
    }
    
    private void Update()
    {
        
    }
}