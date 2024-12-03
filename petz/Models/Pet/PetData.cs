namespace petz.Models.Pet;

public class PetData
{
  public PetData(PetData petData)
  {
    Icon = petData.Icon;
    SpeciesName = petData.SpeciesName;
    Description = petData.Description;
    MaxHunger = petData.MaxHunger;
    MaxHappiness = petData.MaxHappiness;
    MaxLove = petData.MaxLove;
    MaxHealth = petData.MaxHealth;
    MaxEnergy = petData.MaxEnergy;
    MaxBodyTemperature = petData.MaxBodyTemperature;
    PreferredTemperature = petData.PreferredTemperature;
  }

  public PetData()
  {
  }

  public string Icon { get; internal set; } = "\ud83e\udd8a";
  public string SpeciesName { get; internal set; }
  public string Description { get; internal set; }

  /// Max Stats ///
  public int MaxHunger { get; internal set; }

  public int MaxHappiness { get; internal set; }
  public int MaxLove { get; internal set; }
  public int MaxHealth { get; internal set; }
  public int MaxEnergy { get; internal set; }
  public int MaxBodyTemperature { get; internal set; }
  public int MinBodyTemperature { get; internal set; }
  public int PreferredTemperature { get; internal set; }
}