using UnityEngine;

public abstract class ShootingTurret : BoostableTurret, SlowDamageSpeedTurret
{
    public Strategy Strat { get { return focusStratey.GetStrat() ; } }

    public float SlowPercent { get { return slowPercent; } }
    public float SlowPercentUp { get { return slowUp; } }

    [SerializeField]
    protected float damage = 50f;
    [SerializeField]
    [Range(0f, 100f)]
    protected float slowPercent = 0f;

    [Header("Setup")]
    public string enemyTag = "EnemyWalker";       // target type
    //public LayerMask enemyMask;
    public LayerMask[] listEnemyMask;
    [SerializeField]
    protected Transform partToRotateY = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform partToRotateX = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform firePoint = default;
    [SerializeField]
    protected Transform[] listFirePointChecker = default;

    // ---- INTERN ----

    //protected int enemyLayer;
    protected int[] listEnemyLayer;
    protected Transform target;
    protected Enemy targetEnemy;
    protected FocusStrategy focusStratey;

    protected float damageUp;
    protected float baseDamage;

    protected float slowUp;
    protected float baseSlow;


    protected void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        focusStratey = new FocusVisibleNearest(this);
        //enemyLayer = Mathf.RoundToInt(Mathf.Log(enemyMask.value, 2));

        listEnemyLayer = new int[listEnemyMask.Length];
        int i = 0;
        foreach(LayerMask lm in listEnemyMask)
        {
            listEnemyLayer[i] = Mathf.RoundToInt(Mathf.Log(lm.value, 2));
            ++i;
        }
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

    protected override void InitBaseStats()
    {
        base.InitBaseStats();

        baseDamage = damage;
        baseSlow = slowPercent;
    }

    protected override void InitUpStats()
    {
        base.InitUpStats();

        ShootingTurret st = (ShootingTurret)turretUp;
        damageUp = st.damage;
        slowUp = st.slowPercent;
    }

    public abstract float GetFireRate();
    public abstract float GetFireRateUp();

    public override void BoostDamage(float damagePercent, bool isBuff)
    {
        int multiplier = isBuff ? 1 : -1;
        this.damage += (float)(((baseDamage * damagePercent) / 100f) * multiplier);
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
        return slowPercent;
    }

    public float GetSlowUp()
    {
        return SlowPercentUp;
    }

    public abstract float GetSpeed();

    public abstract float GetSpeedUp();
}
