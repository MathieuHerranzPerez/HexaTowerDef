using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClassic : Projectile
{
    protected override void ActOnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy currentEnemy = other.GetComponent<Enemy>();
            if (currentEnemy != null)
            {
                HitTarget(other.transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void UpdateCall()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 direction = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            // if we hit the target
            if (direction.magnitude <= distanceThisFrame)
            {
                HitTarget(target);
            }
            else
            {
                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                transform.LookAt(target);
            }
        }
        // follow the gravity
        //transform.LookAt(transform.position + rb.velocity);
    }
}
