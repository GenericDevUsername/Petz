namespace petzweb.Models.Room;

public class GameRoom(RegisteredRoom room) : RegisteredRoom(room)
{
    public int CurrentTemperature { get; private set; } = room.AmbientRoomTemperature;
    
    public GameSaveRoom ToGameSaveRoom()
    {
        return new GameSaveRoom(this);
    }
}