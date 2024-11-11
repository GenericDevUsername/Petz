namespace petzweb.Models.Actions;

[SetAliases("incrementVariable", "addVar", "varAdd")]
public class VariableAdd : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (!parameters.TryGetValue("var", out var key) ||
            !parameters.TryGetValue("amount", value: out var amount)) return;
        
        // try to parse the amount as a number (INT or FLOAT or DOUBLE)
        if (int.TryParse(amount, out var intAmount))
        {
            variableStore.VariableAdd(key, intAmount);
        }
    }
}