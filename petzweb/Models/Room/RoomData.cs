namespace petzweb.Models;

public class RoomData
{
    /// STATISTICS ///
    int AmbientRoomTemperature { get; set; } = 20;

    protected RoomData(RoomData roomData)
    {
        AmbientRoomTemperature = roomData.AmbientRoomTemperature;
    }
    
    public RoomData()
    {
    }
}