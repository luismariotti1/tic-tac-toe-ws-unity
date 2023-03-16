[System.Serializable]
public class Player
{
    public string marker;
    public string room;
    public bool isTurn;

    public Player(string marker, string room, bool isTurn = false)
    {
        this.marker = marker;
        this.room = room;
        this.isTurn = isTurn;
    }
}