using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    static void Main(string[] args)
    {
      ItemManager itemManager = new ItemManager();
      //Console.WriteLine(itemManager.items);

      petzweb.Models.Item[] items = new petzweb.Models.Item[1];
      itemManager.saveItem(items);
    }
  }
}
