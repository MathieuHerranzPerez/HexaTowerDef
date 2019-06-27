using UnityEngine;

public abstract class BulletTurret : ShootingTurret
{
    [Range(0.05f, 20f)]
    [SerializeField]
    protected float fireRate = 1f;
    [SerializeField]
    protected GameObject bulletPrefab;

    // ---- INTERN ----
    protected float fireCountdown = 0f;
    protected bool canShoot = true;

    protected float fireRateUp;
    protected float baseFireRate;

    protected Projectile lastBulletFired;

    protected override void UpdateCall()
    {
        base.UpdateCall();

        if (target != null)
        { 
            if (canShoot)
            {
                if (CheckShootCondition())
                {
                    Shoot();
                    canShoot = false;
                    fireCountdown = 1f / fireRate;
                }
            }
        }

        fireCountdown -= Time.deltaTime;
        if(fireCountdown <= 0f)
            canShoot = true;
    }

    protected abstract bool CheckShootCondition();

    protected virtual void Shoot()
    {
        audioSource.PlayOneShot(fireSound, volumeFire);     // play the sound
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile bullet = bulletGameObject.GetComponent<Projectile>();
        Destroy(bulletGameObject, 5f);

        lastBulletFired = bullet;
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
        damageUp = bt.bulletPrefab.GetComponent<Projectile>().Damage;
    }

    public override float GetFireRate()
    {
        return fireRate;
    }

    public override float GetFireRateUp()
    {
        return fireRateUp;
    }

    public override float GetSpeed()
    {
        return fireRate;
    }

    public override float GetSpeedUp()
    {
        return fireRateUp;
    }

    public override void BoostFireRate(float fireRatePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.fireRate += (float)(((baseFireRate * fireRatePercent) / 100f) * multiplier);
    }
}
