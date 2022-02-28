using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ItemFactory
{
    private static IEnumerable<Type> _types = GetItemsTypes(Assembly.GetExecutingAssembly());

    public static WeaponDescription CreateWeapon(WeaponPrototype prototype)
    {
        var weapon = (WeaponDescription)Activator.CreateInstance(_types.FirstOrDefault(x 
            => x.GetCustomAttribute<ItemIdAttribute>().Id == prototype.id)!);
        weapon.SetupWeaponFromPrototype(prototype);
        return weapon;
    }

    public static GameObject SpawnWeapon(WeaponDescription desc, Vector3 position, Quaternion rotation) 
        => desc.SpawnWorldRepresentation(position, rotation);

    
    public static GameObject SpawnWeaponFromPrototype(WeaponPrototype prototype, Vector3 position, Quaternion rotation) 
        => CreateWeapon(prototype).SpawnWorldRepresentation(position, rotation);
    
    
    static IEnumerable<Type> GetItemsTypes(Assembly assembly)
    {
        var types = assembly.GetTypes();
        for (var index = 0; index < types.Length; index++)
        {
            var type = types[index];
            if (type.GetCustomAttributes(typeof(ItemIdAttribute), true).Length > 0)
            {
                yield return type;
            }
        }
    }
}
