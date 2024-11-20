using System.Data;

namespace petzweb.Models.Game.Actions;

public class VariableStore
{
    private readonly Dictionary<string, string> _readOnlyVariables = new()
    {
        // PLACEHOLDERS
        ["&sp"] = " ",
        ["&cm"] = ",",
        ["&sc"] = ";",
        ["&eq"] = "=",
        ["&dq"] = "\"",
        ["&rb"] = "]",
        ["&lb"] = "[",
        ["&rc"] = "}",
        ["&lc"] = "{",
        ["&nm"] = "#",
        ["&nl"] = "\n",
        ["&heart"] = "❤",
        ["&skull"] = "☠",
        ["&co"] = ";",
        ["&sq"] = "'",
        ["&da"] = "-",
        ["&bs"] = "\\",
        ["&fs"] = "/"
    };

    private readonly Dictionary<string, string?> _variables = new();

    // Initializes and sets a variable.
    public void SetVariable(string key, string value)
    {
        if (_readOnlyVariables.ContainsKey(key)) return;
        _variables[key] = value;

        Console.WriteLine($"[DEBUG] Set variable \"{key}\" to \"{value}\"");
    }

    // Gets the variable. If the variable is not set, it returns the fallback value.
    public string? GetVariable(string key, string? fallback = null)
    {
        if (_variables.TryGetValue(key, out string? value))
        {
            Console.WriteLine($"[DEBUG] Variable \"{key}\" found, returning value \"{value}\"");
            return value;
        }

        Console.WriteLine($"[DEBUG] Variable \"{key}\" not found, returning fallback value \"{fallback}\"");
        return _readOnlyVariables.TryGetValue(key, out string? readOnlyValue) ? readOnlyValue : fallback;
    }

    // Unsets the variable.
    public void UnsetVariable(string key)
    {
        _variables.Remove(key);

        Console.WriteLine($"[DEBUG] Unset variable \"{key}\"");
    }

    // Adds to a numeric variable.
    public void VariableAdd(string key, int value)
    {
        if (!_variables.TryGetValue(key, out string? current)) return;
        if (int.TryParse(current, out int currentInt))
            _variables[key] = (currentInt + value).ToString();
        else if (double.TryParse(current, out double currentDouble))
            _variables[key] = (currentDouble + value).ToString();
        Console.WriteLine($"[DEBUG] Added \"{value}\" to variable \"{key}\"");
    }

    // Subtracts from a numeric variable.
    public void VariableSubtract(string key, int value)
    {
        if (!_variables.TryGetValue(key, out string? current)) return;
        if (int.TryParse(current, out int currentInt))
            _variables[key] = (currentInt - value).ToString();
        else if (double.TryParse(current, out double currentDouble))
            _variables[key] = (currentDouble - value).ToString();
        Console.WriteLine($"[DEBUG] Subtracted \"{value}\" from variable \"{key}\"");
    }

    // Lets you do calculations with numeric variables.
    public void VariableMath(string key, string expression)
    {
        if (!_variables.TryGetValue(key, out string? current)) return;
        if (!int.TryParse(current, out _) && !double.TryParse(current, out _)) return;

        object result = new DataTable().Compute(expression, null);
        _variables[key] = result.ToString();
        Console.WriteLine($"[DEBUG] Calculated \"{expression}\" on variable \"{key}\" with result \"{result}\"");
    }
}