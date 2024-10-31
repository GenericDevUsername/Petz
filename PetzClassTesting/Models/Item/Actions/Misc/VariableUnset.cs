namespace petzweb.Models.Actions;

[SetAliases("unsetVariable", "unsetVar", "varUnset")]
public class VariableUnset : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (parameters.TryGetValue("var", out var key))
        {
            variableStore.UnsetVariable(key);
        }
    }
}