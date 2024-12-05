namespace petz.Models;

public static class DefaultFiles
{
  public const string Items = """
                              paracetamol:
                                itemCategory: Medicine
                                shopPrice: 40
                                name: Paracetamol
                                icon: 💉
                                description: 50% Chance to heal sickness. +10 health, +5 hunger. Low quality Tescos own brand medicine.
                                maxStackSize: 1000
                                maxUses: 1
                                onUse:
                                  - addHealth{amount=10}
                                  - addHunger{amount=5}
                                  - cureIllness{chance=0.5}]
                              
                              cough_syrup:
                                itemCategory: Medicine
                                shopPrice: 85
                                name: Cough Syrup
                                icon: 🍯
                                description: 60% Chance to heal sickness. +15 health, +10 hunger. Soothes throat and reduces cough.
                                maxStackSize: 300
                                maxUses: 1
                                onUse:
                                  - addHealth{amount=15}
                                  - addHunger{amount=10}
                                  - cureIllness{chance=0.6}
                              
                              antibiotics:
                                itemCategory: Medicine
                                shopPrice: 100
                                name: Antibiotics
                                icon: 💊
                                description: 90% Chance to heal sickness. +20 health, +15 hunger. Strong medicine for serious infections.
                                maxStackSize: 200
                                maxUses: 1
                                onUse:
                                  - addHealth{amount=20}
                                  - addHunger{amount=15}
                                  - cureIllness{chance=0.9}
                              
                              ball:
                                itemCategory: Toys
                                shopPrice: 20
                                name: Ball
                                icon: 🔴
                                description: +5 Happiness. Guaranteed to entertain your pet for hours, unless it gets lost under the sofa first.
                                maxStackSize: 1000
                                maxUses: 3
                                onUse:
                                  - addHappiness{amount=5}
                              
                              chew_toy:
                                itemCategory: Toys
                                shopPrice: 20
                                name: Chew Toy
                                icon: 🦴
                                description: +10 Happiness. nom noms, 20% chance to make your pet hungry.
                                maxStackSize: 500
                                maxUses: 5
                                onUse:
                                  - addHappiness{amount=10}
                                  - addHunger{amount=-5; chance=0.2}
                              
                              laser_pointer:
                                itemCategory: Toys
                                shopPrice: 30
                                name: Laser Pointer
                                icon: 🔦
                                description: +15 Happiness. whats that dot?
                                maxStackSize: 200
                                maxUses: 10
                                onUse:
                                  - addHappiness{amount=15}
                              
                              chicken:
                                itemCategory: Food
                                shopPrice: 10
                                name: Chicken
                                icon: 🍗
                                description: +1-5 Hunger. Straight from KFPF (Kentucky Fried Pet Food)
                                maxStackSize: 99
                                maxUses: 1
                                onUse:
                                  - addHunger{amount=<random.1.5>}
                              
                              whole_chicken:
                                itemCategory: Food
                                shopPrice: 40
                                name: Whole Chicken
                                icon: 🍗
                                description: +1-5 Hunger. Bulk chicken order from KFPF, Buy 4 get 1 free!
                                maxStackSize: 99
                                maxUses: 5
                                onUse:
                                  - addHunger{amount=<random.1.5>}
                              
                              steak:
                                itemCategory: Food
                                shopPrice: 150
                                name: Steak
                                icon: 🥩
                                description: +Max Hunger. no over feeding here!
                                maxStackSize: 99
                                maxUses: 5
                                onUse:
                                  - addHunger{amount=<pet.hunger.max>}
                              """;
  public const string Pets = "";
  public const string Rooms = "";
}