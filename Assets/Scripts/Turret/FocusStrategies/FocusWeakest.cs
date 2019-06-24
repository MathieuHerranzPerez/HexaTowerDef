using UnityEngine;

public class FocusWeakest : FocusStrategy
{
    public FocusWeakest(ShootingTurret turret) : base(turret)
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

        int nbEnemies;
        if (lastTarget != null && Vector3.Distance(lastTarget.transform.position, turret.transform.position) < turret.stats.range)
        {
            nbEnemies = lastTarget.GetNbEnemies();
        }
        else
        {
            nbEnemies = int.MaxValue;
        }

        Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, turret.enemyMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == turret.enemyTag)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    int enemies = enemy.GetNbEnemies();
                    if (enemies < nbEnemies)
                    {
                        enemyToFocus = collider.gameObject;
                        nbEnemies = enemies;

                        lastTarget = enemy;
                    }
                }
            }
        }

        return enemyToFocus;
    }

    public override Strategy GetStrat()
    {
        return Strategy.Fastest;
    }
}
