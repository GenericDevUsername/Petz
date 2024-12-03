namespace petz.Models.Game.Actions.Pet.IsSick;

public class CureIllness : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    // Cure the pet's illness by setting the pet's illness to false
    game.Pet.IsSick = false;
  }
}