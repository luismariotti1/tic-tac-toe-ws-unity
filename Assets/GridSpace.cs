using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public int Row;
    public int Column;
    public string Mark { get; set; }
    private SocketIOConnection _connection;
    private GameObject _player;

    void Start()
    {
        _connection = SocketIOConnection.Instance;
        GetComponentInParent<Button>().onClick.AddListener(OnClick);
    }

    public void SetGridPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public void SetMark(string mark)
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = mark;
        GetComponent<Button>().interactable = mark == "";
    }

    private void OnClick()
    {
        var gameManager = GameManager.Instance;
        var player = gameManager.player;
        if (!player.isTurn) return;
        _connection.Socket.Emit("mark", JsonUtility.ToJson(
            new MoveMessage(Row, Column, player.marker, player.room)));
    }
}

[System.Serializable]
public class MoveMessage
{
    public int Row;
    public int Column;
    public string Marker;
    public string Room;

    public MoveMessage(int row, int column, string marker, string room)
    {
        Row = row;
        Column = column;
        Marker = marker;
        Room = room;
    }
}