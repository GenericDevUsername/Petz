namespace petzweb.Models.Game.Actions.Pet.Health;

public class RemoveHealth : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    new AddHealth().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
      variableStore);
  }
}