using petz.Models;
using petz.Models.Game;
using petz.Models.Game.Actions.Pet.Health;
using petz.Models.Game.Actions.Pet.Hunger;
using petz.Models.Game.Actions.Pet.IsSick;
using petz.Models.Inventory;
using petz.Models.Item;
using petz.Models.Pet;
using petz.Models.Room;

namespace petz_testing.Item;

public class Item_Testing
{
  
  [Test]
  public void Test_ItemPropertiesLoadCorrectly()
  {
    ItemManager itemManager = new ItemManager();
    itemManager.LoadItems(TestData.TestItem);
    RegisteredItem item = itemManager.GetItem("paracetamol");

    Assert.Multiple(() =>
    {
        Assert.That(item.ItemCategory, Is.EqualTo("Medicine"));
        Assert.That(item.Name, Is.EqualTo("Paracetamol"));
        Assert.That(item.Icon, Is.EqualTo("💉"));
        Assert.That(item.Description, Is.EqualTo("Low quality Tescos own brand medicine. Might cure your pets sickness... or not."));
        Assert.That(item.MaxStackSize, Is.EqualTo(1000));
        Assert.That(item.MaxUses, Is.EqualTo(1));
    });
  }
  
  [Test]
  public void Test_ItemActionsLoadCorrectly()
  {
    ItemManager itemManager = new ItemManager();
    itemManager.LoadItems(TestData.TestItem);
    RegisteredItem item = itemManager.GetItem("paracetamol");

    Assert.That(item.OnUse, Has.Count.EqualTo(3));
    Assert.Multiple(() =>
    {
        Assert.That(item.OnUse[0], Is.TypeOf<AddHealth>());
        Assert.That(item.OnUse[1], Is.TypeOf<AddHunger>());
        Assert.That(item.OnUse[2], Is.TypeOf<CureIllness>());
    });
  }
  
  [Test]
  public void Test_ThatYouCanFeed_Medicate_AndPlayWithPet()
  {
    ItemManager itemManager = new ItemManager();
    PetManager petManager = new PetManager();
    RoomManager roomManager = new RoomManager();
    itemManager.LoadItems(TestData.TestItem);
    petManager.LoadPets(TestData.TestPet);
    roomManager.LoadRooms(TestData.TestRoom);
    RegisteredItem item = itemManager.GetItem("paracetamol");
    RegisteredPet pet = petManager.GetPet("robot");
    RegisteredRoom room = roomManager.GetRoom("test_room");
    
    GameData game = TestData.TestGame(pet, room);
    int currentHealth = game.Pet.Health;
    int currentHunger = game.Pet.Hunger;
    bool currentSickness = game.Pet.IsSick;
    item.UseItem(game);
    
    Assert.Multiple(() =>
    {
        Assert.That(game.Pet.Health, Is.EqualTo(currentHealth + 10));
        Assert.That(game.Pet.Hunger, Is.EqualTo(currentHunger + 5));
    });
  }
  
  
  
  
}