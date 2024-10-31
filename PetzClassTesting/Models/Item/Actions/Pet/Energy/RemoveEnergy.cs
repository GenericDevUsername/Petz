namespace petzweb.Models.Actions;

public class RemoveEnergy : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddEnergy().Execute(pet, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } }, variableStore);
    }
}