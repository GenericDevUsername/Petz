namespace petz.Views;

public interface IView
{
    public string ConsoleTitle { get; set; }
    public void Render();
    public void Initialize();
    public void TakeInput(ConsoleKeyInfo key);
}