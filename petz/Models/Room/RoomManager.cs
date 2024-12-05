using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models;

public class RoomManager
{
  private readonly Dictionary<string, RegisteredRoom> _rooms = new();

  /// <summary>
  ///  Load rooms from the rooms.yml file
  /// </summary>
  internal void LoadRooms()
  {
    string yamlFile = ReadFile();

    // Load items from rooms.yml file
    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    Dictionary<string, RoomData>? loadedItems =
      deserializer.Deserialize<Dictionary<string, RoomData>?>(yamlFile) ?? [];

    // register rooms
    Program.Log(
      $"[DEBUG] Registering rooms: {string.Join(", ", loadedItems.Values.Select(room => room.RoomName))}");
    int registeredRoomCount = 0;
    foreach (KeyValuePair<string, RoomData> room in loadedItems)
      try
      {
        RoomData roomData = room.Value;
        _rooms.Add(room.Key, new RegisteredRoom(room.Key, room.Value));
        registeredRoomCount++;
      }
      catch (Exception e)
      {
        Program.Log($"[ERROR] Failed to register room {room.Key}: {e.Message}");
      }

    // print room.RoomName for each room
    Program.Log(
      $"[DEBUG] Registered {registeredRoomCount} rooms: {string.Join(", ", _rooms.Values.Select(room => room.RoomName))}");
  }

  /// <summary>
  ///  Read the rooms.yml file
  /// </summary>
  /// <returns> The contents of the rooms.yml file </returns>
  private static string ReadFile()
  {
    if (File.Exists(Program.GameDataPath + "/rooms.yml")) return File.ReadAllText(Program.GameDataPath + "/rooms.yml");
    File.WriteAllText(Program.GameDataPath + "/rooms.yml", DefaultFiles.Rooms);
    return string.Empty;
  }

  /// <summary>
  ///  Get a room from the registered id
  /// </summary>
  /// <param name="registeredId"> The registered id of the room </param>
  /// <returns> The room if it exists, otherwise null </returns>
  public RegisteredRoom? GetRoom(string registeredId)
  {
    return _rooms[registeredId] ?? null;
  }

  /// <summary>
  ///  Get all rooms
  /// </summary>
  /// <returns> The list of rooms </returns>
  public List<RegisteredRoom> GetRooms()
  {
    return _rooms.Values.ToList();
  }
}