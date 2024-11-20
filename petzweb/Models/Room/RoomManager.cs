using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models;

public class RoomManager
{
    private readonly Dictionary<string, RegisteredRoom> _rooms = new();

    internal void LoadRooms()
    {
        string yamlFile = ReadItemsFile();

        // Load items from rooms.yml file
        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        Dictionary<string, RoomData>? loadedItems =
            deserializer.Deserialize<Dictionary<string, RoomData>?>(yamlFile) ?? [];

        // register rooms
        Console.WriteLine(
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
                Console.WriteLine($"[ERROR] Failed to register room {room.Key}: {e.Message}");
            }

        // print room.RoomName for each room
        Console.WriteLine(
            $"[DEBUG] Registered {registeredRoomCount} rooms: {string.Join(", ", _rooms.Values.Select(room => room.RoomName))}");
    }

    public void SaveRooms(Dictionary<string, RoomData> rooms)
    {
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        string yaml = serializer.Serialize(rooms);

        File.WriteAllText(Program.GameDataPath + "/rooms.yml", yaml);
    }

    private static string ReadItemsFile()
    {
        if (!File.Exists(Program.GameDataPath + "/rooms.yml"))
        {
            File.WriteAllText(Program.GameDataPath + "/rooms.yml", "");
            return string.Empty;
        }

        return File.ReadAllText(Program.GameDataPath + "/rooms.yml");
    }

    public RegisteredRoom? GetRoom(string registeredId)
    {
        return _rooms[registeredId] ?? null;
    }

    public List<RegisteredRoom> GetRooms()
    {
        return _rooms.Values.ToList();
    }
}