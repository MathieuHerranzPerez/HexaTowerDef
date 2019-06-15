using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : ShootingTurret
{
    protected float fireCountdown = 0f;

    protected override void UpdateCall()
    {
        base.UpdateCall();

        if (target != null)
        {
            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / stats.fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
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
