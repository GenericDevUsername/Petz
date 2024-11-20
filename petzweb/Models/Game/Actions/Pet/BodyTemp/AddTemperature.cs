namespace petzweb.Models.Game.Actions.Pet.BodyTemp;

[SetAliases("addtemp", "tempadd", "temperatureadd")]
public class AddTemperature : ValueChangeAction
{
    public override void ChangeValue(GameData game, int amount)
    {
        game.Pet.BodyTemperature = ClampValue(game.Pet.BodyTemperature, amount, game.Pet.MaxBodyTemperature);
    }
}