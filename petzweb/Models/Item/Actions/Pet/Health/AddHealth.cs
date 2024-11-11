namespace petzweb.Models.Actions;

public class AddHealth : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Health = ClampValue(pet.Health, amount, pet.MaxHealth);
    }
}
