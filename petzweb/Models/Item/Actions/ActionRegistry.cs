using System.Reflection;


namespace petzweb.Models;
[AttributeUsage(AttributeTargets.Class)]
public class SetAliasesAttribute(params string[] aliases) : Attribute
{
    public string[] Aliases { get; } = aliases;
}

public class ActionRegistry
{
    private readonly Dictionary<string, IUseAction?> _registry = new();

    public ActionRegistry()
    {
        PopulateActionRegistry();
        Console.WriteLine("Action registry populated with " + _registry.Count + " actions.");
        Console.WriteLine("Action registry: " + string.Join(", ", _registry.Keys));
    }

    private void PopulateActionRegistry()
    {
        var interfaceType = typeof(IUseAction);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        foreach (var type in types)
        {
            var instance = (IUseAction?)Activator.CreateInstance(type);

            // Add default alias (class name)
            _registry[type.Name.ToLowerInvariant()] = instance;

            // Add aliases from attributes if present
            var aliases = type.GetCustomAttributes<SetAliasesAttribute>();
            foreach (var alias in aliases)
            {
                foreach (var aliasName in alias.Aliases)
                {
                    _registry[aliasName.ToLowerInvariant()] = instance;
                }
            }
        }
    }

    public void AddActionWithAliases(IUseAction? action, params string[] aliases)
    {
        foreach (var alias in aliases)
        {
            _registry[alias] = action;
        }
    }

    // allow out variable to be used
    public IUseAction? GetAction(string actionName, out IUseAction? action)
    {
        return _registry.TryGetValue(actionName.ToLowerInvariant(), out action) ? action : _registry.GetValueOrDefault(actionName);
    }
}
