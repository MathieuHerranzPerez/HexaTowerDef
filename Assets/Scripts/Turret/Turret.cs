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
    }

    protected abstract void UpdateCall();

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
