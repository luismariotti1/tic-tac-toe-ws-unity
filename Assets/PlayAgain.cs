using UnityEngine;
using UnityEngine.UIElements;

public class PlayAgain : MonoBehaviour
{
    public void RestartGame()
    {
        var gameConnection = GameConnection.Instance;
        gameConnection.Socket.Emit("restart", "Room1");
    }
}
