namespace petzweb.Models.Actions;

[SetAliases("addHunger")]
public class AddHunger : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Hunger = ClampValue(pet.Hunger, amount, pet.MaxHunger);
    }
}
