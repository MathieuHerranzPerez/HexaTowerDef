public class SlowDebuff : Debuff
{
    public SlowDebuff(float amount, float duration, Enemy enemy) : base(amount, duration, enemy)
    {
    }

    public override void DoEffect()
    {
        enemy.Slow(amount);
    }
}
