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

  /// <summary>
  ///  Convert the game save room to a game room
  /// </summary>
  /// <returns> The game room if the save was converted successfully </returns>
  public GameRoom? ToGameRoom()
  {
    RegisteredRoom? roomData = GameManager.Rooms.GetRoom(RegisteredRoomId);
    return roomData == null ? null : new GameRoom(roomData, CurrentTemperature);
  }
}