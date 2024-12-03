using petzweb.Models.Inventory;
using petzweb.Models.Pet;
using petzweb.Models.Room;

namespace petzweb.Models.Game;

public class GameData
{
  public GameInventory Inventory;
  private int _ticks = 0;
  public DateTime LastSaved = DateTime.Now;
  public GamePet Pet;
  public GameRoom Room;
  public bool Running = false;
  private Thread? _tickThread;
  public string GameId { get; private set; } = Guid.NewGuid().ToString();

  public GameSave ToGameSave()
  {
    return new GameSave
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
  
  public void Start()
  {
    if (Running)
      return;
    Running = true;
    _tickThread = new Thread(() =>
    {
      while (Running)
      {
        if (LastSaved + TimeSpan.FromMinutes(1) < DateTime.Now)
        {
          GameManager.Save(this);
          LastSaved = DateTime.Now;
        }

        Tick();
        // 20th of a second
        Thread.Sleep(1000);
      }
    })
    {
      IsBackground = true
    };
    _tickThread.Start();
  }
  
  public void Stop()
  {
    Running = false;
    _tickThread?.Join();
  }

  /// <summary>
  ///   Actions to be performed every tick
  /// </summary>
  private void Tick()
  {
    _ticks += 20;
    // Increase coins every second
    if (_ticks % 20 == 0) Inventory.ModifyCoins(1);

    // Increase pet's hunger every 5 seconds
    if (_ticks % 100 == 0) Pet.Hunger = Math.Max(0, Pet.Hunger - 1);
    if (Pet.Hunger == 0)
      // decrease health if hunger is 0 every second
      if (_ticks % 20 == 0) Pet.Health = Math.Max(0, Pet.Health - 1);
    
    // get room closer to AmbientRoomTemperature every second
    if (_ticks % 20 == 0) Room.IncrementTowardsAmbientTemperature(0.1f);
    
    if (Pet.Hunger == 0 || Room.CurrentTemperature > Pet.MaxBodyTemperature + 3 || Room.CurrentTemperature < Pet.MinBodyTemperature - 3)
    {
      // if any complaints are active take happiness every second
      if (_ticks % 20 == 0) Pet.Happiness = Math.Max(0, Pet.Happiness - 1);
    }
  }
}