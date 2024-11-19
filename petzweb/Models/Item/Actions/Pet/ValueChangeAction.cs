namespace petzweb.Models.Actions;
public abstract class ValueChangeAction: IUseAction
{
    public virtual void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        if (!parameters.TryGetValue("amount", value: out var parameter)) return;
        if (!int.TryParse(parameter, out var amount)) return;
    }
    public abstract void ChangeValue(Pet pet, int amount);
    

    protected static int ClampValue(int currentValue, int amount, int? maxValue = null)
    {
        return maxValue.HasValue 
            ? Math.Clamp(currentValue + amount, 0, maxValue.Value) 
            : currentValue + amount;
    }
}
