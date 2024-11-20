namespace petzweb.Models.Game.Actions.Pet.BodyTemp;

[SetAliases("removetemp", "tempremove", "temperatureremove")]
public class RemoveTemperature : IUseAction
{
    public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddTemperature().Execute(game, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } },
            variableStore);
    }
}