using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models.Item;

public class ItemManager
{
  private readonly Dictionary<string, RegisteredItem> _items = new();

  internal void LoadItems()
  {
    string yamlFile = ReadItemsFile();

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
        Console.WriteLine($"[ERROR] Failed to register item {item.Key}: {e.Message}");
      }

    Console.WriteLine($"[DEBUG] Registered {registeredItemCount} items");
  }

  private static string ReadItemsFile()
  {
    if (!File.Exists(Program.GameDataPath + "/items.yml"))
    {
      File.WriteAllText(Program.GameDataPath + "/items.yml", "");
      return string.Empty;
    }

    return File.ReadAllText(Program.GameDataPath + "/items.yml");
  }

  public RegisteredItem GetItem(string registeredId)
  {
    return _items[registeredId];
  }

  public List<RegisteredItem> GetItems()
  {
    return _items.Values.ToList();
  }
}