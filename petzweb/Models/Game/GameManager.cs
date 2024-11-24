using petzweb.Models.Item;
using petzweb.Models.Pet;
using petzweb.Models.Room;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models.Game;

public class GameManager
{
    private static readonly string GameDataPath =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) +
        "/.petzgame";
    private static string GameSavesPath => GameDataPath + "/saves";

    public static readonly ItemManager ItemManager = new();
    public static readonly RoomManager RoomManager = new();
    public static readonly PetManager PetManager = new();
    public static bool Initialised { get; private set; } = false;
    
    public required GameData Data { get; init; }
    public bool IsValid { get; private set; } = true;
    
    public void Save()
    {
        Data.LastSaved = DateTime.Now;
        // Save the game to Program.GameSavesPath
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        string yaml = serializer.Serialize(Data.ToGameSave());

        File.WriteAllText(Program.GameSavesPath + $"/{Data.GameId}.yml", yaml);
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
                //Console.WriteLine(data);
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
    ///     Create required program files and load required game objects.
    ///     Run on startup.
    /// </summary>
    public static void Initialize()
    {
        List<string> requiredDirectories =
        [
            Program.GameDataPath,
            Program.GameSavesPath
        ];

        // Iterate through required directories and create them if they don't exist
        foreach (string directory in requiredDirectories.Where(directory => !Directory.Exists(directory)))
            Directory.CreateDirectory(directory);

        // Load required game objects
        ItemManager.LoadItems();
        RoomManager.LoadRooms();
        PetManager.LoadPets();
    }
}