namespace petz.Models.Room;

public class GameRoom(RegisteredRoom room, float? currentTemp) : RegisteredRoom(room)
{
  public float CurrentTemperature { get; set; } = currentTemp ?? room.AmbientRoomTemperature;

  /// <summary>
  ///  Increment the room temperature towards the ambient room temperature.
  /// </summary>
  /// <param name="amount"> The amount to increment the temperature by </param>
  public void IncrementTowardsAmbientTemperature(float amount)
  {
    if (CurrentTemperature < AmbientRoomTemperature)
      CurrentTemperature = Math.Min(CurrentTemperature + amount, AmbientRoomTemperature);
    else if (CurrentTemperature > AmbientRoomTemperature)
      CurrentTemperature = Math.Max(CurrentTemperature - amount, AmbientRoomTemperature);
  }
  

  public GameSaveRoom ToGameSaveRoom()
  {
    return new GameSaveRoom(this);
  }
}