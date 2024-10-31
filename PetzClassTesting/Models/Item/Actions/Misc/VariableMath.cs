namespace petzweb.Models.Actions;

[SetAliases("mathVariable", "varmath", "mathvar")]
public class VariableMath : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (!parameters.TryGetValue("var", out var key) ||
            !parameters.TryGetValue("equation", value: out var equation)) return;
        
        if (variableStore.GetVariable(key) is not null)
        {
            variableStore.VariableMath(key, equation);
        }
    }
}