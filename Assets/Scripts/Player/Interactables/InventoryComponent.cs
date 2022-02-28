using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class InventoryComponent : MonoBehaviour
{
    public WeaponObject[] defaultWeapons = new WeaponObject[8];
    
    public uint size = 8;
    [SerializeField] private InventoryItem[] _inventory = new InventoryItem[8];
    public Dictionary<AmmoType, uint> ammoInventory; //TODO: better ammo inventory

    public UnityEvent<InventoryComponent> inventoryChangedEvent = new();

    void Start()
    {
        _inventory = new InventoryItem[size];
        for (var i = 0; i<defaultWeapons.Length; ++i)
        {
            _inventory[i] = new InventoryItem()
            {
                amount = 1,
                description = ItemFactory.CreateWeapon(defaultWeapons[i].CreatePrototype())
            };
        }

        GetComponentInChildren<HandsController>().UpdateHands();
        inventoryChangedEvent.AddListener(ValidateInventory);
        inventoryChangedEvent.Invoke(this);
    }

    public void ValidateInventory(InventoryComponent comp)
    {
        for (var i = 0; i < size; ++i)
        {
            if(comp._inventory[i] == null) continue;
            if (comp._inventory[i].amount == 0)
            {
                comp.RemoveAt(i, true);
            }
        }
    }

    public bool Add(InventoryItem item)
    {
        for (var i = 0; i < size; ++i)
        {
            if (_inventory[i] == null || _inventory[i]?.description == null)
            {
                _inventory[i] = item;
                inventoryChangedEvent.Invoke(this);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        for (var i = 0; i < size; ++i)
        {
            _inventory[i] = null;
        }
        inventoryChangedEvent.Invoke(this);
    }

    public bool Contains(InventoryItem item)
    {
        for (var i = 0; i < size; ++i)
        {
            if (_inventory[i] == item) return true;
        }

        return false;
    }

    public bool Remove(InventoryItem item)
    {
        for (var i = 0; i < size; ++i)
        {
            if (_inventory[i] == item)
            {
                _inventory[i] = null;
                inventoryChangedEvent.Invoke(this);
                return true;
            }
        }
        return false;
    }

    public InventoryItem GetItemOrDefaultById(int id)
    {
        for (var i = 0; i < size; ++i)
        {
            var item = _inventory[i];
            if (GetItemId(item.description) == id)
            {
                _inventory[i] = null;
                return item;
            }
        }

        return default; //return null
    }

    public bool RemoveItemById(int id)
    {
        for (var i = 0; i < size; ++i)
        {
            var item = _inventory[i];
            if (GetItemId(item.description) == id)
            {
                _inventory[i] = null;
                inventoryChangedEvent.Invoke(this);
                return true;
            }
        }
        return false;
    }
    
    public int IndexOf(InventoryItem item)
    {
        for (var i = 0; i < size; ++i)
        {
            if (_inventory[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveAt(int index, bool disableUpdate = false)
    {
        _inventory[index] = null;
        if (disableUpdate)
        {
            inventoryChangedEvent.Invoke(this);
        }
    }

    public InventoryItem this[int index]
    {
        get => _inventory[index] is null ? new InventoryItem() {amount = 0} : _inventory[index];
        set => _inventory[index] = value;
    }

    static int GetItemId(ItemDescription item)
    {
        var idAttribute = item.GetType().GetCustomAttribute<ItemIdAttribute>();
        if (idAttribute is not null)
        {
            return (int)idAttribute.Id;
        }
        return -1;
    }
}
