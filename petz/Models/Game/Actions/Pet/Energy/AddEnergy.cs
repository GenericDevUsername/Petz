namespace petz.Models.Game.Actions.Pet.Energy;

public class AddEnergy : ValueChangeAction
{
  public override void ChangeValue(GameData game, int amount)
  {
    game.Pet.Energy = ClampValue(game.Pet.Energy, amount);
  }
}