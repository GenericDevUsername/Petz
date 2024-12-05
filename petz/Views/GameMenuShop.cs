using System.Text;
using petz.Models.Game;
using petz.Models.Inventory;
using petz.Models.Item;
using petz.ViewModel;
using Spectre.Console;
using Spectre.Console.Extensions;
using Spectre.Console.Rendering;

namespace petz.Views;

public class GameMenuShop(GameManager game) : IView
{
    private Layout? Layout { get; set; }
    private List<RegisteredItem> ShopItems { get; set; } = [];
    private int _visibleRules = 0;
    private string[] Options { get; } = ["💾", "🏠", "🛒", "📦"];
    private GameManager Game { get; set; } = game;
    private Thread? _updaterThread;
    private bool _rendering = false;
    private int SelectedOption { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - Game";
    
    public bool SickAlerted { get; set; } = false;
    public List<Markup> ActionLog { get; set; } = [];
    
    public GameMenuShop(GameManager game, List<Markup> actionLog): this(game)
    {
        ActionLog = actionLog;
    }

    public void Initialize()
    {
        for (int i = 0; i < GameManager.Items.GetItems().Count; i++)
        {
            RegisteredItem item = GameManager.Items.GetItems()[i];
            if (item.ItemCategory == null) continue;
            ShopItems.Add(item);
        }
        ShopItems.Sort((a, b) => string.Compare(a.ItemCategory, b.ItemCategory, StringComparison.Ordinal));
        
        _updaterThread = new Thread(() =>
        {
            do
            {
                if (!_rendering) Render();
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
        // add menu panel
        int scrollStart = (Console.WindowHeight - 12) / 4 - 1;
        _visibleRules = 0;
        scrollStart = scrollStart < 0 ? 0 : scrollStart;
        int skipAmount = SelectedOption < scrollStart ? 0 : SelectedOption - scrollStart;
        List<RegisteredItem> scrollItems = ShopItems.Skip(skipAmount).ToList();
        string currentCategory = "";
        Layout["LLTop"].Update(
            new Panel(
                ShopItems.Count > 0 ? new Rows(scrollItems.Select(item =>
                {
                    List<IRenderable> renderables =
                    [
                        new Panel(new Rows(
                            new Markup($"[yellow]${item.ShopPrice ?? 0}[/] | {item.Icon} {item.Name} {(Game.Data.Inventory.Coins < item.ShopPrice ? "| [red]Can't Afford[/]" : "")}"),
                            new Markup($"[grey]{(item.Description.EscapeMarkup() == "" ? "No description" : item.Description.EscapeMarkup())}[/]")
                        ))
                        {
                            Border = BoxBorder.Rounded,
                            Width = Console.WindowWidth,
                            Height = 4,
                            BorderStyle = ShopItems[SelectedOption] == item
                                ? Game.Data.Inventory.Coins < item.ShopPrice ? new Style(Color. Red) : new Style(Color.Yellow)
                                : Game.Data.Inventory.Coins < item.ShopPrice ? new Style(Color.Grey) : new Style(Color.White)
                        }.Collapse()
                    ];
                    if (currentCategory == item.ItemCategory) return new Rows(renderables);
                    _visibleRules++;
                    currentCategory = item.ItemCategory ?? currentCategory;
                    renderables.Insert(0, new Rule($"[gray]{currentCategory}[/]"){ Style = new Style(Color.Grey) });
                    return  new Rows(renderables);
                })).Expand() : Align.Center(new Markup("No items in shop..."), VerticalAlignment.Middle)
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
                    new Markup($"Health: {new PercentageBarComponent(Game.Data.Pet.MaxHealth, Game.Data.Pet.Health, 26 - "Health: ".Length - 3, Color.Green).Render()}"),
                    new Markup($"Hunger: {new PercentageBarComponent(Game.Data.Pet.MaxHunger, Game.Data.Pet.Hunger, 26 - "Hunger: ".Length - 3, Color.Yellow).Render()}"),
                    new Markup($"Happiness: {new PercentageBarComponent(Game.Data.Pet.MaxHappiness, Game.Data.Pet.Happiness, 26 - "Happiness: ".Length - 3, Color.Red).Render()}"),
                    new Markup($"Coins: ${Game.Data.Inventory.Coins}"),
                    new Rule($"Controls"),
                    new Markup("[black on silver]-[/] [black on silver]+[/] - Change Temp"),
                    new Markup(@"[black on silver]/\[/] [black on silver]\/[/] - Select"),
                    new Markup("[black on silver]<enter>[/] - Purchase"),
                    new Rule(),
                    new Rows(ActionLog.TakeLast(Console.WindowHeight - 20))
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
        _rendering = false;
    }

    public void TakeInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                Renderer.ChangeView(new GameMenuSave(Game, ActionLog));
                break;
            
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Renderer.ChangeView(new GameMenuRoom(Game, ActionLog));
                break;
            
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                //Renderer.ChangeView(new GameMenuShop(Game));
                break;
            
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                Renderer.ChangeView(new GameMenuInventory(Game, ActionLog));
                break;
            
            case ConsoleKey.UpArrow:
                SelectedOption = SelectedOption - 1 == -1
                    ? (ShopItems.Count) - 1
                    : SelectedOption - 1;
                break;
            
            case ConsoleKey.DownArrow:
                SelectedOption = SelectedOption == (ShopItems.Count) - 1 ? 0 : SelectedOption + 1;
                break;
            
            case ConsoleKey.Enter:
                if (Game.Data.Inventory.Coins < ShopItems[SelectedOption].ShopPrice)
                {
                    ActionLog.Add(new Markup($"[red]![/] You can't afford {ShopItems[SelectedOption].Name}"));
                    break;
                }
                ActionLog.Add(new Markup($"[green]![/] Purchased {ShopItems[SelectedOption].Name}"));
                Game.Data.Inventory.ModifyCoins(-ShopItems[SelectedOption].ShopPrice ?? 0);
                Game.Data.Inventory.AddItem(ShopItems[SelectedOption], 1);
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