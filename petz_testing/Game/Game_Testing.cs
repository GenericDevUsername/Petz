using petz.Models.Game;

namespace petz_testing.Game;

public class Game_Testing
{
  public static readonly string GameDataPath =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) +
    "/.petzgame";
  public static string GameSavesPath => GameDataPath + "/saves";
  public static string LogPath => GameDataPath + "/logs";
  
  
  [Test]
  public void Test_ThatFilesAreCreated()
  {
    GameManager.Initialize();
    Assert.Multiple(() =>
    {
       Assert.That(Directory.Exists(GameDataPath));
       Assert.That(Directory.Exists(GameSavesPath));
       Assert.That(Directory.Exists(LogPath));
    });
  }
}