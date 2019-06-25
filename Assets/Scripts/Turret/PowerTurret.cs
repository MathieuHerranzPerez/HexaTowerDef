
public class PowerTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach(ShootingTurret turret in listTargetToBoost)
        {
            turret.BoostDamage(boost, true);
        }
    }

    protected override void UnboostAll()
    {
        foreach (ShootingTurret turret in listTarget)
        {
            turret.BoostDamage(boost, false);
        }
    }
}
