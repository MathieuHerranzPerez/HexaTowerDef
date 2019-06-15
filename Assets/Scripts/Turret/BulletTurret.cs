using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : ShootingTurret
{
    // ---- INTERN ----
    protected float fireCountdown = 0f;
    protected bool canShoot = true;

    protected override void UpdateCall()
    {
        base.UpdateCall();

        if (target != null)
        { 
            if (canShoot)
            {
                RaycastHit hit;
                // check if there is no obstacle between the fire point and the target
                if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, stats.range))
                {
                    if (hit.collider.gameObject.layer == enemyLayer)
                    {
                        Debug.DrawRay(firePoint.position, firePoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);  // affD

                        Shoot();
                        canShoot = false;
                        fireCountdown = 1f / stats.fireRate;
                    }
                }
            }
        }

        fireCountdown -= Time.deltaTime;
        if(fireCountdown <= 0f)
            canShoot = true;
    }

    protected void Shoot()
    {
        audioSource.PlayOneShot(fireSound, volumeFire);     // play the sound
        GameObject bulletGameObject = (GameObject)Instantiate(stats.bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }
}
