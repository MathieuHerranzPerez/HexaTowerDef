using UnityEngine;

public class FocusSmallest : FocusStrategy
{
    public FocusSmallest(ShootingTurret turret) : base(turret)
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

        float hpMin;
        if (lastTarget != null && Vector3.Distance(lastTarget.transform.position, turret.transform.position) < turret.stats.range)
        {
            hpMin = lastTarget.startHealth;
        }
        else
        {
            hpMin = Mathf.Infinity;
        }

        foreach (int mask in turret.listEnemyMask)
        {
            Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, mask);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.tag == turret.enemyTag)
                {
                    Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (enemy.startHealth < hpMin)
                        {
                            enemyToFocus = collider.gameObject;
                            hpMin = enemy.startHealth;

                            lastTarget = enemy;
                        }
                    }
                }
            }
        }

        return enemyToFocus;
    }

    public override Strategy GetStrat()
    {
        return Strategy.Smallest;
    }
}
