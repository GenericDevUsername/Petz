namespace petzweb.Models.Game.Actions.Pet.Love;

public class AddLove : ValueChangeAction
{
    public override void ChangeValue(GameData game, int amount)
    {
        game.Pet.Love = ClampValue(game.Pet.Love, amount, game.Pet.MaxLove);
    }
}