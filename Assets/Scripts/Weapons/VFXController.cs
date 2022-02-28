using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect visualEffect;
    public new ParticleSystem particleSystem;
    public void Shoot()
    {
        particleSystem.Play();
        //visualEffect.Play();
    }
}
