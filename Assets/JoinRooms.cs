using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinRooms : MonoBehaviour
{
    [SerializeField] private GameObject Room1;
    [SerializeField] private GameObject Room2;
    [SerializeField] private Player player;
    private GameConnection _gameConnection;

    // Start is called before the first frame update
    void Start()
    {
        _gameConnection = GameConnection.Instance;

        Room1.GetComponent<Button>().onClick.AddListener(() => JoinRoom(Room1));
        Room2.GetComponent<Button>().onClick.AddListener(() => JoinRoom(Room2));

        _gameConnection.Socket.OnUnityThread("joinedRoom", (response) =>
        {
            var data = response.GetValue().ToString();
            var json = JsonUtility.FromJson<JoinedRoom>(data);

            if (!json.success) return;
            Debug.Log("Joined room: " + json.room);
            player.GetComponent<Player>().Marker = json.marker;
            
            SceneManager.LoadScene("Game");
            DontDestroyOnLoad(_gameConnection);
            DontDestroyOnLoad(player);
        });
    }

    // join room
    private void JoinRoom(GameObject room)
    {
        _gameConnection.Socket.Emit("joinRoom", room.name);
    }
}

internal class JoinedRoom
{
    public string room;
    public bool success;
    public string marker;
}