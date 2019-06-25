using UnityEngine;

public abstract class ShootingTurret : Turret
{
    public Strategy Strat { get { return focusStratey.GetStrat() ; } }

    public float SlowPercent { get { return slowPercent; } }
    public float SlowPercentUp { get { return slowUp; } }

    [SerializeField]
    protected float damage = 50f;
    [SerializeField]
    protected float slowPercent = 0f;

    [Header("Setup")]
    public string enemyTag = "EnemyWalker";       // target type
    public LayerMask enemyMask;
    [SerializeField]
    protected Transform partToRotateY = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform partToRotateX = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform firePoint = default;

    // ---- INTERN ----

    protected int enemyLayer; 
    protected Transform target;
    protected Enemy targetEnemy;
    protected FocusStrategy focusStratey;

    protected float damageUp;
    protected float baseDamage;

    protected float slowUp;
    protected float baseSlow;


    protected override void Start()
    {
        base.Start();

        InitBaseStats();

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        focusStratey = new FocusVisibleNearest(this);
        enemyLayer = Mathf.RoundToInt(Mathf.Log(enemyMask.value, 2));
    }

    protected override void UpdateCall()
    {
        if (target != null)
        {
            LockOnTarget();
        }
    }

    public void ChangeStrategy(FocusStrategy focusStratey)
    {
        this.focusStratey = focusStratey;
    }

    // if we want to change the target strategy, it's here
    protected void UpdateTarget()
    {
        GameObject enemyToFocus = focusStratey.GetEnemy();

        if (enemyToFocus != null)
        {
            target = enemyToFocus.transform;
            targetEnemy = enemyToFocus.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    protected void LockOnTarget()
    {
        // Y
        Vector3 directionY = target.position - partToRotateY.position;   // direction to the target
        Quaternion lookRatationY = Quaternion.LookRotation(directionY);
        Vector3 rotationY = Quaternion.Lerp(partToRotateY.rotation, lookRatationY, Time.deltaTime * stats.turnSpeed).eulerAngles;
        partToRotateY.rotation = Quaternion.Euler(0f, rotationY.y, 0f);

        // X
        Vector3 directionX = target.position - partToRotateX.position;   // direction to the target
        Quaternion lookRatationX = Quaternion.LookRotation(directionX);
        Vector3 rotationX = Quaternion.Lerp(partToRotateX.rotation, lookRatationX, Time.deltaTime * stats.turnSpeed).eulerAngles;
        partToRotateX.localRotation = Quaternion.Euler(rotationX.x, 0f, 0f);
    }

    protected virtual void InitBaseStats()
    {
        baseDamage = damage;
        baseSlow = slowPercent;

        stats.baseRange = stats.range;
    }

    protected override void InitUpStats()
    {
        base.InitUpStats();

        ShootingTurret st = (ShootingTurret)turretUp;
        damageUp = st.damage;
        slowUp = st.slowPercent;
    }

    public abstract void BoostDamage(float damage, bool isBuff);
    public virtual void BoostRange(float range, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        stats.range += (float) (range * multiplier);
    }

    public abstract float GetFireRate();
    public abstract float GetFireRateUp();

    public float GetDamage()
    {
        return damage;
    }
    public float GetDamageUp()
    {
        return damageUp;
    }
}
