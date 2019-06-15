using UnityEngine;

public abstract class FocusStrategy
{
    protected ShootingTurret turret;

    public FocusStrategy(ShootingTurret turret)
    {
        this.turret = turret;
    }

    public abstract GameObject GetEnemy();
}
