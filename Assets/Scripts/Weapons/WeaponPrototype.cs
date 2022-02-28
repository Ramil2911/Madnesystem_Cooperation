using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrototype
{
    public string Name { get; set; }
    public ItemType ItemType { get; set; } = ItemType.Weapon;
    public Sprite Sprite { get; set; }
    public GameObject worldRepresentation { get; set; }
    public GameObject handsRepresentation { get; set; }
    
    //[ClassExtends(typeof(ItemDescription))]
    //public ClassTypeReference itemDescriptionType;
    
    public uint id;
    public int maxAmmoAmount = 10;
    public WeaponType weaponType;
    public float reloadTime = 5;
    public float shootingRate = 5; //Shots per second
    public float bulletSpeed = 100f;
    public float damage = 10f;
    public AmmoType requiredAmmoType;
    public int ammoAmount;

    private WeaponPrototype(){}

    public static WeaponPrototype CreateFromScriptableObject(WeaponObject obj)
    {
        return new WeaponPrototype
        {
            Name = obj.Name,
            ItemType = obj.ItemType,
            worldRepresentation = obj.worldRepresentation,
            handsRepresentation = obj.handsRepresentation,
            id = obj.id,
            maxAmmoAmount = obj.maxAmmoAmount,
            weaponType = obj.weaponType,
            reloadTime = obj.reloadTime,
            shootingRate = obj.shootingRate,
            bulletSpeed = obj.bulletSpeed,
            damage = obj.damage,
            requiredAmmoType = obj.requiredAmmoType,
            ammoAmount = obj.ammoAmount,
            Sprite = obj.Sprite
        };
    }
}
