namespace petzweb.Models.Pet;

public class RegisteredPet: PetData
{
    public RegisteredPet(string registeredId, PetData petData) : base(petData)
    {
        RegisteredId = registeredId;
    }

    public RegisteredPet(RegisteredPet pet) : base(pet)
    {
        RegisteredId = pet.RegisteredId;
    }

    public RegisteredPet()
    {
    }

    public string RegisteredId { get; }
}