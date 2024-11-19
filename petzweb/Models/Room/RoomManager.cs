using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models;

public class RoomManager
{
  private readonly Dictionary<string, RegisteredRoom> _rooms = new();

  public RoomManager()
  {
  }

  internal void LoadRooms()
  {
    string yamlFile = ReadItemsFile();

    // Load items from rooms.yml file
    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    Dictionary<string, RoomData>? loadedItems = deserializer.Deserialize<Dictionary<string, RoomData>?>(yamlFile) ??
                                                [];

    // register rooms
    int registeredRoomCount = 0;
    foreach (KeyValuePair<string, RoomData> room in loadedItems)
    {
      try
      {
        _rooms.Add(room.Key, new RegisteredRoom(room.Key, room.Value));
        registeredRoomCount++;
      }
      catch (Exception e)
      {
        Console.WriteLine($"[ERROR] Failed to register room {room.Key}: {e.Message}");
      }
    }
    Console.WriteLine($"[DEBUG] Registered {registeredRoomCount} rooms");
  }

  private static string ReadItemsFile()
  {
    if (!File.Exists(Program.GameDataPath + "/rooms.yml"))
    {
      File.WriteAllText(Program.GameDataPath + "/rooms.yml", "");
      return string.Empty;
    }
    else
    {
      return File.ReadAllText(Program.GameDataPath + "/rooms.yml");
    }
  }

  public RegisteredRoom GetRoom(string registeredId)
  {
    return _rooms[registeredId];
  }
  
  public List<RegisteredRoom> GetRooms()
  {
    return _rooms.Values.ToList();
  }
}