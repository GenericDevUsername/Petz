namespace petzweb.Models.Room;

public class GameRoom(RegisteredRoom room, float? currentTemp) : RegisteredRoom(room)
{
  public float CurrentTemperature { get; set; } = currentTemp ?? room.AmbientRoomTemperature;
  
  public void SetTemperature(float temp)
  {
    CurrentTemperature = temp;
  }
  
  public void IncrementTowardsAmbientTemperature(int amount)
  {
    if (CurrentTemperature < AmbientRoomTemperature)
      CurrentTemperature = Math.Min(CurrentTemperature + amount, AmbientRoomTemperature);
    else if (CurrentTemperature > AmbientRoomTemperature)
      CurrentTemperature = Math.Max(CurrentTemperature - amount, AmbientRoomTemperature);
  }
  // override for decimals
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