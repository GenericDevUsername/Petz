namespace petz.Models.Game.Actions;

public interface IUseAction
{
  void Execute(GameData game, Dictionary<string, string> parameters, VariableStore variableStore);
}