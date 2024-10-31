namespace petzweb.Models
{
  public class Pet
  {
    /// PET INFO ///
    public string Name { get; set; }

    /// STATISTICS ///
    public DateTime DateCreated { get; internal set; }
    public int Hunger { get; internal set; }
    public int Happiness { get; internal set; }
    public int Love { get; internal set;}
    public int Health { get; internal set; }
    public int Energy { get; internal set; }
    public int BodyTemperature { get; internal set; }
    public bool IsSick { get; internal set; }
    
    /// Max Stats ///
    public int? MaxHunger { get; internal set; } = 100;
    public int? MaxHappiness { get; internal set; } = 100;
    public int? MaxLove { get; internal set; }
    public int? MaxHealth { get; internal set; } = 100;
    public int? MaxEnergy { get; internal set; } = 100;
    public int? MaxBodyTemperature { get; internal set; } = 100;

    /// PET BOUND INVENTORY ///
    public int Coins { get; internal set; }

    public Pet(string name)
    {
      Name = name;
      DateCreated = DateTime.Now;
      Hunger = 50;
      Happiness = 50; 
      Love = 50; 
      Health = 100; 
      Energy = 100; 
      BodyTemperature = 37; 
      IsSick = true; 
      
      Coins = 10;
    }
  }
}