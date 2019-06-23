using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Turret : MonoBehaviour
{
    public TurretStats stats;

    [Header("Setup")]
    [SerializeField]
    protected Transform partToRotateY = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform partToRotateX = default;          // part of the turret to rotate
    [SerializeField]
    protected Transform firePoint = default;
    [SerializeField]
    protected Sprite shopImg = default;

    [Header("Sounds")]
    [SerializeField]
    protected AudioClip fireSound = default;             // sound when shoot
    [Range(0.05f, 1f)]
    [SerializeField]
    protected float volumeFire = 0.5f;


    public bool HasAnUpgrade { get { return stats.upgradedPrefab != null; } }

    // ---- INTERN ----
    protected AudioSource audioSource;
    protected Wall wall;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        if (stats.upgradedPrefab != null)
        {
            Turret turretUP = stats.upgradedPrefab.GetComponent<Turret>();
            stats.fireRateUP = turretUP.stats.fireRate;
            stats.rangeUP = turretUP.stats.range;

            if (stats.useLaser)
            {
                stats.slowUP = turretUP.stats.slowPercent;
                stats.damageUP = turretUP.stats.damageOverTime;
            }
            else
            {
                stats.slowUP = 0f;
                stats.damageUP = turretUP.stats.bulletPrefab.GetComponent<Bullet>().Damage;
            }
        }
    }

    public abstract int GetDamage();

    protected void Update()
    {
        UpdateCall();
        //if (target != null)
        //{
        //    LockOnTarget();

        //    if (stats.useLaser)
        //    {
        //        Laser();
        //    }
        //    else
        //    {
        //        if (fireCountdown <= 0)
        //        {
        //            Shoot();
        //            fireCountdown = 1f / stats.fireRate;
        //        }

        //        fireCountdown -= Time.deltaTime;
        //    }
        //}
        //else
        //{
        //    if (stats.useLaser)
        //    {/*
        //        if (lineRenderer.enabled)
        //        {
        //            lineRenderer.enabled = false;
        //            impactEffect.Stop();
        //            impactLight.enabled = false;
        //        }*/
        //    }
        //}
    }

    protected abstract void UpdateCall();

    protected void Laser()
    {
        /*
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
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
        }*/
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, stats.range);
    }

    public Sprite GetImg()
    {
        return shopImg;
    }

    public int GetSellAmount()
    {
        return (int)(stats.cost / 1.2f);
    }

    public void SetWall(Wall wall)
    {
        this.wall = wall;
    }

    public void DisplayRange()
    {
        // todo
    }

    private void OnMouseDown()
    {
        wall.OnMouseDown();
    }
}
