using petzweb.Models.Game;
using petzweb.Models.Pet;
using petzweb.ViewModel;
using Spectre.Console;
using System.ComponentModel;

namespace petzweb.Views;

public class NewGame : IView
{
  private Layout? Layout { get; set; }
  public string ConsoleTitle { get; set; } = "PetzGame - New Game";
  
  private List<string> _name = [];
  private int _cursorIndex = 0;
  internal int _selectionIndex = 0;
  private static string _cursor = "_";
  private string? _selectedPet = null;
  // cursor flash
  private static Thread _cursorFlasher => new(() =>
  {
      // flash cursor when _selectionIndex == 0
      while (Renderer.CurrentView is NewGame)
      {

      } 

  });

  public void Render()
  {
    if (!_cursorFlasher.IsAlive) { 
      //Console.WriteLine("Starting cursor flasher");
    }
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
                            new Layout("LBottom").MinimumSize(8)
                                .SplitColumns(
                                    new Layout("LL"),
                                    new Layout("LR").Size(30)
                                )
                        )
                )
        );
    if (_selectedPet == null) Layout["LR"].Invisible();

    // Update the top column
    Layout["Top"].Update(
        new Panel(
                Align.Center(
                    new FigletText(font, "--- petzgame ---").Color(Color.Aqua),
                    VerticalAlignment.Middle))
            .Expand());

    // Controls Panel
    Layout["LTop"].Update(new Panel(new Rows(
        new Markup(
            @"[gray]Press [black on silver]/\[/] and [black on silver]\/[/] to select. [green]<enter>[/] to confirm.[/]"),
        new Markup("[gray]Press [black on silver]<esc>[/] to return to the main menu.[/]"))).Expand());

    List<RegisteredPet> pets = GameManager.PetManager.GetPets();
    Markup name;
    try
    {
      name = new Markup(_name.Count == 0 ? "_" : string.Join("", _name.Take(_cursorIndex)) + _cursor + string.Join("", _name.Skip(_cursorIndex)));
    } catch
    {
      name = new Markup(string.Join("", _name.Take(_cursorIndex)).EscapeMarkup() + _cursor + string.Join("", _name.Skip(_cursorIndex)).EscapeMarkup());
    }
    Layout["LL"].Update(
      new Panel(
        new Rows(
          new Rule("Enter Name").LeftJustified(),
          new Panel(name){ Height = 3, BorderStyle = _selectionIndex == 0 ? new Style(Color.Yellow) : new Style(Color.White) }.Expand(),
          new Rule("Select Pet").LeftJustified(),
          new Columns(pets.Select(pet => new Panel(new Markup(pet.Icon))
          {
            BorderStyle = _selectedPet == pet.RegisteredId ? new Style(Color.Yellow) : new Style(Color.White),
          })).Collapse()
        )
      )
      { Header = new PanelHeader("New Pet") }.Expand());

    // Render the layout
    Console.SetCursorPosition(0, 0);
    AnsiConsole.Write(Layout);
  }

  public void TakeInput(ConsoleKeyInfo key)
  {
    List<RegisteredPet> pets = GameManager.PetManager.GetPets();

    int petsPerRow = (Console.WindowWidth - 34) / 7;
    // edge case for padding merging with last pet (7 is pet plus space, but if padding aligns with last pet, it will be 6)
    if ((petsPerRow + 1) * 7 - 1 == Console.WindowWidth - 34) petsPerRow++;

    IEnumerable<string[]> uiArray = pets.Select(pet => pet.RegisteredId).ToList().Chunk(petsPerRow);
    int selectedPetIndex;

    switch (key.Key)
    {
      case ConsoleKey.Escape:
        Renderer.ChangeView(new MainMenu());
        break;
      case ConsoleKey.DownArrow:
        if (_selectionIndex == 0) { 
          _selectionIndex = 1;
          _selectedPet = pets[0].RegisteredId;
        }
        else
        {
          _selectedPet ??= pets[0].RegisteredId;
          selectedPetIndex = uiArray.SelectMany(x => x).ToList().IndexOf(_selectedPet);


          if (selectedPetIndex + petsPerRow < pets.Count)
          {
            _selectedPet = uiArray.SelectMany(x => x).ToList()[selectedPetIndex + petsPerRow] ?? pets[pets.Count - 1].RegisteredId;
          }
        }
        break;
      case ConsoleKey.UpArrow:
        if (_selectionIndex == 0) { break; };
        _selectedPet ??= pets[0].RegisteredId;
        selectedPetIndex = uiArray.SelectMany(x => x).ToList().IndexOf(_selectedPet);

        if (selectedPetIndex - petsPerRow < 0)
        {
          _selectionIndex = 0;
          _selectedPet = null;
          break;
        }
        _selectedPet = uiArray.SelectMany(x => x).ToList()[selectedPetIndex - petsPerRow];
        break;
      case ConsoleKey.LeftArrow:
        if (_selectionIndex == 0 && _cursorIndex > 0)
        {
          _cursorIndex--;
        }
        else
        {
          _selectedPet ??= pets[0].RegisteredId;
          selectedPetIndex = uiArray.SelectMany(x => x).ToList().IndexOf(_selectedPet);

          if (selectedPetIndex == 0)
          {
            _selectionIndex = 0;
            _selectedPet = null;
            break;
          }
          _selectedPet = uiArray.SelectMany(x => x).ToList()[selectedPetIndex - 1];
        }
        break;
      case ConsoleKey.RightArrow:
        if (_selectionIndex == 0 && _cursorIndex < _name.Count)
        {
          _cursorIndex++;
        }
        else
        {
          _selectedPet ??= pets[0].RegisteredId;
          selectedPetIndex = uiArray.SelectMany(x => x).ToList().IndexOf(_selectedPet);

          if (selectedPetIndex == uiArray.SelectMany(x => x).ToList().Count - 1) break;
          _selectedPet = uiArray.SelectMany(x => x).ToList()[selectedPetIndex + 1];
        }
        break;
      case ConsoleKey.Backspace:
        if (_selectionIndex > 0) break;
        if (_cursorIndex > 0)
        {
          _name.RemoveAt(_cursorIndex - 1);
          _cursorIndex--;
        }
        break;
      default:
        if (_selectionIndex > 0) break;
        if (!char.IsLetterOrDigit(key.KeyChar) && !char.IsSymbol(key.KeyChar) && !char.IsPunctuation(key.KeyChar) && key.KeyChar != ' ') break;
        _name.Insert(_cursorIndex, key.KeyChar.ToString());
        _cursorIndex++;
        break;
    }
  }
}