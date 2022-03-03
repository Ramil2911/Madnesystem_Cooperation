

[AbilityId(1)]
public class PlayerDebuffAbility : Ability
{
    public override float DurationRemaining { get; set; } = float.PositiveInfinity;
    public override float Duration { get; set; } = float.PositiveInfinity;
    public override string Name { get; set; } = "Космическая болезнь";
}
