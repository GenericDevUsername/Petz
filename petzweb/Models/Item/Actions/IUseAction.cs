namespace petzweb.Models;

public interface IUseAction
{
    void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore);
}