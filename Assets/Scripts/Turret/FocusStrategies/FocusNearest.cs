using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusNearest : FocusStrategy
{
    public FocusNearest(ShootingTurret turret) : base(turret)
    {

    }

    public override GameObject GetEnemy()
    {
        GameObject enemyToFocus = null;
        float shortestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(turret.transform.position, turret.stats.range, turret.enemyMask);
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

        return enemyToFocus;
    }
}
