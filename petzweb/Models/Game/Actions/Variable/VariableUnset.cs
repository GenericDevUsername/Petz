namespace petzweb.Models.Game.Actions.Variable;

[SetAliases("unsetVariable", "unsetVar", "varUnset")]
public class VariableUnset : IUseAction
{
    public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (parameters.TryGetValue("var", out string? key)) variableStore.UnsetVariable(key);
    }
}