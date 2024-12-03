using System.Text;
using petzweb.Models.Game;
using petzweb.ViewModel;
using Spectre.Console;
using Spectre.Console.Extensions;
using Spectre.Console.Rendering;

namespace petzweb.Views;

public class GameMenuShop(GameManager game) : IView
{
    private Layout? Layout { get; set; }
    private string[] Options { get; } = ["💾", "🏠", "🛒", "📦"];
    private GameManager Game { get; set; } = game;
    private Thread _updaterThread;
    private int SelectedOption { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - Game";

    public void Initialize()
    {
        _updaterThread = new Thread(() =>
        {
            do
            {
                Render();
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
                Header = new PanelHeader("Shop")
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
                    new Markup($"Health: {new PercentageBarComponent(Game.Data.Pet.MaxHealth, Game.Data.Pet.Health, 24 - "Health: ".Length, Color.Green).Render()}"),
                    new Markup($"Hunger: {new PercentageBarComponent(Game.Data.Pet.MaxHunger, Game.Data.Pet.Hunger, 24 - "Hunger: ".Length, Color.Yellow).Render()}"),
                    new Markup($"Happiness: {new PercentageBarComponent(Game.Data.Pet.MaxHappiness, Game.Data.Pet.Happiness, 24 - "Happiness: ".Length, Color.Red).Render()}"),
                    new Rule()
                )
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.White),
                Header = new PanelHeader("Pet Stats")
            }.Expand()
        );
        
        
        // add navigation panel
        Layout["LLBottom"].Update(
            new Panel(
                Align.Center(new Columns(
                    new Panel(new Markup("💾")){ Header = new PanelHeader("1") },
                    new Panel(new Markup("🏡")){ Header = new PanelHeader("2") },
                    new Panel(new Markup("🛒")){ Header = new PanelHeader("3"), BorderStyle = new Style(Color.Yellow) },
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
    }

    public void TakeInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                Renderer.ChangeView(new GameMenuSave(Game));
                break;
            
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Renderer.ChangeView(new GameMenuRoom(Game));
                break;
            
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                //Renderer.ChangeView(new GameMenuShop(Game));
                break;
            
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                Renderer.ChangeView(new GameMenuInventory(Game));
                break;
        }
    }
}