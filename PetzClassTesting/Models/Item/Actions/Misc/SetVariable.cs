namespace petzweb.Models.Actions;

[SetAliases("setvar", "variableset")]
public class SetVariable : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (parameters.TryGetValue("var", out var key) && parameters.TryGetValue("value", value: out var value))
        {
            variableStore.SetVariable(key, value);
        }
    }
}