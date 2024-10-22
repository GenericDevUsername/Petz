namespace petzweb.Models
{
  public class Item
  {
    public string RegisteredId { get; private set; }
    public string ItemCategory { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int MaxStackSize { get; private set; }
    public int MaxUses { get; private set; }


    public Item(string registeredId, string name, string description, string itemcategory, int maxStackSize, int maxUses = 1)
    {
      RegisteredId = registeredId;
      ItemCategory = itemcategory;
      Name = name;
      Description = description;
      MaxStackSize = maxStackSize;
      MaxUses = maxUses;
    }


  }
}
