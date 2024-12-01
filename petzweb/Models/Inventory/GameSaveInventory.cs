using petzweb.Models.Game;

namespace petzweb.Models.Inventory;

public class GameSaveInventory
{
  public GameSaveInventory(GameInventory inventory)
  {
    Items = inventory.Items;
    Coins = inventory.Coins;
  }

  public GameSaveInventory()
  {
  }

  public List<InventoryItem> Items { get; set; }
  public int Coins { get; set; }

  public GameInventory ToGameInventory(GameData game)
  {
    return new GameInventory(game)
    {
      Coins = Coins,
      Items = Items
    };
  }
}