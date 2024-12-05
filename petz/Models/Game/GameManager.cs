using petz.Models.Inventory;
using petz.Models.Item;
using petz.Models.Pet;
using petz.Models.Room;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Game;

public class GameManager
{
  private static readonly string GameDataPath =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) +
    "/.petzgame";

  public static readonly ItemManager Items = new();
  public static readonly RoomManager Rooms = new();
  public static readonly PetManager Pets = new();

  public required GameData? Data { get; init; }
  public bool IsValid { get; private set; } = true;

  /// <summary>
  ///   Save the game to Program.GameSavesPath
  /// </summary>
  /// <param name="data"> The game data to save </param>
  public static void Save(GameData data)
  {
    data.LastSaved = DateTime.Now;
    // Save the game to Program.GameSavesPath
    ISerializer serializer = new SerializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    string yaml = serializer.Serialize(data.ToGameSave());

    File.WriteAllText(Program.GameSavesPath + $"/{data.GameId}.yml", yaml);
  }

  /// <summary>
  ///  Load a game from Program.GameSavesPath
  /// </summary>
  /// <param name="gameId"> The game id to load </param>
  /// <returns></returns>
  /// <exception cref="Exception"> If the game save is invalid </exception>
  public static GameManager? Load(string gameId)
  {
    string yaml = File.ReadAllText(Program.GameSavesPath + $"/{gameId}.yml");

    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    GameData gameData;
    try
    {
      GameSave data = deserializer.Deserialize<GameSave>(yaml);
      gameData = data.ToGameData() ?? throw new Exception();
    }
    catch (Exception e)
    {
      return null;
    }

    return new GameManager { Data = gameData };
  }

  /// <summary>
  ///   Get all game saves from Program.GameSavesPath
  /// </summary>
  /// <returns> A list of GameManager objects </returns>
  /// <exception cref="Exception"> If a game save is invalid </exception>
  public static List<GameManager> GetSaves()
  {
    List<GameManager> saves = [];

    foreach (string file in Directory.GetFiles(Program.GameSavesPath))
    {
      string yaml = File.ReadAllText(file);

      IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

      GameData gameData;
      bool isValid = true;
      try
      {
        GameSave data = deserializer.Deserialize<GameSave>(yaml);
        //Program.Log(data);
        gameData = data.ToGameData() ?? throw new Exception();
      }
      catch (Exception e)
      {
        isValid = false;
        gameData = new GameData();
      }

      saves.Add(new GameManager { Data = gameData, IsValid = isValid });
    }

    return saves;
  }

  /// <summary>
  ///   Create required program files and load required game objects.
  ///   Run on startup.
  /// </summary>
  public static void Initialize()
  {
    List<string> requiredDirectories =
    [
      Program.GameDataPath,
      Program.GameSavesPath, 
      Program.LogPath
    ];

    // Iterate through required directories and create them if they don't exist
    foreach (string directory in requiredDirectories.Where(directory => !Directory.Exists(directory)))
      Directory.CreateDirectory(directory);
    
    Program.Log("");
    Program.Log("");
    Program.Log("-------- Initializing Game --------");
    Program.Log($"[DEBUG] Game data path: {Program.GameDataPath}");
    Program.Log($"[DEBUG] Game saves path: {Program.GameSavesPath}");
    Program.Log($"[DEBUG] Log path: {Program.LogPath}");
    Program.Log("-----------------------------------");
    Program.Log("");
    Program.Log("");
    // Load required game objects
    Items.LoadItems();
    Rooms.LoadRooms();
    Pets.LoadPets();
  }
  
  public static GameManager CreateNewGame(string petId, string name)
  {
    RegisteredRoom room = Rooms.GetRooms()[0];
    RegisteredPet pet = Pets.GetPet(petId);
    
    GameData data = new GameData()
    {
      LastSaved = DateTime.Now - TimeSpan.FromMinutes(1),
      Pet = new GamePet(pet)
      {
        Name = name,
        Love = 0,
        Happiness = pet.MaxHappiness,
        Hunger = pet.MaxHunger,
        Health = pet.MaxHealth,
        Energy = pet.MaxEnergy,
        BodyTemperature = pet.PreferredTemperature,
        IsSick = false,
      },
      Room = new GameRoom(room, pet.PreferredTemperature)
    };
    data.Inventory = new GameInventory(data);
    
    return new GameManager { Data = data };
  }
}