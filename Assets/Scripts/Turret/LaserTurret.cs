using UnityEngine;

public class LaserTurret : ShootingTurret
{
    [SerializeField]
    private LineRenderer lineRenderer = default;
    [SerializeField]
    private  ParticleSystem impactEffect = default;
    [SerializeField]
    private Light impactLight = default;

    // ---- INTERN ----
    private bool isPlayingSound = false;

    protected override void UpdateCall()
    {
        base.UpdateCall();

        if(target != null)
        {
            RaycastHit hit;
            // check if there is no obstacle between the fire point and the target
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, stats.range))
            {
                if (hit.collider.gameObject.layer == enemyLayer)
                {
                    targetEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if(targetEnemy != null)
                        Laser();
                }
                else
                {
                    StopLaser();
                }
            }
            else
            {
                StopLaser();
            }
        }
        else
        {
            StopLaser();
        }
    }

    protected void Laser()
    {
        targetEnemy.TakeDamage(damage * Time.deltaTime);
        targetEnemy.Slow(slowPercent);

        // graphics
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 direction = firePoint.position - target.position;

        // put the effect on the enemy border
        impactEffect.transform.rotation = Quaternion.LookRotation(direction);
        impactEffect.transform.position = target.position + direction.normalized * (target.localScale.x / 2);


        if (!isPlayingSound)
        {
            audioSource.loop = true;
            audioSource.clip = fireSound;
            audioSource.volume = volumeFire;
            audioSource.Play();
            isPlayingSound = true;
        }
    }

    protected override void InitBaseStats()
    {
        base.InitBaseStats();
    }

    private void StopLaser()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }
    }

    public override void BoostDamage(float damagePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.damage += (float) (((baseDamage * damagePercent) / 100f) * multiplier);
    }

    protected override void InitUpStats()
    {
        LaserTurret lt = (LaserTurret)turretUp;
        damageUp = lt.damage;
    }

    public override float GetFireRate()
    {
        return 0f;
    }

    public override float GetFireRateUp()
    {
        return 0f;
    }

    public override float GetSpeed()
    {
        return 1f;
    }

    public override float GetSpeedUp()
    {
        return 1f;
    }

    public override void BoostFireRate(float fireRatePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.baseDamage += (float)(((baseDamage * fireRatePercent) / 100f) * multiplier);
    }
}
