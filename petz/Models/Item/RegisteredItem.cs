using petz.Models.Game;
using petz.Models.Game.Actions;

namespace petz.Models.Item;

public class RegisteredItem : ItemData
{
  public RegisteredItem(string registeredId, ItemData itemData) : base(itemData)
  {
    RegisteredId = registeredId;

    foreach (OnUseAction? onUseAction in itemData.OnUse.Select(OnUseAction.Parse))
    {
      if (onUseAction == null) continue;
      OnUse.Add(onUseAction);
    }
  }

  public string RegisteredId { get; private set; }
  public new List<OnUseAction> OnUse { get; set; } = [];

  public void UseItem(GameData game)
  {
    Program.Log($"[DEBUG] Using item {Name} with {OnUse.Count} actions");
    UseActionExecutor executor = new(game);
    
    executor.ExecuteActions(OnUse);
  }
}