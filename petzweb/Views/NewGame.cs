using petzweb.ViewModel;
using Spectre.Console;

namespace petzweb.Views;

public class NewGame : IView
{
    private Layout? Layout { get; set; }
    public string ConsoleTitle { get; set; } = "PetzGame - New Game";

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
                                        new Layout("LR").Size(30).Invisible()
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

        // Controls Panel
        Layout["LTop"].Update(new Panel(new Rows(
            new Markup(
                @"[gray]Press [black on silver]/\[/] and [black on silver]\/[/] to select. [green]<enter>[/] to confirm.[/]"),
            new Markup("[gray]Press [black on silver]<esc>[/] to return to the main menu.[/]"))).Expand());

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
        }
    }
}