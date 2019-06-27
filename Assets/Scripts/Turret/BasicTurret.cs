
using UnityEngine;

public class BasicTurret : BulletTurret
{
    protected override bool CheckShootCondition()
    {
        bool res = false;
        RaycastHit hit;
        // check if there is no obstacle between the fire point and the target
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, stats.range))
        {
            if (hit.collider.gameObject.layer == enemyLayer)
            {
                Debug.DrawRay(firePoint.position, firePoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);  // affD
                res = true;
            }
        }

        return res;
    }
}
