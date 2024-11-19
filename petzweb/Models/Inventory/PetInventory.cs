namespace petzweb.Models.Inventory;
public class PetInventory
{
  public Pet Pet { get; private set; }
  public List<InventoryItem> Items { get; private set; } = [];

  public PetInventory(Pet pet)
  {
    Pet = pet;
  }

  public void AddItem(RegisteredItem item, int quantity)
  {
    var existingItem = Items.FirstOrDefault(i => i.Item == item);
    if (existingItem != null)
    {
      existingItem.Add(quantity);
    }
    else
    {
      Items.Add(new InventoryItem(item, quantity, this));
    }
  }
}