using petzweb.Models.Game;
using petzweb.Models.Game.Actions;

namespace petzweb.Models.Item;

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
    UseActionExecutor executor = new();
    executor.ExecuteActions(OnUse, game);
  }
}