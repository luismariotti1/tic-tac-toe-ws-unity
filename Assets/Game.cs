using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject gridSpacePrefab;
    [SerializeField] private GameObject gridSpacesHolder;
    [SerializeField] private GameObject winnerPopup;
    private List<List<GridSpace>> _gridSpaces = new List<List<GridSpace>>();
    private SocketIOConnection _connection;
    private string _room;
    private bool _playerReady;

    void Start()
    {
        _connection = SocketIOConnection.Instance;
        _connection.Socket.Emit("getPlayerData", PlayerPrefs.GetString("user_id"));
        _connection.Socket.OnUnityThread("playerData", (response) =>
        {
            var data = response.GetValue().ToString();
            var json = JsonUtility.FromJson<PlayerData>(data);

            GameManager.Instance.player = new Player(json.marker, json.room);
            CreateGridSpaces();
        });

        // _connection.Socket.Emit("getBoard", this.player.GetComponent<Player>().room);
        _connection.Socket.OnUnityThread("updateBoard", (response) =>
        {
            var data = response.GetValue();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var state = JsonUtility.FromJson<GridSpaceState>(data[i][j].ToString());
                    _gridSpaces[i][j].SetMark(state.Mark);
                }
            }
        });

        _connection.Socket.OnUnityThread("winner", (response) =>
        {
            var data = response.GetValue();
            winnerPopup.SetActive(true);
            var popUpText = winnerPopup.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            var text = data + "\n" + "won the game";
            popUpText.text = text;
        });

        // _connection.Socket.OnUnityThread("restarted", (response) => { winnerPopup.SetActive(false); });
    }

    private void CreateGridSpaces()
    {
        for (int i = 0; i < 3; i++)
        {
            _gridSpaces.Add(new List<GridSpace>());
            for (int j = 0; j < 3; j++)
            {
                var gridSpace = Instantiate(gridSpacePrefab, gridSpacesHolder.transform);
                gridSpace.transform.localPosition = new Vector3(j * 170, i * -170, 0);
                _gridSpaces[i].Add(gridSpace.GetComponent<GridSpace>());
                _gridSpaces[i][j].SetGridPosition(i, j);
            }
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string id;
    public string room;
    public string marker;
    public string turn;
}

internal class GridSpaceState
{
    public int Column;
    public int Row;
    public string Mark;
}