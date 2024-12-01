namespace petzweb.Models.Game.Actions.Pet.Health;

public class AddHealth : ValueChangeAction
{
  public override void ChangeValue(GameData game, int amount)
  {
    game.Pet.Health = ClampValue(game.Pet.Health, amount, game.Pet.MaxHealth);
  }
}