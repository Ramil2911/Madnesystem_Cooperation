using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : WeaponDescription
{
    public override string Name { get; set; } = "";
    public override ItemType ItemType { get; set; } = ItemType.Undefined;
    public override Sprite Sprite { get; set; } = UnityEngine.Sprite.Create(Texture2D.grayTexture, Rect.zero, Vector2.zero);
    public override bool IsStackable { get; set; } = false;
    public override GameObject worldRepresentation { get; set; }
    public override GameObject handsRepresentation { get; set; }
    public override GameObject SpawnWorldRepresentation(Vector3 pos, Quaternion rotation)
        => Object.Instantiate(this.worldRepresentation, pos, rotation);

    public override GameObject SpawnHandsRepresentation(GameObject parent, HandsController hands)
        => Object.Instantiate(this.handsRepresentation, parent.transform.position + Vector3.up * 2, hands._hands == null ? Quaternion.identity : hands._hands.transform.rotation, parent.transform);
}
