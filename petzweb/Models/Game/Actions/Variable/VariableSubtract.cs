namespace petzweb.Models.Game.Actions.Variable;

[SetAliases("variableSub", "subtractVariable", "subVar", "reduceVar")]
public class VariableSubtract : IUseAction
{
  public void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
  {
    if (!parameters.TryGetValue("var", out string? key) ||
        !parameters.TryGetValue("amount", out string? amount)) return;

    // try to parse the amount as a number (INT or FLOAT or DOUBLE)
    if (int.TryParse(amount, out int intAmount)) variableStore.VariableSubtract(key, intAmount);
  }
}