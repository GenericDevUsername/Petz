using Spectre.Console;

namespace petz.ViewModel;

public class PercentageBarComponent(int max, int current, int length, Color color)
{
    public int Max { get; set; } = max;
    public int Current { get; set; } = current;
    public int Length { get; set; } = length;
    public Color BarColor { get; set; } = color;

    /// <summary>
    ///  Render the percentage bar
    /// </summary>
    /// <returns> The rendered percentage bar </returns>
    public string? Render()
    {
        int percentage = (int)Math.Round((double)Current / Max * Length);
        return
            $"[{BarColor.ToMarkup()}]{new string('█', percentage)}[/]{new string('█', Length - percentage)} {Current}";
    }
}