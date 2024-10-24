using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    static void Main(string[] args)
    {
      ItemManager itemManager = new ItemManager();
      //Console.WriteLine(itemManager.items);

      OnUse onUsee = new OnUse
      {
        Actions = new List<OnUseAction>
        {
          new OnUseAction
          {
            Action = "heal",
            Parameters = new Dictionary<string, string> { { "amount", "20"} },
          }
        }
      };
      RegisteredItem[] items = {
        new RegisteredItem("apple", new Item("food", "Apple", "A juicy red apple", 64, onUse: onUsee)),
        new RegisteredItem("sword", new Item("weapon", "Sword", "A sharp sword", 1))
      };
      itemManager.saveItem(items);
    }
  }
}
