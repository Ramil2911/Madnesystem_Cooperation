using UnityEngine;

public class InventoryItem
{
    public bool Stackable => description.IsStackable;
    public uint amount = 1;
    public ItemDescription description;
}

//i know it's better to keep weapon description and common object description separate but i have no idea how to implement it in UI
//well, i have, but i'm too lazy
public abstract class ItemDescription
{
    public abstract string Name { get; set; }
    public abstract ItemType ItemType { get; set; }
    public abstract Sprite Sprite { get; set; }
    public abstract bool IsStackable { get; set; }
    public abstract GameObject worldRepresentation { get; set; }
    public abstract GameObject handsRepresentation { get; set; }
    //public abstract void SetupFromPrototype(WeaponPrototype prototype);

    public abstract GameObject SpawnWorldRepresentation(Vector3 pos, Quaternion rotation);
    public abstract GameObject SpawnHandsRepresentation(GameObject parent, HandsController hands);
}

public abstract class WeaponDescription : ItemDescription
{
    //[ClassExtends(typeof(ItemDescription))]
    //public ClassTypeReference itemDescriptionType;
    
    public int maxAmmoAmount = 10;
    public WeaponType weaponType;
    public float reloadTime = 5;
    public float shootingRate = 5; //Shots per second
    public float bulletSpeed = 100f;
    public float damage = 10f;
    public AmmoType requiredAmmoType;
    public int ammoAmount;

    public virtual void SetupWeaponFromPrototype(WeaponPrototype prototype)
    {
        maxAmmoAmount = prototype.ammoAmount;
        weaponType = prototype.weaponType;
        reloadTime = prototype.reloadTime;
        shootingRate = prototype.shootingRate;
        bulletSpeed = prototype.bulletSpeed;
        damage = prototype.damage;
        requiredAmmoType = prototype.requiredAmmoType;
        ammoAmount = prototype.ammoAmount;
        IsStackable = false;
        Sprite = prototype.Sprite;
        handsRepresentation = prototype.handsRepresentation;
        worldRepresentation = prototype.worldRepresentation;
    }
}

public enum ItemType
{
    Weapon,
    Undefined
}