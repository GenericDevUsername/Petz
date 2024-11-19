namespace petzweb.Models.Room
{
  public class PetRoom(RegisteredRoom room) : RegisteredRoom(room)
  {
    public int RoomTemperature { get; private set; } = room.AmbientRoomTemperature;
  }
}
