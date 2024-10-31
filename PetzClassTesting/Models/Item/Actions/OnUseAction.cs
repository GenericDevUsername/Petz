using System.Text.RegularExpressions;
using petzweb.Models.Actions;

namespace petzweb.Models;

public partial class OnUseAction
{
    public required string Action { get; set; }
    public required Dictionary<string, string> Parameters { get; init; }

    internal static readonly ActionRegistry ActionRegistry = new();

    public static OnUseAction? Parse(string input)
    {
        try
        {
            var match = OnUseActionParser().Match(input);
            if (!match.Success) return null;
            
            var actionType = match.Groups["actionType"].Value;
            var parameters = new Dictionary<string, string>();
            var names = match.Groups["name"].Captures;
            var values = match.Groups["value"].Captures;
            for (int i = 0; i < names.Count; i++)
            {
                parameters.Add(names[i].Value, values[i].Value);
            }

            
            if (ActionRegistry.GetAction(actionType, out IUseAction? value) == null) return null;
            return new OnUseAction
            {
                Action = actionType,
                Parameters = parameters,
            };
        }
        catch
        {
            return null;
        }
    }

    [GeneratedRegex(@"(?<actionType>[a-zA-Z0-9_]*?){((?<name>[a-zA-Z0-9_]*?)=(?<value>[^;]*?);*? *?)*?}")]
    private static partial Regex OnUseActionParser();
}