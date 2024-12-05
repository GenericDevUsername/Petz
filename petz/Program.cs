using System.Diagnostics;
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
    // if --open-local-data is passed, open the local data folder
    if (args.Contains("--show-local-folder"))
    {
      Console.WriteLine(GameDataPath);
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey(true);
      return;
    }
    Console.OutputEncoding = Encoding.UTF8;
    AnsiConsole.Status()
    .Start("Loading game data...", ctx =>
    {
      ctx.Status("Loading game manager...");
      GameManager.Initialize();
      AnsiConsole.MarkupLine(@"[grey]LOG:[/] [white]Files Location:[/] " + GameDataPath);
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

    Renderer.Start(new MainMenu());
  }
}