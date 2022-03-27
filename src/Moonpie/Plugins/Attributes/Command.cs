namespace Moonpie.Plugins.Attributes;


public class Command : Attribute
{
    public string Name { get; }
    public Command(string name)
    {
        Name = name;
    }
}