namespace petzweb.Models;

public class RegisteredRoom : RoomData
{
    public string RegisteredId { get; private set; }
    
    public RegisteredRoom(string registeredId, RoomData roomData) : base(roomData)
    {
        RegisteredId = registeredId;
    }

}