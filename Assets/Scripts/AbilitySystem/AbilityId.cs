using System;
using UnityEngine;

public class AbilityId : Attribute
{
    public uint id;

    public AbilityId(uint id)
    {
        this.id = id;
    }
}
