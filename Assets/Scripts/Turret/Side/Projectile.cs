using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int Damage { get { return damage; } }

    [SerializeField]
    protected float speed = 70f;
    [SerializeField]
    protected float explosionRadius = 0f;
    [SerializeField]
    protected int damage = 50;

    [Header("Setup")]
    // [SerializeField]
    // private GameObject impactEffect;
    [SerializeField]
    protected AudioClip soundWhenTuch = default;
    [Range(0.05f, 1f)]
    [SerializeField]
    protected float volume = 0.5f;
    [SerializeField]
    protected GameObject audioPlayer = default;

    // ---- INTERN ----
    protected Transform target;


    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        UpdateCall();
    }

    protected abstract void UpdateCall();

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    protected void HitTarget()
    {
        // instantiate particules
        // GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy(effectInstance, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            MakeDamage(target);
        }

        if (soundWhenTuch != null)
        {
            // invoke another gameobject to play the sound
            GameObject soundGO = (GameObject)Instantiate(audioPlayer, transform.position, transform.rotation);
            AudioPlayer _audioPlayer = soundGO.GetComponent<AudioPlayer>();
            _audioPlayer.Play(soundWhenTuch, volume);
            Destroy(soundGO, 1f);
        }

        Destroy(gameObject);
    }

    protected void MakeDamage(Transform enemyGameObject)
    {
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    protected void Explode()
    {
        // get all what is hit
        Collider[] colliderHitObjectArray = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliderHitObjectArray)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                MakeDamage(collider.transform);
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
