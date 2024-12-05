using petz.Models.Game;
using petz.Models.Item;

namespace petz.Models.Inventory;

public class InventoryItem
{
  private RegisteredItem? Item { get; set;  }
  public string RegisteredId;
  private GameInventory Inventory { get; set; }
  public int CurrentUses { get; set; }
  public int Quantity { get; set; }

  public InventoryItem(RegisteredItem item, int quantity, GameInventory inventory)
  {
    Item = item;
    RegisteredId = item.RegisteredId;
    Quantity = quantity;
    Inventory = inventory;
  }
  
  public InventoryItem()
  {
  }
  
  /// <summary>
  ///  Attach the item to the inventory
  /// </summary>
  /// <param name="inventory"> The inventory to attach the item to </param>
  public void SetInventory(GameInventory inventory)
  {
    Inventory = inventory;
  }
  
  /// <summary>
  ///  Get the item from the registered id
  /// </summary>
  /// <returns> The item </returns>
  public RegisteredItem GetItem()
  {
    return Item ??= GameManager.Items.GetItem(RegisteredId);
  }

  /// <summary>
  ///  Add an amount of the item to the inventory slot
  /// </summary>
  /// <param name="quantity"> The amount of the item to add </param>
  public void Add(int quantity)
  {
    if (Item == null) GetItem();
    if (Item == null) return;
    if (Quantity + quantity <= Item.MaxStackSize)
    {
      Quantity += quantity;
    }
    else if (Quantity + quantity > Item.MaxStackSize)
    {
      int remainder = Quantity + quantity - Item.MaxStackSize;
      Quantity = Item.MaxStackSize;
      if (remainder > 0) Inventory.AddItem(Item, remainder);
    }
  }

  public void Remove(int quantity)
  {
    Quantity -= quantity;
  }

  public void SetQuantity(int quantity)
  {
    Quantity = quantity;
  }

  /// <summary>
  ///  Use the item
  /// </summary>
  /// <param name="amount"> The amount of the item to use </param>
  public void Use(int amount = 1)
  {
    if (Item == null) GetItem();
    if (Item == null) return;
    while (true)
    {
      if (CurrentUses + amount <= Item.MaxUses)
      {
        CurrentUses += amount;
        if (CurrentUses == Item.MaxUses)
        {
          Quantity--;
          CurrentUses = 0;
          if (Quantity <= 0)
          {
            Inventory.Items.Remove(this);
            break;
          }
        }
      }
      else if (CurrentUses + amount > Item.MaxUses)
      {
        int remainder = CurrentUses + amount - Item.MaxUses;
        CurrentUses = 0;
        Quantity--;
        if (Quantity <= 0)
        {
          if (remainder > 0) Inventory.AddItem(Item, remainder);
          break;
        }
        if (remainder > 0)
        {
          amount = remainder;
          continue;
        }
      }

      Item.UseItem(Inventory.GameData);

      break;
    }
  }
}