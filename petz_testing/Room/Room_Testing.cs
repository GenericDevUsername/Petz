using petz.Models;
using petz.Models.Game;
using petz.Models.Pet;
namespace petz_testing.Room;

public class Room_Testing
{
  [Test]
  public void Test_RoomPropertiesLoadCorrectly()
  {
    RegisteredRoom room = TestData.Get_TestRoom();
    Console.WriteLine(room.RoomName);
    Console.WriteLine(room.RoomDescription);
    Console.WriteLine(room.AmbientRoomTemperature);

    Assert.Multiple(() =>
    {
      Assert.That(room.RoomName, Is.EqualTo("Test Room"));
      Assert.That(room.RoomDescription, Is.EqualTo("A room for testing purposes."));
      Assert.That(room.AmbientRoomTemperature, Is.EqualTo(20));
    });
  }

  [Test]
  [TestCaseSource(nameof(SourceFor_Test_ThatRoomCanBeIncrementedTowardsAmbientTemperature))]
  public void Test_ThatRoomCanBeIncrementedTowardsAmbientTemperature(float current, int ambient, int change)
  {
    GameData game = TestData.TestGame(TestData.Get_TestPet(), TestData.Get_TestRoom());
    game.Room.CurrentTemperature = current;
    game.Room.AmbientRoomTemperature = ambient;
    
    float currentTemperature = game.Room.CurrentTemperature;
    float currentDiff = game.Room.AmbientRoomTemperature - currentTemperature;
    if ( currentDiff < 0 ) currentDiff = -currentDiff;
    game.Room.IncrementTowardsAmbientTemperature(change);
    float newTemperature = game.Room.CurrentTemperature;
    float newDiff = game.Room.AmbientRoomTemperature - newTemperature;
    if ( newDiff < 0 ) newDiff = -newDiff;
    
    Console.WriteLine($"TEMP: {currentTemperature}°C -> {change} -> {newTemperature}°C");
    Console.WriteLine($"DIFF: {currentDiff} -> {newDiff}");
    
    Assert.Multiple(() =>
    {
      Assert.That(newDiff, Is.LessThan(currentDiff));
    });
  }
  
  private static IEnumerable<TestCaseData> SourceFor_Test_ThatRoomCanBeIncrementedTowardsAmbientTemperature()
  {
    Random random = new Random();
    for (int i = 0; i < 10; i++)
    {
      float current = (float)(random.NextDouble() * 40); // Random temperature between 0 and 40
      int ambient = random.Next(10, 30); // Random ambient temperature between 10 and 30
      int change = random.Next(1, 5); // Random change between 1 and 5
      yield return new TestCaseData(current, ambient, change).SetName($"test that {current}°C can be incremented towards {ambient}°C by {change}°C");
    }
  }
}