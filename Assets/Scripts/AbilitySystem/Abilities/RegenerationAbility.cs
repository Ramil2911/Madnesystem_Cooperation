using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

[AbilityId(0)]
public class RegenerationAbility : Ability
{
    public override float DurationRemaining { get; set; } = 15;
    public override float Duration { get; set; } = 15;

    public override string Name { get; set; } = "Ускоренная регенерация";
    public override string Description { get; set; } = "Арктика-8 - лучшее лекарство!";

    public float pointsPerSecond = 1;

    public override void OnUpdate(JobHandle? handle)
    {
        entityComponent.Health += Time.deltaTime * pointsPerSecond;
    }
}
