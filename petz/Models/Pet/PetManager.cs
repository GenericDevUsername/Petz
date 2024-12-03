using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Pet;

public class PetManager
{
  private readonly Dictionary<string, RegisteredPet> _pets = new();

  internal void LoadPets()
  {
    string yamlFile = ReadItemsFile();

    // Load items from rooms.yml file
    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    Dictionary<string, PetData>? loadedItems =
      deserializer.Deserialize<Dictionary<string, PetData>?>(yamlFile) ?? [];

    // register pets
    int registeredRoomCount = 0;
    foreach (KeyValuePair<string, PetData> pet in loadedItems)
      try
      {
        _pets.Add(pet.Key, new RegisteredPet(pet.Key, pet.Value));
        registeredRoomCount++;
      }
      catch (Exception e)
      {
        Program.Log($"[ERROR] Failed to register pet {pet.Key}: {e.Message}");
      }

    // print room.RoomName for each room
    Program.Log(
      $"[DEBUG] Registered {registeredRoomCount} pets: {string.Join(" ", _pets.Values.Select(pet => pet.Icon))}");
  }

  public void SavePets(Dictionary<string, PetData> pets)
  {
    ISerializer serializer = new SerializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    string yaml = serializer.Serialize(pets);

    File.WriteAllText(Program.GameDataPath + "/pets.yml", yaml);
  }

  private static string ReadItemsFile()
  {
    if (!File.Exists(Program.GameDataPath + "/pets.yml"))
    {
      File.WriteAllText(Program.GameDataPath + "/pets.yml", DefaultFiles.Pets);
      return string.Empty;
    }

    return File.ReadAllText(Program.GameDataPath + "/pets.yml");
  }

  public RegisteredPet GetPet(string registeredId)
  {
    return _pets[registeredId];
  }

  public List<RegisteredPet> GetPets()
  {
    return _pets.Values.ToList();
  }
}