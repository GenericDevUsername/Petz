using System.Diagnostics;
using petzweb.Models;

namespace PetzClassTesting
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var itemManager = new ItemManager();
      //Console.WriteLine(itemManager.items);

      /*List<string> onUseActions = [
        "heal{amount=20}",
        "cureIllness{chance=0.5}"
      ];
      RegisteredItem[] items =
      [
        new RegisteredItem("apple", new Item("food", "Apple", "A juicy red apple", 64, onUse: onUseActions)),
        new RegisteredItem("sword", new Item("weapon", "Sword", "A sharp sword", 1))
      ];
      itemManager.SaveItem(items);*/
      
      RegisteredItem paracetamol = itemManager.GetItem("paracetamol");
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
  }
}
