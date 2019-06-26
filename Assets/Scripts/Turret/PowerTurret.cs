
public class PowerTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach(BoostableTurret turret in listTargetToBoost)
        {
            turret.BoostDamage(boost, true);
        }
    }

    protected override void UnboostAll()
    {
        foreach (BoostableTurret turret in listTarget)
        {
            turret.BoostDamage(boost, false);
        }
    }
}
