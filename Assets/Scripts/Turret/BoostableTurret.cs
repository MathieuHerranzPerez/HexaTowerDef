
public abstract class BoostableTurret : Turret
{
    public abstract void BoostDamage(float damagePercent, bool isBuff);
    public virtual void BoostRange(float rangePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        stats.range += (float)(((stats.baseRange * rangePercent) / 100f) * multiplier);
    }
    public abstract void BoostFireRate(float fireRatePercent, bool isBuff);
}
