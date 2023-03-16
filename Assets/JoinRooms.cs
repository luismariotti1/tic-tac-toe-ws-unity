using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinRooms : MonoBehaviour
{
    [SerializeField] private GameObject findGameButton;
    [SerializeField] private GameObject showUserName;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject connection;
    private SocketIOConnection _connection;

    private void Awake()
    {
        Instantiate(connection);
    }

    // Start is called before the first frame update
    void Start()
    {
        _connection = SocketIOConnection.Instance;
        
        findGameButton.GetComponent<Button>().onClick.AddListener(JoinRoom);
        
        showUserName.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetString("username");
        
        _connection.Socket.On("joinedRoom", (response) =>
        {
            UnityThread.executeInUpdate(() => {
                Debug.Log("joined room");
                findGameButton.SetActive(false);
                loading.SetActive(true);
            });
        });
        
        _connection.Socket.On("startGame", (response) =>
        {
            UnityThread.executeInUpdate(() => {
                Debug.Log("start");
                DontDestroyOnLoad(_connection);
                SceneManager.LoadScene("Game");
            });
        });
    }

    // join room
    private void JoinRoom()
    {
        _connection.Socket.Emit("joinRoom", PlayerPrefs.GetString("user_id"));
    }
}

[System.Serializable]
public class JoinedRoom
{
    public string marker;
    public string room;
}