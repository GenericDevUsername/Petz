using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Item;

public class ItemManager
{
  private readonly Dictionary<string, RegisteredItem> _items = new();

  /// <summary>
  ///  Load items from the items.yml file
  /// </summary>
  public void LoadItems(string? itemsYaml = null)
  {
    string yamlFile = itemsYaml ?? ReadFile();

    // Load items from data/items.yml file
    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    Dictionary<string, ItemData>? loadedItems;
    try
    {
      loadedItems = deserializer.Deserialize<Dictionary<string, ItemData>?>(yamlFile) ?? [];
    }
    catch (Exception e)
    {
      loadedItems = [];
      // regenerating the file if it fails to load
      Program.Log($"[ERROR] Failed to load rooms.yml: {e.Message}");
      Program.Log("[DEBUG] Regenerating rooms.yml");
      RegenerateFile();
      LoadItems();
    }

    // register items
    int registeredItemCount = 0;
    foreach (KeyValuePair<string, ItemData> item in loadedItems)
      try
      {
        _items.Add(item.Key, new RegisteredItem(item.Key, item.Value));
        registeredItemCount++;
      }
      catch (Exception e)
      {
        Program.Log($"[ERROR] Failed to register item {item.Key}: {e.Message}");
      }

    Program.Log($"[DEBUG] Registered {registeredItemCount} items");
  }

  /// <summary>
  ///  Read the items.yml file
  /// </summary>
  /// <returns> The contents of the items.yml file </returns>
  private static string ReadFile()
  {
    if (File.Exists(Program.GameDataPath + "/items.yml")) return File.ReadAllText(Program.GameDataPath + "/items.yml");
    File.WriteAllText(Program.GameDataPath + "/items.yml", DefaultFiles.Items);
    return string.Empty;

  }
  
  private static void RegenerateFile()
  {
    // make backup of rooms.yml at rooms.yml.yyMMddHHmmss.backup
    File.Move(Program.GameDataPath + "/items.yml", Program.GameDataPath + $"/items.yml.{DateTime.Now:yyMMddHHmmss}.backup");
    File.WriteAllText(Program.GameDataPath + "/items.yml", DefaultFiles.Items);
  }

  /// <summary>
  ///  Get an item from the registered id
  /// </summary>
  /// <param name="registeredId"> The registered id of the item </param>
  /// <returns> The item </returns>
  public RegisteredItem GetItem(string registeredId)
  {
    return _items[registeredId];
  }

  /// <summary>
  ///  Get all registered items
  /// </summary>
  /// <returns> A list of registered items </returns>
  public List<RegisteredItem> GetItems()
  {
    return _items.Values.ToList();
  }
}