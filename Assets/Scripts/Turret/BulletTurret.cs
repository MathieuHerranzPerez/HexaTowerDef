using UnityEngine;

public class BulletTurret : ShootingTurret
{
    [Range(0.05f, 20f)]
    public float fireRate = 1f;
    public GameObject bulletPrefab;

    // ---- INTERN ----
    protected float fireCountdown = 0f;
    protected bool canShoot = true;

    protected float fireRateUp;
    protected float baseFireRate;

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
                        fireCountdown = 1f / fireRate;
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
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        bullet.SetDamage((int) damage);

        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }

    protected override void InitBaseStats()
    {
        base.InitBaseStats();

        baseFireRate = fireRate;
        baseDamage = damage;
    }

    protected override void InitUpStats()
    {
        base.InitUpStats();

        BulletTurret bt = (BulletTurret)turretUp;
        fireRateUp = bt.fireRate;
        slowUp = bt.slowPercent;
        damageUp = bt.bulletPrefab.GetComponent<Bullet>().Damage;
    }

    public override void BoostDamage(float damage, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        damage += (float) (damage * multiplier);
    }

    public override float GetFireRate()
    {
        return fireRate;
    }

    public override float GetFireRateUp()
    {
        return fireRateUp;
    }

    public void BoostFireRate(float fireRate, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        fireRate += (float)(fireRate * multiplier);
    }
}
