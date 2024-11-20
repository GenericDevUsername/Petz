namespace petzweb.Models;

public class RegisteredRoom : RoomData
{
    public RegisteredRoom(string registeredId, RoomData roomData) : base(roomData)
    {
        RegisteredId = registeredId;
    }

    public RegisteredRoom(RegisteredRoom room) : base(room)
    {
        RegisteredId = room.RegisteredId;
    }

    public RegisteredRoom()
    {
    }

    public string RegisteredId { get; }
}