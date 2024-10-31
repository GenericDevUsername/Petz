namespace petzweb.Models.Actions;

public class RemoveLove : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        new AddLove().Execute(pet, new Dictionary<string, string> { { "amount", "-" + parameters["amount"] } }, variableStore);
    }
}