using UnityEngine;
using UnityEngine.UIElements;

public class PlayAgain : MonoBehaviour
{
    public void RestartGame()
    {
        var connection = SocketIOConnection.Instance;
        connection.Socket.Emit("restart", GameManager.Instance.player.room);
    }
}
