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

  public const string Pets = """
                             dog:
                               icon: "\U0001F436"
                               speciesName: Dog
                               description: The loyal companion.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 39
                               minBodyTemperature: 10
                               preferredTemperature: 22

                             wolf:
                               icon: "\U0001F43A"
                               speciesName: Wolf
                               description: The wild and majestic.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 39
                               minBodyTemperature: 5
                               preferredTemperature: 15

                             cat:
                               icon: "\U0001F431"
                               speciesName: Cat
                               description: The independent feline.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 40
                               minBodyTemperature: 10
                               preferredTemperature: 20

                             mouse:
                               icon: "\U0001F42D"
                               speciesName: Mouse
                               description: The small and nimble.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 37
                               minBodyTemperature: 15
                               preferredTemperature: 25

                             hamster:
                               icon: "\U0001F439"
                               speciesName: Hamster
                               description: The pocket-sized adventurer.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 36
                               minBodyTemperature: 15
                               preferredTemperature: 24

                             rabbit:
                               icon: "\U0001F430"
                               speciesName: Bunny
                               description: The fluffy and gentle. Dumb.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 38
                               minBodyTemperature: 10
                               preferredTemperature: 20

                             frog:
                               icon: "\U0001F438"
                               speciesName: Frog
                               description: The amphibious leaper.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 35
                               minBodyTemperature: 5
                               preferredTemperature: 18

                             tiger:
                               icon: "\U0001F42F"
                               speciesName: Tiger
                               description: The powerful predator.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 37
                               minBodyTemperature: 10
                               preferredTemperature: 25

                             koala:
                               icon: "\U0001F428"
                               speciesName: Koala
                               description: The sleepy eucalyptus lover.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 36
                               minBodyTemperature: 15
                               preferredTemperature: 20

                             bear:
                               icon: "\U0001F43B"
                               speciesName: Bear
                               description: The mighty forager.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 37
                               minBodyTemperature: 5
                               preferredTemperature: 15

                             pig:
                               icon: "\U0001F437"
                               speciesName: Pig
                               description: The intelligent and social.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 39
                               minBodyTemperature: 10
                               preferredTemperature: 22

                             penguin:
                               icon: "\U0001F427"
                               speciesName: Penguin
                               description: The dapper swimmer.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 38
                               minBodyTemperature: -10
                               preferredTemperature: 5

                             bird:
                               icon: "\U0001F426"
                               speciesName: Bird
                               description: The cheerful singer.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 40
                               minBodyTemperature: 15
                               preferredTemperature: 20

                             unicorn:
                               icon: "\U0001F984"
                               speciesName: Unicorn
                               description: The magical and rare.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 38
                               minBodyTemperature: 0
                               preferredTemperature: 15

                             raccoon:
                               icon: "\U0001F99D"
                               speciesName: Raccoon
                               description: The curious bandit.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 37
                               minBodyTemperature: 5
                               preferredTemperature: 18

                             fox:
                               icon: "\U0001F98A"
                               speciesName: Fox
                               description: The clever and cunning. Sly.
                               maxHunger: 100
                               maxHappiness: 100
                               maxLove: 100
                               maxHealth: 100
                               maxEnergy: 100
                               maxBodyTemperature: 37
                               minBodyTemperature: 10
                               preferredTemperature: 18
                             """;
  public const string Rooms = """
                              living_room:
                                ambientRoomTemperature: 20
                                roomName: Living Room
                                roomDescription: A cozy living room with a fireplace and a TV.
                              
                              kitchen:
                                ambientRoomTemperature: 22
                                roomName: Kitchen
                                roomDescription: ___
                              
                              bedroom:
                                ambientRoomTemperature: 18
                                roomName: Bedroom
                                roomDescription: ___
                              
                              bathroom:
                                ambientRoomTemperature: 24
                                roomName: Bathroom
                                roomDescription: ___
                              
                              office:
                                ambientRoomTemperature: 21
                                roomName: Office
                                roomDescription: ___
                              
                              dining_room:
                                ambientRoomTemperature: 20
                                roomName: Dining Room
                                roomDescription: ___
                              
                              guest_room:
                                ambientRoomTemperature: 19
                                roomName: Guest Room
                                roomDescription: ___
                              
                              garage:
                                ambientRoomTemperature: 17
                                roomName: Garage
                                roomDescription: ___
                              
                              basement:
                                ambientRoomTemperature: 16
                                roomName: Basement
                                roomDescription: ___
                              
                              attic:
                                ambientRoomTemperature: 15
                                roomName: Attic
                                roomDescription: ___
                              """;
}