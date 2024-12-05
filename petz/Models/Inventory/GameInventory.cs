using petz.Models.Game;
using petz.Models.Item;

namespace petz.Models.Inventory;

public class GameInventory(GameData game)
{
  public GameData GameData { get; private set; } = game;
  public List<InventoryItem> Items { get; init; } = [];
  public int Coins { get; internal set; }

  public GameSaveInventory ToGameSaveInventory()
  {
    return new GameSaveInventory(this);
  }

  /// <summary>
  ///  Add an item to the inventory
  /// </summary>
  /// <param name="item"> The item to add </param>
  /// <param name="quantity"> The quantity of the item to add </param>
  public void AddItem(RegisteredItem item, int quantity)
  {
    InventoryItem? existingItem = Items.FirstOrDefault(i => i.GetItem() == item);
    if (existingItem != null && existingItem.Quantity + quantity <= item.MaxStackSize)
      existingItem.Add(quantity);
    else
      Items.Add(new InventoryItem(item, quantity, this));
  }
  
  /// <summary>
  ///  Modify the coins value in the inventory
  /// </summary>
  /// <param name="amount"> The amount to modify the coins by, can be negative </param>
  public void ModifyCoins(int amount)
  {
    Coins += Coins - amount < 0 ? 0 : amount;
  }
}