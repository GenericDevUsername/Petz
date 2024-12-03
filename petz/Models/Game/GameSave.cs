using petz.Models.Inventory;
using petz.Models.Pet;
using petz.Models.Room;

namespace petz.Models.Game;

public class GameSave
{
  public GameSaveInventory? Inventory;
  public DateTime LastSaved = DateTime.Now;
  public GameSavePet? Pet;
  public GameSaveRoom? Room;
  public string GameId { get; internal set; } = Guid.NewGuid().ToString();

  public GameData? ToGameData()
  {
    return new GameData().LoadGameSave(this);
  }
}