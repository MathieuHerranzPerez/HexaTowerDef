
public class SpeedTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach (BoostableTurret turret in listTargetToBoost)
        { 
            turret.BoostFireRate(boost, true);
        }
    }

    protected override void UnboostAll()
    {
        foreach (BoostableTurret turret in listTarget)
        {
            turret.BoostFireRate(boost, false);
        }
    }
}
