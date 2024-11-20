namespace petzweb.Models.Game.Actions.Pet.Hunger;

public class RemoveHunger : IUseAction
{
    public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddHunger().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
            variableStore);
    }
}