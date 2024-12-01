namespace petzweb.Models.Game.Actions.Variable;

[SetAliases("mathVariable", "varmath", "mathvar")]
public class VariableMath : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    if (!parameters.TryGetValue("var", out string? key) ||
        !parameters.TryGetValue("equation", out string? equation)) return;

    if (variableStore.GetVariable(key) is not null) variableStore.VariableMath(key, equation);
  }
}