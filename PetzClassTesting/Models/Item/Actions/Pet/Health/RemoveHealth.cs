namespace petzweb.Models.Actions;

public class RemoveHealth : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddHealth().Execute(pet, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } }, variableStore);
    }
}