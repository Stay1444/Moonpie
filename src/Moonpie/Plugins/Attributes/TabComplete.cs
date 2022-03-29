namespace Moonpie.Plugins.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class TabComplete : Attribute
{
    public string Name { get; }
    public TabComplete(string name)
    {
        Name = name;
    }
}