namespace petzweb.Views;

public interface IView
{
    public string ConsoleTitle { get; set; }
    public void Render();
    public void TakeInput(ConsoleKeyInfo key);
}