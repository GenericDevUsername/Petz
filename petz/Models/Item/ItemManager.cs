using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petz.Models.Item;

public class ItemManager
{
  private readonly Dictionary<string, RegisteredItem> _items = new();

  /// <summary>
  ///  Load items from the items.yml file
  /// </summary>
  internal void LoadItems()
  {
    string yamlFile = ReadFile();

    // Load items from data/items.yml file
    IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build();

    Dictionary<string, ItemData>? loadedItems =
      deserializer.Deserialize<Dictionary<string, ItemData>?>(yamlFile) ?? [];

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