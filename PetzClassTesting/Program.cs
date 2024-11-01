using System.Diagnostics;
using System.Runtime.InteropServices;
using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    public static readonly string GameDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.petzgame";
    public static readonly string SavesPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.petzgame/Saves";
    
    public static readonly ItemManager ItemManager = new();
    
    private static void Main(string[] args)
    {
      Initialize();

      RegisteredItem paracetamol = ItemManager.GetItem("paracetamol");
      Pet pet = new Pet("Fido")
      {
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
        GameDataPath + "/saves"
      ];
      Console.WriteLine($"[DEBUG] Creating Save Directory: {GameDataPath}");
      
      // Create the directory if it doesn't exist
      foreach (var directory in requiredDirectories.Where(directory => !Directory.Exists(directory)))
      {
        Directory.CreateDirectory(directory);
      }
    }
  }
}
