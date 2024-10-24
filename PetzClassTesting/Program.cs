using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    static void Main(string[] args)
    {
      ItemManager itemManager = new ItemManager();
      //Console.WriteLine(itemManager.items);

      RegisteredItem[] items = { 
        new RegisteredItem("apple", "Apple", "A juicy red apple", "food", 64),
        new RegisteredItem("sword", "Sword", "A sharp sword", "weapon", 1)
      };
      itemManager.saveItem(items);
    }
  }
}
