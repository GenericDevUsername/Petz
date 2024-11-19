namespace petzweb.Models;
public class ItemData
{
  public string ItemCategory { get; private set; } = "misc";
  public string Name { get; private set; } = "Unknown Item";
  public string Description { get; private set; } = "";
  public int MaxStackSize { get; private set; } = 10;
  public int MaxUses { get; private set; } = 1;
  public int ShopPrice { get; private set; } = 0;
  public List<string> OnUse { get; set; } = [];

  protected ItemData(ItemData itemData)
  {
    ItemCategory = itemData.ItemCategory;
    Name = itemData.Name;
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