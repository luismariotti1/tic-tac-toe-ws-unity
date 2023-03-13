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
    private SocketIOConnection _connection;
  
    // Start is called before the first frame update
    void Start()
    {
        _connection = SocketIOConnection.Instance;

        findGameButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom());
        
        // show user name is text tmp
        showUserName.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetString("username");

        _connection.Socket.OnUnityThread("joinedRoom", (response) =>
        {
            findGameButton.SetActive(false);
            loading.SetActive(true);
        });
    }

    // join room
    private void JoinRoom()
    {
        _connection.Socket.Emit("joinRoom", PlayerPrefs.GetString("user_id"));
    }
}

internal class JoinedRoom
{
    public string room;
    public bool success;
    public string marker;
}