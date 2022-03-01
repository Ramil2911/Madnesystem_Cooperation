using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ItemId(Id = 1)]
public class Shotgun : WeaponDescription
{
    public override string Name { get; set; } = "Shotgun";
    public override ItemType ItemType { get; set; } = ItemType.Weapon;
    public override Sprite Sprite { get; set; }
    public override bool IsStackable { get; set; } = false;
    public override GameObject worldRepresentation { get; set; }
    public override GameObject handsRepresentation { get; set; }
    public override GameObject SpawnWorldRepresentation(Vector3 pos, Quaternion rotation)
    {
        var go = Object.Instantiate(this.worldRepresentation, pos, rotation);
        go.GetComponent<InventoryItemInteractableComponent>().item = this;
        return go;
    }

    public override GameObject SpawnHandsRepresentation(GameObject parent, HandsController hands)
    {
        var go = Object.Instantiate(this.handsRepresentation, parent.transform.position, parent.transform.rotation, parent.transform);
        go.transform.localPosition+= new Vector3(0, 0, -0.2f);
        go.transform.localRotation = Quaternion.identity;
        return go; 
    }
}
