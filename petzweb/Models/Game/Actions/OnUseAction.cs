using System.Text.RegularExpressions;

namespace petzweb.Models.Game.Actions;

public partial class OnUseAction
{
    internal static readonly ActionRegistry ActionRegistry = new();
    public required string Action { get; set; }
    public required Dictionary<string, string> Parameters { get; init; }

    public static OnUseAction? Parse(string input)
    {
        try
        {
            Match match = OnUseActionParser().Match(input);
            if (!match.Success) return null;

            string actionType = match.Groups["actionType"].Value;
            Dictionary<string, string> parameters = new();
            CaptureCollection names = match.Groups["name"].Captures;
            CaptureCollection values = match.Groups["value"].Captures;
            for (int i = 0; i < names.Count; i++) parameters.Add(names[i].Value, values[i].Value);


            if (ActionRegistry.GetAction(actionType, out IUseAction? value) == null) return null;
            return new OnUseAction
            {
                Action = actionType,
                Parameters = parameters
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