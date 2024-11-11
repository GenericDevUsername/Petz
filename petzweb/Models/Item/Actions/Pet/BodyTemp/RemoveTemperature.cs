namespace petzweb.Models.Actions;

[SetAliases("removetemp", "tempremove", "temperatureremove")]
public class RemoveTemperature : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddTemperature().Execute(pet, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } }, variableStore);
    }
}