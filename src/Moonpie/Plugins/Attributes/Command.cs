﻿namespace Moonpie.Plugins.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class Command : Attribute
{
    public string Name { get; }
    public Command(string name)
    {
        Name = name;
    }
}