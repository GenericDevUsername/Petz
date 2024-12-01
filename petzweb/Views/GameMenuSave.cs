using petzweb.Models.Game;
using petzweb.ViewModel;
using Spectre.Console;
using Spectre.Console.Extensions;
using Spectre.Console.Rendering;

namespace petzweb.Views;

public class GameMenuSave(GameManager game) : IView
{
    private Layout? Layout { get; set; }
    private string[] Options { get; } = ["💾", "🏠", "🛒", "📦"];
    private GameManager Game { get; set; } = game;
    private int SelectedOption { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - Game";

    public void Initialize()
    {
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
                                new Layout("LR").Size(30)
                                    .SplitRows(
                                        new Layout("LRTop").MinimumSize(8),
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
                Renderer.ChangeView(new GameMenuRoom(Game));
                break;
            
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                Renderer.ChangeView(new GameMenuShop(Game));
                break;
            
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                Renderer.ChangeView(new GameMenuInventory(Game));
                break;
        }
    }
}