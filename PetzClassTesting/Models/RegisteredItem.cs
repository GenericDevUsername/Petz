namespace petzweb.Models
{
  public class RegisteredItem: Item
  {
    public string RegisteredId { get; private set; }

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
