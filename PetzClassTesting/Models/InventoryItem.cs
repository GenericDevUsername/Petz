namespace petzweb.Models
{
    public class Inventoryitem : RegisteredItem
  {
    public RegisteredItem Item { get; private set; }
    public int CurrentUses { get; private set; }
    public int Quantity { get; private set; }

    public Inventoryitem(RegisteredItem item, int quantity) : base(item.RegisteredId, item.Name, item.Description, item.ItemCategory, item.MaxStackSize, item.MaxUses)
    {
      Item = item;
      Quantity = quantity;
    }
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
          Use(remainder);
        }
      }
    }

    public void Repair(int amount = 1)
    {
      if (CurrentUses - amount >= 0)
      {
        CurrentUses -= amount;
      }
      else if (CurrentUses - amount < 0)
      {
        int remainder = amount - CurrentUses;
        CurrentUses = Item.MaxUses;
        Quantity++;
        if (remainder > 0)
        {
          Repair(remainder);
        }
      }
    }

    public bool Merge(Inventoryitem item)
    {
      if (Item != item.Item)
      {
        return false;
      }

      int total = Quantity + item.Quantity;
      if (total <= MaxStackSize)
      {
        Quantity = total;
        this.Repair(item.MaxUses - item.CurrentUses);
        item.SetQuantity(0);
        return true;
      }
      else if (total > MaxStackSize)
      {
        int remainder = total - MaxStackSize;
        Quantity = MaxStackSize;
        this.Repair(item.MaxUses - item.CurrentUses);
        item.SetQuantity(remainder);
        return true;
      }
      else
      {
        return false;
      }
    }

  }
}
