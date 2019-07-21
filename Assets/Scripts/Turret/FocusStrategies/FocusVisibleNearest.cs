﻿using UnityEngine;

public class FocusVisibleNearest : FocusStrategy
{
    public FocusVisibleNearest(ShootingTurret turret) : base(turret)
    {

    }

    public override GameObject GetEnemy()
    {
        GameObject enemyToFocus = null;
        float shortestDistance = Mathf.Infinity;

        foreach (int mask in turret.listEnemyMask)
        {
            Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, mask);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.tag == turret.enemyTag)
                {
                    // get ditance between the enemy and this
                    float distanceToEnemy = Vector3.Distance(turret.transform.position, collider.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        enemyToFocus = collider.gameObject;
                    }
                }
            }
        }

        return enemyToFocus;
    }

    public override Strategy GetStrat()
    {
        return Strategy.Nearest;
    }
}
