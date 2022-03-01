using UnityEngine;
using UnityEngine.VFX;

public class DestroyOnVFXFinished : MonoBehaviour
{
    public VisualEffect effect;
    public float ttl;

    // Update is called once per frame
    void Start()
    {
        Destroy(this.gameObject, ttl);
    }
}
