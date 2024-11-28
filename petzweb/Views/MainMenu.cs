using petzweb.ViewModel;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace petzweb.Views;

public class MainMenu : IView
{
    private Layout? Layout { get; set; }
    private string[] Options { get; } = ["Adopt new pet", "Load current pets", "Exit to desktop"];
    private int SelectedOption { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - Main Menu";

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
            );

        // Update the top column
        Layout["Top"].Update(
            new Panel(
                    Align.Center(
                        new FigletText(font, "--- petzgame ---").Color(Color.Aqua),
                        VerticalAlignment.Middle))
                .Expand());

        IRenderable[] rows = new IRenderable[Options.Length + 1];
        // pad name to width of 20
        for (int i = 0; i < Options.Length; i++)
            rows[i] = new Panel(Align.Center(new Markup(SelectedOption == i ? $"[yellow]{Options[i]}[/]" : Options[i])))
            {
                Border = BoxBorder.Rounded,
                Width = 20,
                BorderStyle = SelectedOption == i ? new Style(Color.Yellow) : new Style(Color.White)
            };

        rows[Options.Length] =
            new Markup(
                @"[gray]Press [black on silver]/\[/] and [black on silver]\/[/] to select. [green]<enter>[/] to confirm.[/]");

        // Update the bottom column
        Layout["Bottom"].Update(
            new Panel(
                Align.Center(new Rows(rows).Expand())
            ).Expand()
        );

        // Render the layout
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(Layout);
    }

    public void TakeInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.W:
            case ConsoleKey.UpArrow:
                SelectedOption = SelectedOption == 0 ? Options.Length - 1 : SelectedOption - 1;
                break;
            case ConsoleKey.S:
            case ConsoleKey.DownArrow:
                SelectedOption = SelectedOption == Options.Length - 1 ? 0 : SelectedOption + 1;
                break;
            case ConsoleKey.Enter:
                switch (SelectedOption)
                {
                    case 0:
                        Renderer.ChangeView(new NewGame());
                        break;
                    case 1:
                        Renderer.ChangeView(new LoadGame());
                        break;
                    case 2:
                        Renderer.Stop();
                        break;
                }

                break;
        }
    }
}