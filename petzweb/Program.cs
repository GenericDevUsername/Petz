using System.Diagnostics;
using System.Runtime.InteropServices;
using petzweb.Models;

namespace petzweb;

internal class Program
{
  public static readonly string GameDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) + "/.petzgame";
  public static string GameSavesPath => GameDataPath + "/saves";
  public bool Initialised { get; private set; } = false;
    
  public static readonly ItemManager ItemManager = new();
  public static readonly RoomManager RoomManager = new();
    
  private static void Main(string[] args)
  {
    Initialize();

    RegisteredItem paracetamol = ItemManager.GetItem("paracetamol");
    Pet pet = new()
    {
      Name = "Fido",
      Health = 50,
    };
    Console.WriteLine(paracetamol.ShopPrice);
    Console.WriteLine(pet.Health + "/" + pet.MaxHealth);
    Console.WriteLine(pet.IsSick);
    // count how long it takes to use the item
    paracetamol.UseItem(pet);
    Console.WriteLine(pet.Health + "/" + pet.MaxHealth);
    Console.WriteLine(pet.IsSick);
  }

  private static void Initialize()
  {
    List<string> requiredDirectories =
    [
      GameDataPath,
      GameSavesPath,
    ];
      
    // Create the directory if it doesn't exist
    Console.WriteLine($"[DEBUG] Creating Save Directory: {GameDataPath}");
    foreach (string directory in requiredDirectories.Where(directory => !Directory.Exists(directory)))
    {
      Directory.CreateDirectory(directory);
    }

    // Load required game objects
    Console.WriteLine("[DEBUG] Loading Game Objects");
    ItemManager.LoadItems();
    RoomManager.LoadRooms();
  }
}