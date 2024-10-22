using PetzClassTesting.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace petzweb.Models
{
  public class ItemManager
  {
    public Dictionary<string, Item> items = new Dictionary<string, Item>();

    public ItemManager()
    {
      this.loadItems();
    }

    public void loadItems()
    {
      var yamlFile = readItemsFile();

      // Load items from data/items.yml file
      var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

      var loadedItems = deserializer.Deserialize<Dictionary<string, Item>>(yamlFile);
      // register items
      foreach (var item in loadedItems)
      {
        items.Add(item.Key, new RegisteredItem(item.Key, item.Value));
      }
    }

    public void saveItem(RegisteredItem[] items)
    {
      var serializer = new SerializerBuilder()
          .WithNamingConvention(CamelCaseNamingConvention.Instance)
          .Build();

      // remove registeredId from dictionary
      var itemDict = items.ToDictionary(item => item.RegisteredId, item => {
        var itemCopy = item;

        return itemCopy;
      });

      var yaml = serializer.Serialize(itemDict);

      File.WriteAllText("Data/Items.yml", yaml);
    }


    public string readItemsFile()
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
  }
}
