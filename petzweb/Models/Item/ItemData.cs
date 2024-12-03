namespace petzweb.Models.Item;

public class ItemData
{
  public string ItemCategory { get; set; }
  public string Name { get; set; }
  public string Icon { get; set; }
  public string Description { get; set; }
  public int MaxStackSize { get; set; }
  public int MaxUses { get; set; }
  public int ShopPrice { get; set; }
  public List<string> OnUse { get; set; }
  
  public ItemData(ItemData itemData)
  {
    ItemCategory = itemData.ItemCategory;
    Name = itemData.Name;
    Icon = itemData.Icon;
    Description = itemData.Description;
    MaxStackSize = itemData.MaxStackSize;
    MaxUses = itemData.MaxUses;
    OnUse = itemData.OnUse;
    ShopPrice = itemData.ShopPrice;
  }

  public ItemData()
  {
  }
}