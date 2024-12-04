using petz.Models.Game;

namespace petz.Models.Pet;

public class GameSavePet
{
  public GameSavePet(GamePet pet)
  {
    RegisteredPetId = pet.RegisteredId;
    Name = pet.Name;
    DateCreated = pet.DateCreated;
    Hunger = pet.Hunger;
    Happiness = pet.Happiness;
    Love = pet.Love;
    Health = pet.Health;
    Energy = pet.Energy;
    BodyTemperature = pet.BodyTemperature;
    IsSick = pet.IsSick;
  }

  public GameSavePet()
  {
  }

  public string RegisteredPetId { get; set; }
  public string Name { get; set; }
  public DateTime DateCreated { get; set; }
  public int Hunger { get; set; }
  public int Happiness { get; set; }
  public int Love { get; set; }
  public int Health { get; set; }
  public int Energy { get; set; }
  public int BodyTemperature { get; set; }
  public bool IsSick { get; set; }


  public GamePet? ToGamePet()
  {
    try
    {
      RegisteredPet petData = GameManager.Pets.GetPet(RegisteredPetId);
      return new GamePet(this, petData);
    }
    catch (Exception)
    {
      return null;
    }
  }
}