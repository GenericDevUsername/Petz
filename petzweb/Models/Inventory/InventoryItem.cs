using petzweb.Models.Game;
using petzweb.Models.Item;

namespace petzweb.Models.Inventory;

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
  
  public void SetInventory(GameInventory inventory)
  {
    Inventory = inventory;
  }
  
  public RegisteredItem GetItem()
  {
    return Item ??= GameManager.ItemManager.GetItem(RegisteredId);
  }
  
  public void RegisterItem(RegisteredItem item)
  {
    if (Item != null) return;
    Item = item;
  }

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
        }
      }
      else if (CurrentUses + amount > Item.MaxUses)
      {
        int remainder = CurrentUses + amount - Item.MaxUses;
        CurrentUses = 0;
        Quantity--;
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

  public void Repair(int amount = 1)
  {
    if (Item == null) GetItem();
    if (Item == null) return;
    while (true)
    {
      switch (CurrentUses - amount)
      {
        case >= 0:
          CurrentUses -= amount;
          break;
        case < 0:
        {
          int remainder = amount - CurrentUses;
          CurrentUses = Item.MaxUses;
          Quantity++;
          if (remainder > 0)
          {
            amount = remainder;
            continue;
          }

          break;
        }
      }

      break;
    }
  }

  public bool Merge(InventoryItem inventoryItem)
  {
    if (Item == null) GetItem();
    if (Item == null) return false;
    if (Item != inventoryItem.Item) return false;

    int total = Quantity + inventoryItem.Quantity;
    if (total <= Item.MaxStackSize)
    {
      Quantity = total;
      Repair(inventoryItem.Item.MaxUses - inventoryItem.CurrentUses);
      inventoryItem.SetQuantity(0);
      return true;
    }

    if (total > Item.MaxStackSize)
    {
      int remainder = total - Item.MaxStackSize;
      Quantity = Item.MaxStackSize;
      Repair(inventoryItem.Item.MaxUses - inventoryItem.CurrentUses);
      inventoryItem.SetQuantity(remainder);
      return true;
    }

    return false;
  }
}