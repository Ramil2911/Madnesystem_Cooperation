using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemInteractableComponent : Interactable
{
    [SerializeField]
    public WeaponObject weaponObject;
    public ItemDescription item
    {
        get => weaponObject ? ItemFactory.CreateWeapon(weaponObject.CreatePrototype()) : _item;
        set => _item = value;
    }
    
    [HideInInspector]
    private ItemDescription _item;
    
    public override void Interact(GameObject actor)
    {
        if (actor.TryGetComponent<InventoryComponent>(out var inventory))
        {
            if (inventory.Add(new InventoryItem()
            {
                description = item,
                amount = 1
            }))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
