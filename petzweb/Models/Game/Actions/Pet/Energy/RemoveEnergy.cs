namespace petzweb.Models.Game.Actions.Pet.Energy;

public class RemoveEnergy : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    new AddEnergy().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
      variableStore);
  }
}