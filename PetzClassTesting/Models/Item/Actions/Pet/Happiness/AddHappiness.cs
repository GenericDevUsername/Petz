namespace petzweb.Models.Actions;

public class AddHappiness : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Happiness = ClampValue(pet.Happiness, amount, pet.MaxHappiness);
    }
}