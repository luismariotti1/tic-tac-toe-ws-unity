using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public int Row;
    public int Column;
    public string Mark { get; set; }
    private GameConnection _gameConnection;
    private GameObject _player;

    void Start()
    {
        _gameConnection = GameConnection.Instance;
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
        _player = GameObject.FindWithTag("Player");
        var playerScript = _player.GetComponent<Player>();
        _gameConnection.Socket.Emit("mark", JsonUtility.ToJson(
            new MoveMessage(Row, Column, playerScript.Marker)));
    }
}

public class MoveMessage
{
    public int Row;
    public int Column;
    public string Marker;

    public MoveMessage(int row, int column, string marker)
    {
        Row = row;
        Column = column;
        Marker = marker;
    }
}