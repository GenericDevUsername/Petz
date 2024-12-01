namespace petzweb.Models.Room;

public class GameRoom(RegisteredRoom room, int? currentTemp) : RegisteredRoom(room)
{
  public int CurrentTemperature { get; private set; } = currentTemp ?? room.AmbientRoomTemperature;

  public GameSaveRoom ToGameSaveRoom()
  {
    return new GameSaveRoom(this);
  }
}