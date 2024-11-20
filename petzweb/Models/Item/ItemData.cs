namespace petzweb.Models.Item;

public class ItemData
{
    public ItemData(ItemData itemData)
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

    public string ItemCategory { get; } = "misc";
    public string Name { get; } = "Unknown Item";
    public string Description { get; } = "";
    public int MaxStackSize { get; } = 10;
    public int MaxUses { get; } = 1;
    public int ShopPrice { get; }
    public List<string> OnUse { get; set; } = [];
}