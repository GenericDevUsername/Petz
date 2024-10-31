namespace petzweb.Models.Actions;

public class AddEnergy : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Energy = ClampValue(pet.Energy, amount);
    }
}
