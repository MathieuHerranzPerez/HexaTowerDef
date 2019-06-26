using UnityEngine;

public class BasicWithoutTargetTurret : WithoutTargetTurret
{
    private bool doEffect = false;

    protected override void Shoot()
    {
        Collider[] colliderHitObjectArray = Physics.OverlapSphere(transform.position, stats.range, enemyLayer);
        foreach (Collider collider in colliderHitObjectArray)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.Slow(slow, slowDuration);
                doEffect = true;
            }
        }

        if(doEffect)
        {
            ParticleSystem ps = (ParticleSystem)Instantiate(effect, effectSpawnPoint.position, effectSpawnPoint.rotation, transform);
            Destroy(ps.gameObject, 2f);
            doEffect = false;
        }
    }

    public override void BoostDamage(float damagePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.baseDamage += (float)(((baseDamage * damagePercent) / 100f) * multiplier);
    }

    public override void BoostFireRate(float fireRatePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.fireRate += (float)(((baseFireRate * fireRatePercent) / 100f) * multiplier); ;
    }
}
