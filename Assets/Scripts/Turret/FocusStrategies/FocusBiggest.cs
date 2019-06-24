using UnityEngine;

public class FocusBiggest : FocusStrategy
{
    public FocusBiggest(ShootingTurret turret) : base(turret)
    {

    }

    public override GameObject GetEnemy()
    {
        GameObject enemyToFocus;
        if(lastTarget != null)
        {
            enemyToFocus = lastTarget.gameObject;
        }
        else
        {
            enemyToFocus = null;
        }

        float hpMax;
        if (lastTarget != null && Vector3.Distance(lastTarget.transform.position, turret.transform.position) < turret.stats.range)
        {
            hpMax = lastTarget.startHealth;
        }
        else
        {
            hpMax = 0f;
        }

        Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, turret.enemyMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == turret.enemyTag)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if(enemy != null)
                {
                    if (enemy.startHealth > hpMax)
                    {
                        enemyToFocus = collider.gameObject;
                        hpMax = enemy.startHealth;

                        lastTarget = enemy;
                    }
                }
            }
        }

        return enemyToFocus;
    }

    public override Strategy GetStrat()
    {
        return Strategy.Biggest;
    }
}
