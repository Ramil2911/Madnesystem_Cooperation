using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilController : MonoBehaviour
{
    public readonly List<Target> targets = new();
    public Transform x;
    public Transform y;

    public void Update()
    {
        for (var i = 0; i < targets.Count; ++i)
        {
            var target = targets[i];
            Recoil(ref target);
            targets[i] = target;
            if(target.elapsed>target.duration) targets.RemoveAt(i);
        }
    }
    
    private void Recoil(ref Target target)
    {
        var rotation = x.rotation.eulerAngles;
        rotation.x += (target.delta.x / target.duration) * Time.deltaTime;
        x.rotation = Quaternion.Euler(rotation);
            
        rotation = y.rotation.eulerAngles;
        rotation.y += (target.delta.y / target.duration) * Time.deltaTime;
        y.rotation = Quaternion.Euler(rotation);

        target.elapsed += Time.deltaTime;
    }

    public void Add(Vector2 delta, float duration)
    {
        targets.Add(new Target()
        {
            delta = delta,
            duration = duration
        });
    }

    public struct Target
    {
        public Vector2 delta;
        public float duration;
        public float elapsed;
    }
}
