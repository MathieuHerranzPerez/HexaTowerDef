public abstract class Debuff
{
    public float amount;
    public float duration;

    protected Enemy enemy;

    public Debuff(float amount, float duration, Enemy enemy)
    {
        this.amount = amount;
        this.duration = duration;

        this.enemy = enemy;
    }

    public abstract void DoEffect();


    public static bool operator>(Debuff d1, Debuff d2)
    {
        return d1.amount > d2.amount;
    }

    public static bool operator<(Debuff d1, Debuff d2)
    {
        return d1.amount < d2.amount;
    }
}
