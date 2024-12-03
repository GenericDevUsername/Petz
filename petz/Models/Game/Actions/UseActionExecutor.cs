using System.Text.RegularExpressions;

namespace petz.Models.Game.Actions;

public partial class UseActionExecutor(GameData game)
{
  private readonly VariableStore _variableStore = new(game);

  [GeneratedRegex(@"<(?<key>[^<>|]+)(\|(?<fallback>[^<>]*))?>")]
  private static partial Regex VariableRegex();

  private void ReplaceVariablesInParameters(OnUseAction action)
  {
    foreach (KeyValuePair<string, string> parameter in action.Parameters)
      action.Parameters[parameter.Key] = ReplaceVariablesInString(parameter.Value, _variableStore);
  }

  private static string ReplaceVariablesInString(string input, VariableStore store)
  {
    string result;
    do
    {
      result = input;
      input = VariableRegex().Replace(input, match =>
      {
        string varName = match.Groups["key"].Value;
        string fallback = match.Groups["fallback"].Value;

        string? variableValue = store.GetVariable(varName);
        return variableValue ?? fallback;
      });
    } while (result != input);

    return result;
  }


  private static bool HandleUniversalAttributes(OnUseAction action)
  {
    foreach (KeyValuePair<string, string> parameter in action.Parameters)
      switch (parameter.Key.ToLower())
      {
        case "chance":
          if (!ChanceAttribute(parameter.Value))
          {
            Program.Log("[DEBUG] Failed chance check on " + action.Action +
                              " action with parameters: " + parameter.Value);
            return false;
          }

          break;
      }

    return true;
  }

  private static bool ChanceAttribute(string chance)
  {
    if (!double.TryParse(chance, out double chanceValue)) return false;
    Random random = new();
    double check = random.NextDouble();
    return check < chanceValue;
  }

  public void ExecuteActions(List<OnUseAction> actions)
  {
    foreach (OnUseAction action in actions) ExecuteAction(action, game);
  }

  public void ExecuteAction(OnUseAction action, GameData gameData)
  {
    // make a copy of onUseAction so it doesnt overwrite the original
    action = new OnUseAction
    {
      Action = action.Action,
      Parameters = new Dictionary<string, string>(action.Parameters)
    };
    // execution checks
    if (OnUseAction.ActionRegistry.GetAction(action.Action, out IUseAction? value) == null)
    {
      Program.Log("[DEBUG] Failed to find action " + action.Action);
      return;
    }
    
    ReplaceVariablesInParameters(action);
    if (!HandleUniversalAttributes(action)) return;

    Program.Log("[DEBUG] Executing " + action.Action + " action with parameters: " +
                string.Join(", ", action.Parameters));
    value?.Execute(gameData, action.Parameters, _variableStore);
  }
}