using UnityEngine;

public class RocketTurret : BulletTurret
{
    [SerializeField]
    protected float impulseForce = 50f;

    protected override bool CheckShootCondition()
    {
        return true;
    }

    protected override void Shoot()
    {
        base.Shoot();

        if (lastBulletFired != null && lastBulletFired is GuidedMissile)
        {
            GuidedMissile missile = (GuidedMissile)lastBulletFired;
            missile.ResetVelocity();
            missile.Impulse(impulseForce);
        }
    }
}
