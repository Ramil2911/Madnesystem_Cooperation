using UnityEngine;

[CreateAssetMenu(menuName = "Create WeaponObject", fileName = "Weapon", order = 51)]
public class WeaponObject : ScriptableObject
{
    public string Name;
    public ItemType ItemType = ItemType.Weapon;
    public Sprite Sprite;
    public GameObject worldRepresentation;
    public GameObject handsRepresentation;
    
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

    public WeaponPrototype CreatePrototype()
    => WeaponPrototype.CreateFromScriptableObject(this);
    
    public static implicit operator WeaponDescription(WeaponObject obj)
    {
        return ItemFactory.CreateWeapon(obj.CreatePrototype());
    }
    
}

public enum WeaponType
{
    Auto,
    SemiAuto
}
