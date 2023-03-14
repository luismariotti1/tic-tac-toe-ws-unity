using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject gridSpacePrefab;
    [SerializeField] private GameObject gridSpacesHolder;
    [SerializeField] private GameObject winnerPopup;
    private List<List<GridSpace>> _gridSpaces = new List<List<GridSpace>>();
    private SocketIOConnection _connection;

    void Start()
    {
        _connection = SocketIOConnection.Instance;
        CreateGridSpaces();
        _connection.Socket.Emit("getBoard", "Room1");
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
        
        _connection.Socket.OnUnityThread("restarted", (response) =>
        {
            winnerPopup.SetActive(false);
        });
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

internal class GridSpaceState
{
    public int Column;
    public int Row;
    public string Mark;
}