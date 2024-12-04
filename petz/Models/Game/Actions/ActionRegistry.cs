using System.Reflection;

namespace petz.Models.Game.Actions;

public class ActionRegistry
{
  private static readonly Dictionary<string, IUseAction?> Registry = new();
  private bool _populated = false;

  public ActionRegistry()
  {
    PopulateActionRegistry();
    Program.Log("Action registry populated with " + Registry.Count + " actions.");
    Program.Log("Action registry: " + string.Join(", ", Registry.Keys));
  }
  
  public static int Count => Registry.Count;
  public static IEnumerable<string> Keys => Registry.Keys;

  public static void Initialize()
  {
    Program.Log("Initializing action registry...");
    PopulateActionRegistry();
    Program.Log("Action registry populated with " + Registry.Count + " actions.");
    Program.Log("Action registry: " + string.Join(", ", Registry.Keys));
  }
  
  private static void PopulateActionRegistry()
  {
    Type interfaceType = typeof(IUseAction);
    IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

    foreach (Type type in types)
    {
      IUseAction? instance = (IUseAction?)Activator.CreateInstance(type);

      // Add default alias (class name)
      Registry[type.Name.ToLowerInvariant()] = instance;

      // Add aliases from attributes if present
      IEnumerable<SetAliasesAttribute> aliases = type.GetCustomAttributes<SetAliasesAttribute>();
      foreach (SetAliasesAttribute alias in aliases)
      foreach (string aliasName in alias.Aliases)
        Registry[aliasName.ToLowerInvariant()] = instance;
    }
  }

  public void AddActionWithAliases(IUseAction? action, params string[] aliases)
  {
    foreach (string alias in aliases) Registry[alias] = action;
  }

  // allow out variable to be used
  public IUseAction? GetAction(string actionName, out IUseAction? action)
  {
    return Registry.TryGetValue(actionName.ToLowerInvariant(), out action)
      ? action
      : Registry.GetValueOrDefault(actionName);
  }
}