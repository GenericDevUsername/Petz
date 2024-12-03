namespace petz.Models.Pet;

public class GamePet : RegisteredPet
{
  public GamePet(RegisteredPet registeredPet) : base(registeredPet)
  {
  }

  public GamePet(GameSavePet pet, RegisteredPet registeredPet) : base(registeredPet)
  {
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

  /// PET INFO ///
  public string Name { get; set; }

  /// STATISTICS ///
  public DateTime DateCreated { get; internal set; }

  public int Hunger { get; internal set; }
  public int Happiness { get; internal set; }
  public int Love { get; internal set; }
  public int Health { get; internal set; }
  public int Energy { get; internal set; }
  public int BodyTemperature { get; internal set; }
  public bool IsSick { get; internal set; }

  public GameSavePet ToGameSavePet()
  {
    return new GameSavePet(this);
  }
}