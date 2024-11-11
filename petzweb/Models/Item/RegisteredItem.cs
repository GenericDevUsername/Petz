using petzweb.Models.Actions;

namespace petzweb.Models;

public class RegisteredItem: ItemData
{
  public string RegisteredId { get; private set; }
  public new List<OnUseAction> OnUse { get; set; } = [];

  public RegisteredItem(string registeredId, ItemData itemData) : base(itemData)
  {
    RegisteredId = registeredId;

    foreach (OnUseAction? onUseAction in itemData.OnUse.Select(OnUseAction.Parse))
    {
      if (onUseAction == null) continue;
      OnUse.Add(onUseAction);
    }
  }
    
  public void UseItem(Pet pet)
  {
    UseActionExecutor executor = new UseActionExecutor();
    executor.ExecuteActions(OnUse, pet);
  }

}