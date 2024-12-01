namespace petzweb.Models.Game.Actions.Pet.Hunger;

[SetAliases("addHunger")]
public class AddHunger : ValueChangeAction
{
  public override void ChangeValue(GameData game, int amount)
  {
    game.Pet.Hunger = ClampValue(game.Pet.Hunger, amount, game.Pet.MaxHunger);
  }
}