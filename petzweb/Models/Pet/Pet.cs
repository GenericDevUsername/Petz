using petzweb.Models.Room;

namespace petzweb.Models;
public class Pet
{
    /// PET INFO ///
    public required string Name { get; set; }
    /// Max Stats ///
    public int MaxHunger { get; internal set; } = 100;
    public int MaxHappiness { get; internal set; } = 100;
    public int MaxLove { get; internal set; }
    public int MaxHealth { get; internal set; } = 100;
    public int MaxEnergy { get; internal set; } = 100;
    public int MaxBodyTemperature { get; internal set; } = 100;
    public int PreferredTemperature { get; internal set; } = 20;

    /// STATISTICS ///
    public DateTime DateCreated { get; internal set; }
    public int Hunger { get; internal set; }
    public int Happiness { get; internal set; }
    public int Love { get; internal set; }
    public int Health { get; internal set; }
    public int Energy { get; internal set; }
    public int BodyTemperature { get; internal set; }
    public bool IsSick { get; internal set; }
    public PetRoom CurrentRoom { get; internal set; }


    /// PET BOUND INVENTORY ///
    public int Coins { get; internal set; }

    public Pet(RegisteredRoom room)
    {
        CurrentRoom = new PetRoom(room);
        Hunger = MaxHunger;
        Happiness = MaxHappiness;
        Love = 0;
        Health = MaxHealth;
        Energy = 3;
        BodyTemperature = PreferredTemperature;
    }
}