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

  public int Hunger { get; set; }
  public int Happiness { get; set; }
  public int Love { get; set; }
  public int Health { get; set; }
  public int Energy { get; set; }
  public int BodyTemperature { get; set; }
  public bool IsSick { get; set; }

  /// <summary>
  ///  Convert the game pet to a game save pet
  /// </summary>
  /// <returns> The game save pet </returns>
  public GameSavePet ToGameSavePet()
  {
    return new GameSavePet(this);
  }
}