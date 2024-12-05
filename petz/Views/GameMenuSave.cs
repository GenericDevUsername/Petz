using System.Text;
using petz.Models.Game;
using petz.ViewModel;
using Spectre.Console;
using Spectre.Console.Extensions;
using Spectre.Console.Rendering;

namespace petz.Views;

public class GameMenuSave(GameManager game) : IView
{
    private Layout? Layout { get; set; }
    private string[] MenuOptions { get; } = ["Save and Exit to Main Menu", "Save and exit to Desktop", "Save Game"];
    private bool[] MenuOptionsEnabled { get; } = {true, true, true};
    private GameManager Game { get; set; } = game;
    private Thread _updaterThread;
    private bool _rendering = false;
    private int SelectedOption { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - Game";
    
    public List<Markup> ActionLog { get; set; } = [];
    public bool SickAlerted { get; set; } = false;
    
    public GameMenuSave(GameManager game, List<Markup> actionLog): this(game)
    {
        ActionLog = actionLog;
    }

    public void Initialize()
    {
        _updaterThread = new Thread(() =>
        {
            do
            {
                if(!_rendering) Render();
                Thread.Sleep(1000);
            } while (Renderer.CurrentView == this);
        })
        {
            IsBackground = true
        };
        _updaterThread.Start();
    }
    
    public void Render()
    {
        _rendering = true;
        switch (Game.Data.Pet.IsSick)
        {
            case true when !SickAlerted:
                ActionLog.Add(new Markup($"[red]![/] {Game.Data.Pet.Name} has fallen ill!"));
                SickAlerted = true;
                break;
            case false when SickAlerted:
                ActionLog.Add(new Markup($"[green]![/] {Game.Data.Pet.Name} has recovered!"));
                SickAlerted = false;
                break;
        }
        FigletFont font = FigletFont.Load(@"Assets\Font\logo.flf");
        // Create the layout
        Layout = new Layout("Root")
            .SplitRows(
                new Layout("Top").Size(5),
                new Layout("Bottom").MinimumSize(10)
                    .SplitColumns(
                        new Layout("Left")
                            .SplitColumns(
                                new Layout("LL")
                                    .SplitRows(
                                        new Layout("LLTop"),
                                        new Layout("LLBottom").MinimumSize(5).Size(5)
                                    ),
                                new Layout("LR").Size(31)
                                    .SplitRows(
                                        new Layout("LRRoomTemp").Size(3),
                                        new Layout("LRStats").MinimumSize(8),
                                        new Layout("LRBottom").MinimumSize(8).Invisible()
                                        )
                            )
                    )
            );
        
        // Update the top column
        Layout["Top"].Update(
            new Panel(
                    Align.Center(
                        new FigletText(font, "--- petzgame ---").Color(Color.Aqua),
                        VerticalAlignment.Middle))
                .Expand());
        
        // add menu panel
        Layout["LLTop"].Update(
            new Panel(
                ""
            )
            {
                Header = new PanelHeader("Escape Menu")
            }.Expand()
        );
        
        // generate temperature string 
        // example output: "*************|*************"
        // | = current temp always centered
        // * = room temp
        // *(green) = room temp within range of Game.Data?.Pet.MinBodyTemp and Game.Data?.Pet.MaxBodyTemp
        // *(yellow) = 3 padding on each side of the safe zone
        // *(red) = outside the safe zone
        StringBuilder tempBar = new StringBuilder();
        const int stringLength = 27;
        int startTemp = (int)Math.Round((Game.Data?.Room.CurrentTemperature ?? 0) - ((stringLength -1) / 2), 0);
        int endTemp = (int)Math.Round((Game.Data?.Room.CurrentTemperature ?? 0) + ((stringLength -1) / 2));
        for (int i = startTemp; i <= endTemp; i++)
        {
            string symbol = i == (int)Math.Round(Game.Data?.Room.CurrentTemperature ?? 0, 0) ? "|" : "*";
            if (i < Game.Data?.Pet.MinBodyTemperature || i > Game.Data?.Pet.MaxBodyTemperature)
            {
                tempBar.Append($"[red]{symbol}[/]");
            }
            else if (i < Game.Data?.Pet.MinBodyTemperature + 3 || i > Game.Data?.Pet.MaxBodyTemperature - 3)
            {
                tempBar.Append($"[yellow]{symbol}[/]");
            }
            else
            {
                tempBar.Append($"[green]{symbol}[/]");
            }
        }
            
        
        
        Layout["LRRoomTemp"].Update(
            new Panel(
                Align.Center(new Markup(tempBar.ToString()), VerticalAlignment.Middle)
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.White),
                Width = 20,
                Header = new PanelHeader($"Room Temp: {Math.Round(Game.Data?.Room.CurrentTemperature ?? 0, 1)}°C")
            }.Expand()
        );
        
        Layout["LRStats"].Update(
            new Panel(
                new Rows(
                    new Rule($"{Game.Data?.Pet.Name}'s Stats"),
                    new Markup($"Health: {new PercentageBarComponent(Game.Data.Pet.MaxHealth, Game.Data.Pet.Health, 26 - "Health: ".Length - 3, Color.Green).Render()}"),
                    new Markup($"Hunger: {new PercentageBarComponent(Game.Data.Pet.MaxHunger, Game.Data.Pet.Hunger, 26 - "Hunger: ".Length - 3, Color.Yellow).Render()}"),
                    new Markup($"Happiness: {new PercentageBarComponent(Game.Data.Pet.MaxHappiness, Game.Data.Pet.Happiness, 26 - "Happiness: ".Length - 3, Color.Red).Render()}"),
                    new Markup($"Coins: ${Game.Data.Inventory.Coins}"),
                    new Rule($"Controls"),
                    new Markup("[black on silver]-[/] [black on silver]+[/] - Change Temp"),
                    new Markup(@"[black on silver]/\[/] [black on silver]\/[/] - Select"),
                    new Markup("[black on silver]<enter>[/] - Confirm"),
                    new Rule(),
                    new Rows(ActionLog.TakeLast(Console.WindowHeight - 20))
                )
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.White),
            }.Expand()
        );
        
