namespace petzweb.Models.Actions;

public class AddCoins : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.Coins = ClampValue(pet.Coins, amount);
    }
}