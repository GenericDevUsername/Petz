using petzweb.Models.Pet;

namespace petzweb.Models.Game.Actions.Pet;

public abstract class ValueChangeAction : IUseAction
{
    public virtual void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (!parameters.TryGetValue("amount", out string? parameter)) return;
        if (!int.TryParse(parameter, out int amount)) return;
    }

    public abstract void ChangeValue(GameData game, int amount);


    protected static int ClampValue(int currentValue, int amount, int? maxValue = null)
    {
        return maxValue.HasValue
            ? Math.Clamp(currentValue + amount, 0, maxValue.Value)
            : currentValue + amount;
    }
}