        IRenderable[] rows = new IRenderable[MenuOptions.Length];
        // pad name to width of 20
        for (int i = 0; i < MenuOptions.Length; i++)
            rows[i] = new Panel(Align.Center(
                new Markup(SelectedOption == i ? $"[yellow]{MenuOptions[i]}[/]" : MenuOptions[i])))
            {
                Border = BoxBorder.Rounded,
                Width = 20,
                BorderStyle = MenuOptionsEnabled[i] ? (SelectedOption == i ? new Style(Color.Yellow) : new Style(Color.White)) : (
                    SelectedOption == i ? new Style(Color.Yellow4) : new Style(Color.Grey))
            }.Expand();
        
        Layout["LLTop"].Update(
            new Panel(
                Align.Center(new Rows(rows).Expand(),
                    VerticalAlignment.Middle)
            ).Expand()
        );
        
        // add navigation panel
        Layout["LLBottom"].Update(
            new Panel(
                Align.Center(new Columns(
                    new Panel(new Markup("💾")){ Header = new PanelHeader("1"), BorderStyle = new Style(Color.Yellow) },
                    new Panel(new Markup("🏡")){ Header = new PanelHeader("2") },
                    new Panel(new Markup("🛒")){ Header = new PanelHeader("3") },
                    new Panel(new Markup("🧰")){ Header = new PanelHeader("4") }
                ).Collapse())
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.White),
                Width = 20
            }.Expand()
        );

        // Render the layout
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(Layout);
        _rendering = false;
    }

    public void TakeInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                //Renderer.ChangeView(new GameMenuSave(Game));
                break;
            
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Renderer.ChangeView(new GameMenuRoom(Game, ActionLog));
                break;
            
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                Renderer.ChangeView(new GameMenuShop(Game, ActionLog));
                break;
            
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                Renderer.ChangeView(new GameMenuInventory(Game, ActionLog));
                break;
            
            case ConsoleKey.UpArrow:
                SelectedOption = SelectedOption == 0 ? MenuOptions.Length - 1 : SelectedOption - 1;
                break;
            
            case ConsoleKey.DownArrow:
                SelectedOption = SelectedOption == MenuOptions.Length - 1 ? 0 : SelectedOption + 1;
                break;
            
            case ConsoleKey.Enter:
                if (!MenuOptionsEnabled[SelectedOption]) return;
                switch (SelectedOption)
                {
                    case 0:
                        Game.Data.Stop();
                        GameManager.Save(Game.Data);
                        Renderer.ChangeView(new MainMenu());
                        break;
                    case 1:
                        Game.Data.Stop();
                        GameManager.Save(Game.Data);
                        Renderer.Stop();
                        break;
                    case 2:
                        GameManager.Save(Game.Data);
                        MenuOptionsEnabled[2] = false;
                        break;
                }
                break;
            
            case ConsoleKey.OemMinus:
            case ConsoleKey.Subtract:
                Game.Data.Room.CurrentTemperature -= 0.5f;
                break;
            
            case ConsoleKey.OemPlus:
            case ConsoleKey.Add:
                Game.Data.Room.CurrentTemperature += 0.5f;
                break; 
        }
    }
}