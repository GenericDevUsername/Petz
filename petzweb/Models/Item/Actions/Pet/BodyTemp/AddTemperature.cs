namespace petzweb.Models.Actions;

[SetAliases("addtemp", "tempadd", "temperatureadd")]
public class AddTemperature : ValueChangeAction
{
    public override void ChangeValue(Pet pet, int amount)
    {
        pet.BodyTemperature = ClampValue(pet.BodyTemperature, amount, pet.MaxBodyTemperature);
    }
}