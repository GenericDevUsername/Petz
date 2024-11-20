using petzweb.Models.Game;

namespace petzweb.Models.Room;

public class GameSaveRoom
{
    public int CurrentTemperature { get; private set; }
    public string RegisteredRoomId { get; set; }
    
    public GameSaveRoom(GameRoom room)
    {
        CurrentTemperature = room.CurrentTemperature;
        RegisteredRoomId = room.RegisteredId;
    }
    public GameSaveRoom()
    {
    }
    
    public GameRoom? ToGameRoom()
    {
        RegisteredRoom? roomData = GameManager.RoomManager.GetRoom(RegisteredRoomId);
        return roomData == null ? null : new GameRoom(roomData);
    }
}