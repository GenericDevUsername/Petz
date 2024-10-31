using petzweb.Models.Actions;

namespace petzweb.Models
{
  public class RegisteredItem: Item
  {
    public string RegisteredId { get; private set; }
    public new List<OnUseAction> OnUse { get; set; } = [];

    public RegisteredItem(string registeredId, Item item) : base(item)
    {
      RegisteredId = registeredId;

      foreach (OnUseAction? onUseAction in item.OnUse.Select(OnUseAction.Parse))
      {
        if (onUseAction == null) continue;
        OnUse.Add(onUseAction);
      }
    }
    
    public void UseItem(Pet pet)
    {
      var executor = new UseActionExecutor();
      executor.ExecuteActions(OnUse, pet);
    }

  }
}
