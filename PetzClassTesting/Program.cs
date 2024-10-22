using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    static void Main(string[] args)
    {
      ItemManager itemManager = new ItemManager();
      //Console.WriteLine(itemManager.items);

      Item[] items = { 
        new Item("apple", "Apple", "A juicy red apple", "food", 64),
        new Item("sword", "Sword", "A sharp sword", "weapon", 1)
      };
      itemManager.saveItem(items);
    }
  }
}
