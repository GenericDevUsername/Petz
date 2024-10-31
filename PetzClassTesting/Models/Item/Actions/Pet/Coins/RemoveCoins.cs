namespace petzweb.Models.Actions;

public class RemoveCoins : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddCoins().Execute(pet, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } }, variableStore);
    }
}