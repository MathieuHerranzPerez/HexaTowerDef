using UnityEngine;

public class FocusSlowest : FocusStrategy
{
    public FocusSlowest(ShootingTurret turret) : base(turret)
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

        float speed;
        if (lastTarget != null && Vector3.Distance(lastTarget.transform.position, turret.transform.position) < turret.stats.range)
        {
            speed = lastTarget.speed;
        }
        else
        {
            speed = Mathf.Infinity;
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
                        if (enemy.startSpeed < speed)
                        {
                            enemyToFocus = collider.gameObject;
                            speed = enemy.startSpeed;

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
        return Strategy.Slowest;
    }
}
