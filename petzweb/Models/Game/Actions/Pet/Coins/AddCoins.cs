using petzweb.Models.Pet;

namespace petzweb.Models.Game.Actions.Pet.Coins;

public class AddCoins : ValueChangeAction
{
    public override void ChangeValue(GameData game, int amount)
    {
        game.Inventory.ModifyCoins(amount);
    }
}