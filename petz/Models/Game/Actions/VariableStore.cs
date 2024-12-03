using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace petz.Models.Game.Actions;

public partial class VariableStore
{
  private readonly Dictionary<string, string?> _variables = new();
  private readonly Dictionary<string, string?> _readOnlyVariables = new()
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
  // dynamic variables ( variables with => operator )
  private readonly Dictionary<string, Func<string>> _dynamicVariables = new();
  
  // inserts game variables
  public VariableStore(GameData game)
  {
    _dynamicVariables["pet.name"] = () => game.Pet.Name;
    _dynamicVariables["pet.health"] = () => game.Pet.Health.ToString();
    _dynamicVariables["pet.hunger"] = () => game.Pet.Hunger.ToString();
    _dynamicVariables["pet.happiness"] = () => game.Pet.Happiness.ToString();
    _dynamicVariables["pet.energy"] = () => game.Pet.Energy.ToString();
    _dynamicVariables["pet.love"] = () => game.Pet.Love.ToString();
    _dynamicVariables["pet.isSick"] = () => game.Pet.IsSick.ToString();
    _dynamicVariables["pet.bodyTemperature"] = () => game.Pet.BodyTemperature.ToString();
    _dynamicVariables["pet.dateCreated"] = () =>
    {
      DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      TimeSpan timeSpan = game.Pet.DateCreated - unixEpoch;
      return timeSpan.TotalSeconds.ToString();
    };
    _dynamicVariables["pet.bodyTemperature.min"] = () => game.Pet.MinBodyTemperature.ToString();
    _dynamicVariables["pet.bodyTemperature.max"] = () => game.Pet.MaxBodyTemperature.ToString();
    
    
    _dynamicVariables["room.name"] = () => game.Room.RoomName;
    _dynamicVariables["room.temperature"] = () => game.Room.CurrentTemperature.ToString();
    _dynamicVariables["room.ambientTemperature"] = () => game.Room.AmbientRoomTemperature.ToString();
    
    _dynamicVariables["inventory.coins"] = () => game.Inventory.Coins.ToString();
    
    // dynamic variable random
    _dynamicVariables["random"] = () => new Random().Next().ToString();
  }

  // Initializes and sets a variable.
  public void SetVariable(string key, string value)
  {
    if (_readOnlyVariables.ContainsKey(key)) return;
    _variables[key] = value;

    Program.Log($"[DEBUG] Set variable \"{key}\" to \"{value}\"");
  }

  // Gets the variable. If the variable is not set, it returns the fallback value.
  public string? GetVariable(string key, string? fallback = null)
  {
    if (_variables.TryGetValue(key, out string? value))
    {
      Program.Log($"[DEBUG] Variable \"{key}\" found, returning value \"{value}\"");
      return value;
    }

    if (_readOnlyVariables.TryGetValue(key, out string? readOnlyValue)) return readOnlyValue;
    if (_dynamicVariables.TryGetValue(key, out Func<string>? dynamicValue)) return dynamicValue();
    
    // random.{min}.{max}
    Regex regex = RandomMinMax();
    Match match = regex.Match(key);
    if (match.Success)
    {
      int min = int.Parse(match.Groups[1].Value);
      int max = int.Parse(match.Groups[2].Value);
      string randomValue = new Random().Next(min, max).ToString();
      return randomValue;
    }
    
    Program.Log($"[DEBUG] Variable \"{key}\" not found, returning fallback value \"{fallback}\"");
    return fallback;
  }

  // Unsets the variable.
  public void UnsetVariable(string key)
  {
    _variables.Remove(key);

    Program.Log($"[DEBUG] Unset variable \"{key}\"");
  }

  // Adds to a numeric variable.
  public void VariableAdd(string key, int value)
  {
    if (!_variables.TryGetValue(key, out string? current)) return;
    if (int.TryParse(current, out int currentInt))
      _variables[key] = (currentInt + value).ToString();
    else if (double.TryParse(current, out double currentDouble))
      _variables[key] = (currentDouble + value).ToString();
    Program.Log($"[DEBUG] Added \"{value}\" to variable \"{key}\"");
  }

  // Subtracts from a numeric variable.
  public void VariableSubtract(string key, int value)
  {
    if (!_variables.TryGetValue(key, out string? current)) return;
    if (int.TryParse(current, out int currentInt))
      _variables[key] = (currentInt - value).ToString();
    else if (double.TryParse(current, out double currentDouble))
      _variables[key] = (currentDouble - value).ToString();
    Program.Log($"[DEBUG] Subtracted \"{value}\" from variable \"{key}\"");
  }

  // Lets you do calculations with numeric variables.
  public void VariableMath(string key, string expression)
  {
    if (!_variables.TryGetValue(key, out string? current)) return;
    if (!int.TryParse(current, out _) && !double.TryParse(current, out _)) return;

    object result = new DataTable().Compute(expression, null);
    _variables[key] = result.ToString();
    Program.Log($"[DEBUG] Calculated \"{expression}\" on variable \"{key}\" with result \"{result}\"");
  }

    [GeneratedRegex(@"random.(\d+).(\d+)")]
    private static partial Regex RandomMinMax();
}