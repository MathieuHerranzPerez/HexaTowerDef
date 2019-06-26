using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Turret : MonoBehaviour
{
    public TurretStats stats;

    [Header("Setup")]
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
    protected Turret turretUp = null;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        InitBaseStats();

        if (stats.upgradedPrefab != null)
        {
            turretUp = stats.upgradedPrefab.GetComponent<Turret>();

            InitUpStats();
        }
    }

    protected virtual void InitBaseStats()
    {
        stats.baseRange = stats.range;
    }

    protected virtual void InitUpStats()
    {
        stats.rangeUp = turretUp.stats.range;
    }

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

    private void OnMouseDown()
    {
        wall.OnMouseDown();
    }
}
