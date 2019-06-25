using UnityEngine;

public class RangeTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach (ShootingTurret turret in listTargetToBoost)
        {
            turret.BoostRange(boost, true);
            Debug.Log("Boost : " + turret);
        }
    }

    protected override void UnboostAll()
    {
        foreach (ShootingTurret turret in listTarget)
        {
            turret.BoostRange(boost, false);
        }
    }
}
