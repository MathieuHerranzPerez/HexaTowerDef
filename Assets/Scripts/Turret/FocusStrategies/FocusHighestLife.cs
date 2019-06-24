using UnityEngine;

public class FocusHighestLife : FocusStrategy
{
    public FocusHighestLife(ShootingTurret turret) : base(turret)
    {

    }

    public override GameObject GetEnemy()
    {
        GameObject enemyToFocus;
        if (lastTarget != null)
        {
            enemyToFocus = lastTarget.gameObject;
        }
        else
        {
            enemyToFocus = null;
        }

        float life;
        if (lastTarget != null && Vector3.Distance(lastTarget.transform.position, turret.transform.position) < turret.stats.range)
        {
            life = lastTarget.Health;
        }
        else
        {
            life = 0f;
        }

        Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, turret.enemyMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == turret.enemyTag)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.Health > life)
                    {
                        enemyToFocus = collider.gameObject;
                        life = enemy.Health;

                        lastTarget = enemy;
                    }
                }
            }
        }

        return enemyToFocus;
    }

    public override Strategy GetStrat()
    {
        return Strategy.HighestLife;
    }
}

