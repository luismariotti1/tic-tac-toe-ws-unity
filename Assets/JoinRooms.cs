using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinRooms : MonoBehaviour
{
    [SerializeField] private GameObject findGameButton;
    private GameConnection _gameConnection;
    private string player = "Player";
    private int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        _gameConnection = GameConnection.Instance;

        findGameButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom());

        // _gameConnection.Socket.OnUnityThread("joinedRoom", (response) =>
        // {
        //     var data = response.GetValue().ToString();
        //     var json = JsonUtility.FromJson<JoinedRoom>(data);
        //
        //     if (!json.success) return;
        //     Debug.Log("Joined room: " + json.room);
        //     player.GetComponent<Player>().Marker = json.marker;
        //     
        //     SceneManager.LoadScene("Game");
        //     DontDestroyOnLoad(_gameConnection);
        //     DontDestroyOnLoad(player);
        // });
    }

    // join room
    private void JoinRoom()
    {
        _gameConnection.Socket.Emit("joinRoom", player + id);
        id++;
    }
}

internal class JoinedRoom
{
    public string room;
    public bool success;
    public string marker;
}