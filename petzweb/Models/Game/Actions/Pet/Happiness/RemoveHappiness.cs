namespace petzweb.Models.Game.Actions.Pet.Happiness;

public class RemoveHappiness : IUseAction
{
    public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddHappiness().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
            variableStore);
    }
}