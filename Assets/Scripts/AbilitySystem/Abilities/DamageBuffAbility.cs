
[AbilityId(2)]
public class DamageBuffAbility : Ability
{
    public override float DurationRemaining { get; set; } = float.PositiveInfinity;
    public override float Duration { get; set; } = float.PositiveInfinity;

    public override string Name { get; set; } = "Всё БЕСИТ!";
    public override string Description { get; set; } = "Космопендосы взбесили вас!";

    private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            WeaponBuff.damageMultiplier = 1 + (float)_level / 10;
        }
    }

    public override void OnStart()
    {
        WeaponBuff.damageMultiplier = 1 + (float)_level / 10;
    }
}
