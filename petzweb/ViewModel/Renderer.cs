using System.Text;
using petzweb.Views;
using Spectre.Console;

namespace petzweb.ViewModel;

public static class Renderer
{
    private static bool Initialised { get; set; }
    private static IView? CurrentView { get; set; }
    private static Thread? InputThread { get; set; }
    private static Thread? ConsoleResizeListener { get; set; }

    public static void Start(IView? view)
    {
        // prevent multiple initialisations
        if (Initialised) throw new InvalidOperationException("Renderer has already been initialised.");
        Initialised = true;
        Console.Clear();
        Console.CursorVisible = false;


        // Initialise the view window
        CurrentView = view ?? throw new ArgumentNullException(nameof(view));
        CurrentView.Render();
        Console.Title = CurrentView?.ConsoleTitle ?? "Console";


        // create a new thread to listen for input
        InputThread = new Thread(() =>
        {
            while (Initialised)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                CurrentView?.TakeInput(key);
                CurrentView?.Render();
            }
        });
        InputThread.Start();

        // create a new thread to listen for console resizes
        ConsoleResizeListener = new Thread(() =>
        {
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
            DateTime lastResize = DateTime.Now;
            bool resized = false;
            while (Initialised)
            {
                // only resize once if the user has resized the console in the last 500ms
                if (Console.WindowWidth != lastWidth || Console.WindowHeight != lastHeight)
                {
                    Console.Clear();
                    lastWidth = Console.WindowWidth;
                    lastHeight = Console.WindowHeight;
                    lastResize = DateTime.Now;
                    resized = true;
                }

                if (DateTime.Now - lastResize <= TimeSpan.FromMilliseconds(100)) continue;
                if (!resized) continue;
                CurrentView?.Render();
                resized = false;
            }
        });
        ConsoleResizeListener.Start();
    }

    public static void ChangeView(IView? view)
    {
        // change the current view
        CurrentView = view;
        Console.Title = CurrentView?.ConsoleTitle ?? "Console";
        CurrentView?.Render();
    }

    public static void Stop()
    {
        // stop the input thread
        Initialised = false;

        // clear the console
        Console.Clear();
        Console.ResetColor();
        Console.Title = "Console";
        Console.CursorVisible = true;
    }
}