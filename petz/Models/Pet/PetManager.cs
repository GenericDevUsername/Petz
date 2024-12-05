using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Pet;

public class PetManager
{
  private readonly Dictionary<string, RegisteredPet> _pets = new();

  /// <summary>
  ///  Load pets from the pets.yml file
  /// </summary>
  public void LoadPets(string? petsYaml = null)
  {
    string yamlFile = petsYaml ?? ReadFile();

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

  /// <summary>
  ///  Read the pets.yml file
  /// </summary>
  /// <returns> The contents of the pets.yml file </returns>
  private static string ReadFile()
  {
    if (!File.Exists(Program.GameDataPath + "/pets.yml"))
    {
      File.WriteAllText(Program.GameDataPath + "/pets.yml", DefaultFiles.Pets);
      return string.Empty;
    }

    return File.ReadAllText(Program.GameDataPath + "/pets.yml");
  }

  /// <summary>
  ///  Get a pet from the registered id
  /// </summary>
  /// <param name="registeredId"> The registered id of the pet </param>
  /// <returns> The pet </returns>
  public RegisteredPet GetPet(string registeredId)
  {
    return _pets[registeredId];
  }

  /// <summary>
  ///  Get all registered pets
  /// </summary>
  /// <returns> The pets </returns>
  public List<RegisteredPet> GetPets()
  {
    return _pets.Values.ToList();
  }
}