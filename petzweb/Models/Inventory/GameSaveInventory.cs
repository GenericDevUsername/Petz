using petzweb.Models.Game;

namespace petzweb.Models.Inventory;

public class GameSaveInventory
{
    public List<InventoryItem> Items { get; set;  }
    public int Coins { get; set; }
    
    public GameSaveInventory(GameInventory inventory)
    {
        Items = inventory.Items;
        Coins = inventory.Coins;
    }
    public GameSaveInventory()
    {
    }
    
    public GameInventory ToGameInventory(GameData game)
    {
        return new GameInventory(game)
        {
            Coins = Coins,
            Items = Items
        };
    }
}