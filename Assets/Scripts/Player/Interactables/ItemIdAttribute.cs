using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ItemIdAttribute : Attribute
{
    public uint Id { get; set; }
}
