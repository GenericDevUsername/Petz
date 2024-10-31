namespace petzweb.Models
{
  public class Item
  {
    public string ItemCategory { get; private set; } = "misc";
    public string Name { get; private set; } = "Unknown Item";
    public string Description { get; private set; } = "";
    public int MaxStackSize { get; private set; } = 10;
    public int MaxUses { get; private set; } = 1;
    public int ShopPrice { get; private set; } = 0;
    public List<string> OnUse { get; set; } = [];

    public Item(string itemCategory, string name, string description, int maxStackSize, int maxUses = 1, int shopPrice = 0, List<string>? onUse = null)
    {
      ItemCategory = itemCategory;
      Name = name;
      Description = description;
      MaxStackSize = maxStackSize;
      MaxUses = maxUses;
      OnUse = onUse ?? [];
      ShopPrice = shopPrice;
    }
    public Item(Item item)
    {
      ItemCategory = item.ItemCategory;
      Name = item.Name;
      Description = item.Description;
      MaxStackSize = item.MaxStackSize;
      MaxUses = item.MaxUses;
      OnUse = item.OnUse;
      ShopPrice = item.ShopPrice;
    }
    public Item()
    {
    }
  }
}
