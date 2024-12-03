namespace petz.Models.Game.Actions.Variable;

[SetAliases("setvar", "variableset")]
public class SetVariable : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    if (parameters.TryGetValue("var", out string? key) && parameters.TryGetValue("value", out string? value))
      variableStore.SetVariable(key, value);
  }
}