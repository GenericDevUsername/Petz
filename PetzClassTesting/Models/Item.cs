using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petzweb.Models
{
  public class OnUseAction
  {
    public string Action { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
  }

  public class OnUse
  {
    public List<OnUseAction> Actions { get; set; }
  }

  public class Item
  {
    public string ItemCategory { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int MaxStackSize { get; private set; }
    public int MaxUses { get; private set; }
    public OnUse OnUse { get; set; } = new OnUse { Actions = new List<OnUseAction>() };


    public Item(string itemCategory, string name, string description, int maxStackSize, int maxUses = 1, OnUse onUse = null)
    {
      ItemCategory = itemCategory;
      Name = name;
      Description = description;
      MaxStackSize = maxStackSize;
      MaxUses = maxUses;
      OnUse = onUse ?? new OnUse { Actions = new List<OnUseAction>() };
    }
    public Item(Item item)
    {
      ItemCategory = item.ItemCategory;
      Name = item.Name;
      Description = item.Description;
      MaxStackSize = item.MaxStackSize;
      MaxUses = item.MaxUses;
      OnUse = item.OnUse;
    }
    public Item()
    {
    }
  }
}
