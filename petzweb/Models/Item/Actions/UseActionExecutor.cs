using System.Text.RegularExpressions;


namespace petzweb.Models;
public partial class UseActionExecutor
{
    [GeneratedRegex(@"<(?<key>[^<>|]+)(\|(?<fallback>[^<>]*))?>")]
    private static partial Regex VariableRegex();
    
    private readonly VariableStore _variableStore = new();

    private void ReplaceVariablesInParameters(OnUseAction action)
    {
        foreach (var parameter in action.Parameters)
        {
            action.Parameters[parameter.Key] = ReplaceVariablesInString(parameter.Value, _variableStore);
        }
    }

    private static string ReplaceVariablesInString(string input, VariableStore store)
    {
        string result;
        do
        {
            result = input;
            input = VariableRegex().Replace(input, match =>
            {
                var varName = match.Groups["key"].Value;
                var fallback = match.Groups["fallback"].Value;

                var variableValue = store.GetVariable(varName);
                return variableValue ?? fallback;
            });
        }
        while (result != input);

        return result;
    }


    private static bool HandleUniversalAttributes(OnUseAction action)
    {
        foreach (var parameter in action.Parameters)
        {
            switch (parameter.Key.ToLower())
            {
                case "chance":
                    if (!ChanceAttribute(parameter.Value))
                    {
                        Console.WriteLine("[DEBUG] Failed chance check on " + action.Action + " action with parameters: " + parameter.Value);
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    private static bool ChanceAttribute(string chance)
    {
        if (!double.TryParse(chance, out var chanceValue)) return false;
        var random = new Random();
        var check = random.NextDouble();
        return check < chanceValue;
    }
    
    public void ExecuteActions(List<OnUseAction> actions, Pet pet)
    {
        foreach (var action in actions)
        {
            ExecuteAction(action, pet);
        }
    }
    
    public void ExecuteAction(OnUseAction action, Pet pet)
    {
        // execution checks
        if (OnUseAction.ActionRegistry.GetAction(action.Action, out var value) == null)
        { 
            Console.WriteLine("[DEBUG] Failed to find action " + action.Action);
            return;
        };
        ReplaceVariablesInParameters(action);
        if (!HandleUniversalAttributes(action)) return;
        
        Console.WriteLine("[DEBUG] Executing " + action.Action + " action with parameters: " + string.Join(", ", action.Parameters));
        value?.Execute(pet, action.Parameters, _variableStore);
    }
}