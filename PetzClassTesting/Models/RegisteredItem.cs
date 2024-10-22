using PetzClassTesting.Models;

namespace petzweb.Models
{
  public class RegisteredItem: Item
  {
    public string RegisteredId { get; private set; }


    public RegisteredItem(string registeredId, string name, string description, string itemcategory, int maxStackSize, int maxUses = 1) : base(itemcategory, name, description, maxStackSize, maxUses)
    {
      RegisteredId = registeredId;
    }
    public RegisteredItem(string registeredId, Item item) : base(item)
    {
      RegisteredId = registeredId;
    }

    public void SetRegisteredId(string registeredId)
    {
      RegisteredId = registeredId;
    }

  }
}
