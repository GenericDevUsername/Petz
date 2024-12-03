namespace petz.Models.Game.Actions.Pet.Coins;

public class RemoveCoins : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    new AddCoins().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
      variableStore);
  }
}