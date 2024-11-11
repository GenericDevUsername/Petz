namespace petzweb.Models.Actions;

public class MakeSick : IUseAction
{
    public void Execute(Pet pet, Dictionary<string, string> parameters, VariableStore variableStore)
    {
        // Cure the pet's illness by setting the pet's illness to false
        pet.IsSick = true;
    }
}
