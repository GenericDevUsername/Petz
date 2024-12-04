using petz.Models.Game;

namespace petz.Models.Room;

public class GameSaveRoom
{
  public GameSaveRoom(GameRoom room)
  {
    CurrentTemperature = room.CurrentTemperature;
    RegisteredRoomId = room.RegisteredId;
  }

  public GameSaveRoom()
  {
  }

  public float CurrentTemperature { get; private set; }
  public string RegisteredRoomId { get; set; }

  public GameRoom? ToGameRoom()
  {
    RegisteredRoom? roomData = GameManager.Rooms.GetRoom(RegisteredRoomId);
    return roomData == null ? null : new GameRoom(roomData, CurrentTemperature);
  }
}