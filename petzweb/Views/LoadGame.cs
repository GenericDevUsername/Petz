using petzweb.Models.Game;
using petzweb.ViewModel;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace petzweb.Views;

public class LoadGame : IView
{
  private readonly List<GameManager> _saveFiles = GameManager.GetSaves();
  private Layout? Layout { get; set; }
  private int SelectedOption { get; set; }
  public string ConsoleTitle { get; set; } = "PetzGame - Load Game";

  public void Initialize()
  {
      // empty
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
                        .SplitRows(
                            new Layout("LTop").Size(4),
                            new Layout("LBottom").MinimumSize(8)),
                    new Layout("Right").Size(30)
                        .SplitRows(
                            new Layout("BRTop").Size(4),
                            new Layout("BRBottom").MinimumSize(8)))
        );

    // (Console.WindowHeight - 11) / 6 is the number of save files that can be displayed on the screen before scrolling is needed
    // if under 0 then set to 0
    int scrollStart = (Console.WindowHeight - 11) / 5 - 1;
    scrollStart = scrollStart < 0 ? 0 : scrollStart;
    int skipAmount = SelectedOption < scrollStart ? 0 : SelectedOption - scrollStart;
    List<GameManager> saveFiles = _saveFiles.Skip(skipAmount).ToList();
    int selectedOptionDisplay = SelectedOption - skipAmount;

    IRenderable[] rows = new IRenderable[saveFiles.Count + 1];
    for (int i = 0; i < saveFiles.Count; i++)
    {
      GameManager saveFile = saveFiles[i];
      Table table = new();
      table.AddColumn(!saveFile.IsValid
          ? new TableColumn(new Markup("[red]Invalid Save[/]"))
          : new TableColumn(new Markup($"{saveFile.Data.Pet.Icon ?? "#"} {saveFile.Data.Pet.Name}")));
      table.AddColumn(
          saveFile.IsValid
              ? new TableColumn(new Markup($"[maroon]❤[/]  [red]{saveFile.Data?.Pet.Love.ToString() ?? "???"}[/]"))
              : new TableColumn(new Markup(saveFile.Data?.Pet?.Name ?? "???")));
      table.AddRow(
          new Markup(
              $"[blue]Last Played:[/] {saveFile.Data?.LastSaved.ToShortDateString() ?? "Unknown"} {saveFile.Data?.LastSaved.ToShortTimeString() ?? ""}"),
          new Markup($"{saveFile.Data?.Room.RoomName ?? "Unknown Room"}"));
      table.Border = TableBorder.Rounded;
      table.BorderStyle = selectedOptionDisplay == i
          ?
          saveFile.IsValid ? new Style(Color.Yellow) : new Style(Color.Red)
          : saveFile.IsValid
              ? new Style(Color.White)
              : new Style(Color.Grey);

      rows[i] = table.Expand();
    }

    rows[saveFiles.Count] =
        new Panel(Align.Center(new Markup(SelectedOption == _saveFiles.Count
            ? "[yellow]Create New Pet[/]"
            : "Create New Pet")))
        {
          Border = BoxBorder.Rounded,
          Width = 20,
          BorderStyle = SelectedOption == _saveFiles.Count ? new Style(Color.Yellow) : new Style(Color.White)
        };

    // Update the left column
    Panel panel = new(Align.Center(new Rows(rows).Expand()))
    {
      Header = new PanelHeader("Load Game")
    };
    Layout["LTop"].Update(new Panel(new Rows(
        new Markup(
            @"[gray]Press [black on silver]/\[/] and [black on silver]\/[/] to select. [green]<enter>[/] to confirm.[/]"),
        new Markup("[gray]Press [black on silver]<esc>[/] to return to the main menu.[/]"))).Expand());
    Layout["LBottom"].Update(panel.Expand());


    // Update the preview boxes
    if (SelectedOption < _saveFiles.Count)
    {
      Panel brTop = new(
          new Markup(_saveFiles[SelectedOption].Data.Pet.Icon).Centered()
      )
      {
        Header = new PanelHeader(_saveFiles[SelectedOption].Data.Pet.Name).Centered()
      };
      Layout["BRTop"].Update(brTop.Expand());
      Layout["BRBottom"].Update(new Panel(new Rows(
          new Markup(
              $"[red]Happiness:[/] {new PercentageBarComponent(_saveFiles[SelectedOption].Data.Pet.MaxHappiness, _saveFiles[SelectedOption].Data.Pet.Happiness, 22 - "Happiness: ".Length, Color.Red).Render()}"),
          new Markup(
              $"[yellow]Hunger:[/]    {new PercentageBarComponent(_saveFiles[SelectedOption].Data.Pet.MaxHunger, _saveFiles[SelectedOption].Data.Pet.Hunger, 22 - "Hunger:    ".Length, Color.Yellow).Render()}"),
          new Markup(
              $"[green]Health:[/]    {new PercentageBarComponent(_saveFiles[SelectedOption].Data.Pet.MaxHealth, _saveFiles[SelectedOption].Data.Pet.Health, 22 - "Health:    ".Length, Color.Green).Render()}"),
          new Markup($"[blue]In Room:[/] {_saveFiles[SelectedOption].Data.Room.RoomName ?? "???"}")
      ))
      {
        Header = new PanelHeader("Pet Info").Centered()
      }.Expand());
    }
    else
    {
      Layout["BRTop"].Update(new Panel(new Markup("???").Centered()).Expand());
      Layout["BRBottom"].Update(new Panel(new Markup("Create a new pet to start a new game")).Expand());
    }

    // Update the top column
    Layout["Top"].Update(
        new Panel(
                Align.Center(
                    new FigletText(font, "--- petzgame ---").Color(Color.Aqua),
                    VerticalAlignment.Middle))
            .Expand());

    // Render the layout
    Console.SetCursorPosition(0, 0);
    AnsiConsole.Write(Layout);
  }

  public void TakeInput(ConsoleKeyInfo key)
  {
    switch (key.Key)
    {
      case ConsoleKey.Escape:
        Renderer.ChangeView(new MainMenu());
        break;
      case ConsoleKey.W:
      case ConsoleKey.UpArrow:
        SelectedOption = SelectedOption == 0 ? _saveFiles.Count : SelectedOption - 1;
        break;
      case ConsoleKey.S:
      case ConsoleKey.DownArrow:
        SelectedOption = SelectedOption == _saveFiles.Count ? 0 : SelectedOption + 1;
        break;
      case ConsoleKey.Enter:
        if (SelectedOption == _saveFiles.Count)
        {
          Renderer.ChangeView(new NewGame());
        }
        else
        {
          Renderer.ChangeView(new GameMenuRoom(_saveFiles[SelectedOption]));
        }
        break;
    }
  }
}