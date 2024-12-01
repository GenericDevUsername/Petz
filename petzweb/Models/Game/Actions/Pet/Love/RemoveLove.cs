namespace petzweb.Models.Game.Actions.Pet.Love;

public class RemoveLove : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    new AddLove().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
      variableStore);
  }
}