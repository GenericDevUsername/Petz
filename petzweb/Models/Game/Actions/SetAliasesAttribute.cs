namespace petzweb.Models.Game.Actions;

[AttributeUsage(AttributeTargets.Class)]
public class SetAliasesAttribute(params string[] aliases) : Attribute
{
  public string[] Aliases { get; } = aliases;
}