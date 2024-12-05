using petz.Models.Game;

namespace petz.Models.Inventory;

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

  /// <summary>
  ///  Convert the game save inventory to a game inventory
  /// </summary>
  /// <param name="game"> The game data to attach the inventory to </param>
  /// <returns> The game inventory if the save was converted successfully </returns>
  public GameInventory ToGameInventory(GameData game)
  {
    GameInventory inv = new GameInventory(game)
    {
      Coins = Coins,
      Items = Items
    };
    foreach (InventoryItem t in inv.Items)
    {
      t.SetInventory(inv);
    }

    return inv;
  }
}