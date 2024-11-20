using petzweb.Models.Inventory;
using petzweb.Models.Pet;
using petzweb.Models.Room;

namespace petzweb.Models.Game;

public class GameData
{
    public string GameId { get; internal set; } = Guid.NewGuid().ToString();
    public DateTime LastSaved = DateTime.Now;
    public GamePet Pet;
    public GameRoom Room;
    public GameInventory Inventory;
    
    public GameSave ToGameSave()
    {
        return new GameSave()
        {
            GameId = GameId,
            LastSaved = LastSaved,
            Pet = Pet.ToGameSavePet(),
            Room = Room.ToGameSaveRoom(),
            Inventory = Inventory.ToGameSaveInventory()
        };
    }
    
    public GameData? LoadGameSave(GameSave save)
    {
        GamePet? gamePet = save.Pet.ToGamePet();
        GameRoom? gameRoom = save.Room.ToGameRoom();
        GameInventory? gameInventory = save.Inventory.ToGameInventory(this);
        if (gamePet == null || gameRoom == null)
            return null;
        
        GameId = save.GameId;
        LastSaved = save.LastSaved;
        Pet = gamePet;
        Room = gameRoom;
        Inventory = gameInventory;
        return this;
    }
}