using petz.Models.Item;
using petz.Models.Pet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Game;

public class GameManager
{
  private static readonly string GameDataPath =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) +
    "/.petzgame";

  public static readonly ItemManager ItemManager = new();
  public static readonly RoomManager RoomManager = new();
  public static readonly PetManager PetManager = new();
  private static string GameSavesPath => GameDataPath + "/saves";
  public static bool Initialised { get; private set; } = false;

  public required GameData? Data { get; set; }
  public bool IsValid { get; private set; } = true;

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
    ItemManager.LoadItems();
    RoomManager.LoadRooms();
    PetManager.LoadPets();
  }
}