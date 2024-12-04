using System.Text;
using petz.Models.Game;
using petz.Models.Game.Actions;
using petz.ViewModel;
using petz.Views;
using Spectre.Console;

namespace petz;

internal abstract class Program
{
  public static readonly string GameDataPath =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) +
      "/.petzgame";
  public static readonly DateTime StartTime = DateTime.Now;

  public static string GameSavesPath => GameDataPath + "/saves";
  public static string LogPath => GameDataPath + "/logs";
  public bool Initialised { get; private set; } = false;
  
  
  public static void Log(string message)
  {
    if (!Directory.Exists(LogPath))
    {
      Directory.CreateDirectory(LogPath);
    }
    File.AppendAllText(LogPath + $"\\{DateTime.Now:yyyy-MM-dd}.log", $"[{TimeSpan.FromTicks(DateTime.Now.Ticks - StartTime.Ticks):hh\\:mm\\:ss}] {message}\n");
  }

  private static void Main(string[] args)
  {
    Console.OutputEncoding = Encoding.UTF8;
    // Asynchronous
    AnsiConsole.Status()
    .Start("Loading game data...", ctx =>
    {
      // Omitted
      ctx.Status("Loading game manager...");
      GameManager.Initialize();
      AnsiConsole.MarkupLine(@$"[grey]LOG:[/] [white]Loaded[/] [green]{GameManager.Pets.GetPets().Count}[/] [white]pets[/]");
      AnsiConsole.MarkupLine(@$"[grey]LOG:[/] [white]Loaded[/] [green]{GameManager.Rooms.GetRooms().Count}[/] [white]rooms[/]");
      AnsiConsole.MarkupLine(@$"[grey]LOG:[/] [white]Loaded[/] [green]{GameManager.Items.GetItems().Count}[/] [white]items[/]");
      
      ctx.Status("Loading action registry...");
      ActionRegistry.Initialize();
      AnsiConsole.MarkupLine(@$"[grey]LOG:[/] [white]Loaded[/] [green]{ActionRegistry.Count}[/] [white]actions[/]");
      AnsiConsole.MarkupLine(@$"[grey]LOG:[/] [white]{string.Join(", ", ActionRegistry.Keys).EscapeMarkup()}[/]");
      
      ctx.Status("Finishing up...");
      AnsiConsole.MarkupLine(@"[grey]LOG:[/] [white]Initialising renderer...[/]");
    });

    

    /*RegisteredRoom? room = GameManager.RoomManager.GetRoom("living_room");
    int i = 0;
    while (i < 10)
    {
      Random random = new Random();
      GameManager saveToMake = new GameManager()
      {
         Data = new GameData()
         {
            Pet = new GamePet(GameManager.PetManager.GetPet("dog"))
            {
               Name = random.Next(0, 100).ToString(),
               Love = random.Next(0, 14),
                    Happiness = random.Next(1, 100),
                    Hunger = random.Next(1, 100),
                    Health = random.Next(1, 100),
                    Energy = random.Next(1, 100),
                    BodyTemperature = random.Next(1, 100),
                    IsSick = random.Next(0, 1) == 1
                },
                Room = new GameRoom(room)
            }
        };
        saveToMake.Data.Inventory = new GameInventory(saveToMake.Data);
        saveToMake.Save();
      i++;
    }*/

    /*Dictionary<string, RoomData> rooms = new();
    rooms.Add("living_room", new RoomData(){RoomName = "Living Room", RoomDescription = "A cozy living room with a fireplace and a TV.", AmbientRoomTemperature = 20});
    rooms.Add("kitchen", new RoomData(){RoomName = "Kitchen", RoomDescription = "A kitchen with a fridge and a stove.", AmbientRoomTemperature = 20});
    rooms.Add("bedroom", new RoomData(){RoomName = "Bedroom", RoomDescription = "A bedroom with a bed and a closet.", AmbientRoomTemperature = 20});
    rooms.Add("bathroom", new RoomData(){RoomName = "Bathroom", RoomDescription = "A bathroom with a shower and a toilet.", AmbientRoomTemperature = 20});
    rooms.Add("microwave", new RoomData(){RoomName = "Microwave", RoomDescription = "A microwave.", AmbientRoomTemperature = 1000});
    GameManager.RoomManager.SaveRooms(rooms);*/

    /*Dictionary<string, PetData> pets = new();
    pets.Add("dog", new PetData(){SpeciesName = "Dog", Icon = "🐶", PreferredTemperature = 20, MaxBodyTemperature = 40, MinBodyTemperature = 10, MaxHappiness = 100, MaxHealth = 100, MaxHunger = 100, MaxEnergy = 100, MaxLove = 100,
        Description = "The trusty companion of humans. Dogs are loyal and loving animals that require a lot of attention and care."});
    pets.Add("cat", new PetData(){SpeciesName = "Cat", Icon = "🐱", PreferredTemperature = 20, MaxBodyTemperature = 40, MinBodyTemperature = 10, MaxHappiness = 100, MaxHealth = 100, MaxHunger = 100, MaxEnergy = 100, MaxLove = 100,
        Description = "The independent feline. Cats are known for their playful and curious nature. They require a lot of attention and care."});
    pets.Add("fish", new PetData(){SpeciesName = "Fish", Icon = "🐟", PreferredTemperature = 20, MaxBodyTemperature = 40, MinBodyTemperature = 10, MaxHappiness = 100, MaxHealth = 100, MaxHunger = 100, MaxEnergy = 100, MaxLove = 100,
        Description = "The aquatic pet. Fish are low maintenance pets that require a clean tank and regular feeding."});
    pets.Add("bird", new PetData(){SpeciesName = "Bird", Icon = "🐦", PreferredTemperature = 20, MaxBodyTemperature = 40, MinBodyTemperature = 10, MaxHappiness = 100, MaxHealth = 100, MaxHunger = 100, MaxEnergy = 100, MaxLove = 100,
        Description = "The chirpy companion. Birds are social animals that require a lot of attention and care."});
    GameManager.PetManager.SavePets(pets);*/


    Renderer.Start(new MainMenu());
  }
}