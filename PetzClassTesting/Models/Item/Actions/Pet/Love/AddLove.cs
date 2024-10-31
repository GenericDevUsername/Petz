namespace petzweb.Models.Actions;

public class AddLove : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Love = ClampValue(pet.Love, amount, pet.MaxLove);
    }
}
