using UnityEngine;

public abstract class FocusStrategy
{
    protected ShootingTurret turret;
    protected Enemy lastTarget = null;

    public FocusStrategy(ShootingTurret turret)
    {
        this.turret = turret;
    }

    public abstract GameObject GetEnemy();
    public abstract Strategy GetStrat();
}
