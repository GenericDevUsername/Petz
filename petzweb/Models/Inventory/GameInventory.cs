using petzweb.Models.Game;
using petzweb.Models.Item;
using petzweb.Models.Room;

namespace petzweb.Models.Inventory;

public class GameInventory(GameData game)
{
    public GameData GameData { get; private set; } = game;
    public List<InventoryItem> Items { get; init; } = [];
    public int Coins { get; internal set; } = 0;
    
    public GameSaveInventory ToGameSaveInventory()
    {
        return new GameSaveInventory(this);
    }
    
    public void AddItem(RegisteredItem item, int quantity)
    {
        InventoryItem? existingItem = Items.FirstOrDefault(i => i.Item == item);
        if (existingItem != null)
            existingItem.Add(quantity);
        else
            Items.Add(new InventoryItem(item, quantity, this));
    }


    public void ModifyCoins(int amount)
    {
        Coins += Coins - amount < 0 ? 0 : amount;
    }
    
    public void SetCoins(int amount)
    {
        Coins = amount;
    }
    
}