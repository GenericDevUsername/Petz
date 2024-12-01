namespace petzweb.Models.Game.Actions.Pet.Happiness;

public class AddHappiness : ValueChangeAction
{
  public override void ChangeValue(GameData game, int amount)
  {
    game.Pet.Happiness = ClampValue(game.Pet.Happiness, amount, game.Pet.MaxHappiness);
  }
}