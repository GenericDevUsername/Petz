using petz.Models.Pet;

namespace petz_testing.Pet;

public class Pet_testing
{
  
  [Test]
  public void Test_PetPropertiesLoadCorrectly()
  {
    RegisteredPet pet = TestData.Get_TestPet();

    Assert.Multiple(() =>
    {
        Assert.That(pet.SpeciesName, Is.EqualTo("Test Droid"));
        Assert.That(pet.Icon, Is.EqualTo("🤖"));
        Assert.That(pet.Description, Is.EqualTo("A test droid for testing purposes. It's not very good at much."));
        Assert.That(pet.MaxHunger, Is.EqualTo(100));
        Assert.That(pet.MaxHappiness, Is.EqualTo(100));
        Assert.That(pet.MaxLove, Is.EqualTo(100));
        Assert.That(pet.MaxHealth, Is.EqualTo(100));
        Assert.That(pet.MaxEnergy, Is.EqualTo(100));
        Assert.That(pet.MaxBodyTemperature, Is.EqualTo(40));
        Assert.That(pet.MinBodyTemperature, Is.EqualTo(10));
        Assert.That(pet.PreferredTemperature, Is.EqualTo(20));
    });
  }
}