using petz.Models;
using petz.Models.Game;
using petz.Models.Inventory;
using petz.Models.Item;
using petz.Models.Pet;
using petz.Models.Room;

namespace petz_testing;

public class TestData
{
  public const string TestItem = """
                                 paracetamol:
                                   itemCategory: Medicine
                                   name: Paracetamol
                                   icon: 💉
                                   description: Low quality Tescos own brand medicine. Might cure your pets sickness... or not.
                                   maxStackSize: 1000
                                   maxUses: 1
                                   onUse:
                                     - addHealth{amount=10}
                                     - addHunger{amount=5}
                                     - cureIllness{chance=0.5}]
                                 """;

  public static readonly string InvalidItem = """
                                              paracetasmol:
                                                itemCategory: Medicine
                                                name: Paracetamol
                                                icon: 💉
                                                description: Low quality Tescos own brand medicine. Might cure your pets sickness... or not.
                                                maxStackSize: 1000
                                                maxUses: 1
                                                onUse:
                                                  - addHealth{amount=10}
                                                  - addHunger{5}
                                                  - cureIllness{chance=0.5}]
                                                  
                                              """;

  public const string TestPet = """
                                robot:
                                  icon: "🤖"
                                  speciesName: Test Droid
                                  description: A test droid for testing purposes. It's not very good at much.
                                  maxHunger: 100
                                  maxHappiness: 100
                                  maxLove: 100
                                  maxHealth: 100
                                  maxEnergy: 100
                                  maxBodyTemperature: 40
                                  minBodyTemperature: 10
                                  preferredTemperature: 20
                                """;
  
  public const string TestRoom = """
                                 test_room:
                                   roomName: Test Room
                                   roomDescription: A room for testing purposes.
                                   ambientRoomTemperature: 20
                                 """;

  public static GameData TestGame(RegisteredPet pet, RegisteredRoom room)
  { 
    GameData game = new GameData()
    {
      Pet = new GamePet(pet)
      {
        Name = "Test Pet",
        Love = 0,
        Happiness = pet.MaxHappiness/2,
        Hunger = pet.MaxHunger/2,
        Health = pet.MaxHealth/2,
        Energy = pet.MaxEnergy/2,
        BodyTemperature = pet.PreferredTemperature,
        IsSick = true,
      },
      Room = new GameRoom(room, pet.PreferredTemperature),
      LastSaved = DateTime.Now,
    }; 
    game.Inventory = new GameInventory(game);

    return game;
  }
  
  public static RegisteredRoom Get_TestRoom()
  {
    RoomManager roomManager = new RoomManager();
    roomManager.LoadRooms(TestData.TestRoom);
    RegisteredRoom room = roomManager.GetRoom("test_room");
    return room;
  }
  
  public static RegisteredPet Get_TestPet()
  {
    PetManager petManager = new PetManager();
    petManager.LoadPets(TestData.TestPet);
    RegisteredPet pet = petManager.GetPet("robot");
    return pet;
  }
  
  public static RegisteredItem Get_TestItem()
  {
    ItemManager itemManager = new ItemManager();
    itemManager.LoadItems(TestData.TestItem);
    RegisteredItem item = itemManager.GetItem("paracetamol");
    return item;
  }
}