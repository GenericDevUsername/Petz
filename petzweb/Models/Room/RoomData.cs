namespace petzweb.Models;

public class RoomData
{
    public RoomData(RoomData roomData)
    {
        AmbientRoomTemperature = roomData.AmbientRoomTemperature;
        RoomName = roomData.RoomName;
        RoomDescription = roomData.RoomDescription;
    }

    public RoomData()
    {
    }

    /// STATISTICS ///
    public int AmbientRoomTemperature { get; set; }

    public string RoomName { get; set; }
    public string RoomDescription { get; set; }
}