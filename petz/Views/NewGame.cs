using Spectre.Console;
using System.ComponentModel;
using petz.Models.Game;
using petz.Models.Pet;
using petz.ViewModel;

namespace petz.Views;

public class NewGame : IView
{
  private Layout? Layout { get; set; }
  public string ConsoleTitle { get; set; } = "PetzGame - New Game";
  
  private readonly List<string> _name = [];
  private int _cursorIndex = 0;
  private static int _selectionIndex = 0;
  private static string _cursor = "_";
  private string? _selectedPet = null;
  private string? _confirmedPet = null;
  // cursor flash
  private readonly Thread _cursorFlasher;
  private void CursorFunction()
  {
      do
      {
        if (_selectionIndex != 0) continue;
        _cursor = _cursor == "_" ? " " : "_";
        Renderer.CurrentView?.Render();
        Thread.Sleep(500);
      } while (Renderer.CurrentView == this);
      _cursor = "_";
  }
  
  public NewGame()
  {
    _cursorFlasher = new Thread(CursorFunction)
    {
      IsBackground = true
    };
  }
  
  public void Initialize()
  {
    if (_cursorFlasher.ThreadState != ThreadState.Running) _cursorFlasher.Start();
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
                            new Layout("LBottom").MinimumSize(8)
                                .SplitColumns(
                                    new Layout("LL"),
                                    new Layout("LR").Size(30)
                                )
                        )
                )
        );
    if (_selectedPet == null && _confirmedPet == null) Layout["LR"].Invisible();

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
          _selectionIndex switch
          {
            0 => @"[gray]Press [black on silver]/\[/] and [black on silver]\/[/] to select. [black on silver]<[/] and [black on silver]>[/] to move cursor. Keyboard to type. [green]<enter>[/] to confirm.[/]",
            1 => @"[gray]Press [black on silver]/\[/], [black on silver]\/[/], [black on silver]<[/] and [black on silver]>[/] to select. [green]<enter>[/] to confirm.[/]",
            2 => "[gray]Press [green]<enter>[/] to confirm.[/]"
          }
                
        ),
        new Markup("[gray]Press [black on silver]<esc>[/] to return to the main menu.[/]"))).Expand());

    List<RegisteredPet> pets = GameManager.Pets.GetPets();
    Markup name;
    try
    {
      name = new Markup(_name.Count == 0 ? _cursor : string.Join("", _name.Take(_cursorIndex)) + (_selectionIndex == 0 ? _cursor : "") + string.Join("", _name.Skip(_cursorIndex)));
    } catch
    {
      name = new Markup(string.Join("", _name.Take(_cursorIndex)).EscapeMarkup() + (_selectionIndex == 0 ? _cursor : "") + string.Join("", _name.Skip(_cursorIndex)).EscapeMarkup());
    }

    Layout["LL"].Update(
      new Panel(
        new Rows(
          new Rule(_name.Count > 0 ? "Enter Name" : "[red]*[/] Enter Name").LeftJustified(),
          new Panel(name)
              { Height = 3, BorderStyle = _selectionIndex == 0 ? new Style(Color.Yellow) : new Style(Color.White) }
            .Expand(),
          new Rule(_confirmedPet != null ? "Select Pet" : "[red]*[/] Select Pet").LeftJustified(),
          new Columns(pets.Select(pet => new Panel(new Markup(pet.Icon))
          {
            BorderStyle = _confirmedPet == pet.RegisteredId ? new Style(Color.PaleGreen1) :
              _selectedPet == pet.RegisteredId ? new Style(Color.Yellow) : new Style(Color.White),
          })).Collapse(),
          new Rule(),
          new Panel(new Markup("Create").Centered())
          {
            Height = 3,
            BorderStyle = _confirmedPet != null && _name.Count > 0 ? 
              _selectionIndex == 2 ? new Style(Color.Yellow) : new Style(Color.White) 
              : _selectionIndex == 2 ? new Style(Color.Red) : new Style(Color.Grey)
          }.Expand()
        )
      )
      { Header = new PanelHeader("New Pet") }.Expand());

    RegisteredPet? selectedPet = pets.Find(pet => pet.RegisteredId == (_selectionIndex == 1 ? _selectedPet : (_confirmedPet ?? _selectedPet)));
    Layout["LR"].Update(
      new Panel(selectedPet?.Description ?? "Select a pet to view its description.")
      {
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.White),
        Header = new PanelHeader(selectedPet?.SpeciesName ?? "Unknown Species"),
      }.Expand()
    );

    // Render the layout
    Console.SetCursorPosition(0, 0);
    AnsiConsole.Write(Layout);
  }

  public void TakeInput(ConsoleKeyInfo key)
  {
    List<RegisteredPet> pets = GameManager.Pets.GetPets();

    int petsPerRow = (Console.WindowWidth - 34) / 7;
    // edge case for padding merging with last pet (7 is pet plus space, but if padding aligns with last pet, it will be 6)
    if ((petsPerRow + 1) * 7 - 1 == Console.WindowWidth - 34) petsPerRow++;

    IEnumerable<string[]> uiArray = pets.Select(pet => pet.RegisteredId).ToList().Chunk(petsPerRow);
    int selectedPetIndex;

    switch (key.Key)
    {
      //case ConsoleKey.P:
      case ConsoleKey.Escape:
        Renderer.ChangeView(new MainMenu());
        break;
      case ConsoleKey.Enter:
        switch (_selectionIndex)
        {
          case 1:
            //_selectionIndex = 2;
            _confirmedPet = _selectedPet;
            break;
          case 2 when _confirmedPet != null && _name.Count > 0:
            GameManager game = GameManager.CreateNewGame(_confirmedPet, string.Join("", _name));
            Renderer.ChangeView(new GameMenuRoom(game));
            break;
        }
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
          
          if (selectedPetIndex + petsPerRow >= pets.Count)
          {
            _selectionIndex = 2;
            _selectedPet = null;
            break;
          }

          if (selectedPetIndex + petsPerRow < pets.Count)
          {
            _selectedPet = uiArray.SelectMany(x => x).ToList()[selectedPetIndex + petsPerRow] ?? pets[pets.Count - 1].RegisteredId;
          }
        }
        break;
      case ConsoleKey.UpArrow:
        if (_selectionIndex == 0) { break; };
        if (_selectionIndex == 2)
        {
          _selectionIndex = 1;
          _selectedPet = pets[^1].RegisteredId;
          break;
        }
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
        else if (_selectionIndex == 1)
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
        else if (_selectionIndex == 1)
        {
          _selectedPet ??= pets[0].RegisteredId;
          selectedPetIndex = uiArray.SelectMany(x => x).ToList().IndexOf(_selectedPet);
          
          // check for index 2
          if (selectedPetIndex == uiArray.SelectMany(x => x).ToList().Count - 1)
          {
            _selectionIndex = 2;
            _selectedPet = null;
            break;
          }
          
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
        int formattedLength;
        try
        {
          formattedLength = new Markup(string.Join("", _name)).Length;
        } catch
        {
          formattedLength = string.Join("", _name).Length;
        }
        if (formattedLength >= 50) break;
        _name.Insert(_cursorIndex, key.KeyChar.ToString());
        _cursorIndex++;
        break;
    }
  }
}