namespace petzweb.Models;

public class Inventoryitem(RegisteredItem item, int quantity, Inventory inventory)
{
  public RegisteredItem Item { get; private set; } = item;
  private Inventory Inventory { get; set; } = inventory;
  public int CurrentUses { get; private set; } = 0;
  public int Quantity { get; private set; } = quantity;

  public void Add(int quantity)
  {
    Quantity += quantity;
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
        int remainder = (CurrentUses + amount) - Item.MaxUses;
        CurrentUses = 0;
        Quantity--;
        if (remainder > 0)
        {
          amount = remainder;
          continue;
        }
      }
          
      Item.UseItem(Inventory.Pet);

      break;
    }
  }

  public void Repair(int amount = 1)
  {
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

  public bool Merge(Inventoryitem inventoryItem)
  {
    if (Item != inventoryItem.Item)
    {
      return false;
    }

    int total = Quantity + inventoryItem.Quantity;
    if (total <= Item.MaxStackSize)
    {
      Quantity = total;
      this.Repair(inventoryItem.Item.MaxUses - inventoryItem.CurrentUses);
      inventoryItem.SetQuantity(0);
      return true;
    }
    else if (total > Item.MaxStackSize)
    {
      int remainder = total - Item.MaxStackSize;
      Quantity = Item.MaxStackSize;
      this.Repair(inventoryItem.Item.MaxUses - inventoryItem.CurrentUses);
      inventoryItem.SetQuantity(remainder);
      return true;
    }
    else
    {
      return false;
    }
  }

}