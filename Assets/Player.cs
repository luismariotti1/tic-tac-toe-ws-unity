[System.Serializable]
public class Player
{
    public string marker;
    public string room;

    public Player(string marker, string room)
    {
        this.marker = marker;
        this.room = room;
    }
}