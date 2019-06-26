using UnityEngine;

public abstract class WithoutTargetTurret : BoostableTurret, SlowDamageSpeedTurret
{
    [SerializeField]
    protected float fireRate = 1f;
    [SerializeField]
    [Range(0f, 100f)]
    protected float slow = 0f;
    [SerializeField]
    protected float slowDuration = 1f;
    [SerializeField]
    protected float damage = 50f;

    [Header("Setup")]
    [SerializeField]
    protected LayerMask enemyLayer = default;
    [SerializeField]
    protected ParticleSystem effect = default;
    [SerializeField]
    protected Transform effectSpawnPoint = default;

    // ---- INTERN ----
    protected float baseFireRate;
    protected float baseSlow;
    protected float baseDamage;

    protected float fireRateUp;
    protected float slowUp;
    protected float damageUp;

    private float time = 0f;

    protected override void InitBaseStats()
    {
        base.InitBaseStats();

        baseFireRate = fireRate;
        baseSlow = slow;
        baseDamage = damage;
    }

    protected override void InitUpStats()
    {
        base.InitUpStats();

        WithoutTargetTurret wtt = (WithoutTargetTurret)turretUp;
        fireRateUp = wtt.fireRate;
        slowUp = wtt.slow;
        damageUp = wtt.damage;
    }


    protected override void UpdateCall()
    {
        if(time >= 1f/fireRate)
        {
            time = 0f;
            Shoot();
        }

        time += Time.deltaTime;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetDamageUp()
    {
        return damageUp;
    }

    public float GetSlow()
    {
        return slow;
    }

    public float GetSlowUp()
    {
        return slowUp;
    }

    public float GetSpeed()
    {
        return fireRate;
    }

    public float GetSpeedUp()
    {
        return fireRateUp;
    }

    protected abstract void Shoot();
}
