
public class RangeTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach (BoostableTurret turret in listTargetToBoost)
        {
            turret.BoostRange(boost, true);
        }
    }

    protected override void UnboostAll()
    {
        foreach (BoostableTurret turret in listTarget)
        {
            turret.BoostRange(boost, false);
        }
    }
}
