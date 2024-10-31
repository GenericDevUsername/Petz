using petzweb.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models
{
  public class ItemManager
  {
    public readonly Dictionary<string, RegisteredItem> Items = new();

    public ItemManager()
    {
      this.LoadItems();
    }

    private void LoadItems()
    {
      var yamlFile = ReadItemsFile();

      // Load items from data/items.yml file
      var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

      var loadedItems = deserializer.Deserialize<Dictionary<string, Item>>(yamlFile);
      // register items
      foreach (var item in loadedItems)
      {
        Items.Add(item.Key, new RegisteredItem(item.Key, item.Value));
      }
    }

    public void SaveItem(RegisteredItem[] itemsToSave)
    {
      var serializer = new SerializerBuilder()
          .WithNamingConvention(CamelCaseNamingConvention.Instance)
          .Build();

      // remove registeredId from dictionary
      var itemDict = itemsToSave.ToDictionary(item => item.RegisteredId, item => {
        Item itemCopy = new Item(item);
        return itemCopy;
      });

      var yaml = serializer.Serialize(itemDict);

      File.WriteAllText("Data/Items.yml", yaml);
    }


    private static string ReadItemsFile()
    {
      if (!File.Exists("Data/Items.yml"))
      {
        File.WriteAllText("Data/Items.yml", "");
        return string.Empty;
      }
      else
      {
        return File.ReadAllText("Data/Items.yml");
      }
    }

    public RegisteredItem GetItem(string registeredId)
    {
      return Items[registeredId];
    }
  }
}
