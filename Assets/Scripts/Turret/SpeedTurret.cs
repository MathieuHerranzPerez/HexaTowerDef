
public class SpeedTurret : BoostTurret
{
    protected override void BoostAll()
    {
        foreach (ShootingTurret turret in listTargetToBoost)
        {
            if(turret is BulletTurret)
            {
                BulletTurret bt = (BulletTurret)turret;
                bt.BoostFireRate(boost, true);
            } 
        }
    }

    protected override void UnboostAll()
    {
        foreach (ShootingTurret turret in listTarget)
        {
            if (turret is BulletTurret)
            {
                BulletTurret bt = (BulletTurret)turret;
                bt.BoostFireRate(boost, false);
            }
        }
    }
}
