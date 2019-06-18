using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get { return damage; } }

    [SerializeField]
    private float speed = 70f;
    [SerializeField]
    private float explosionRadius = 0f;
    [SerializeField]
    private int damage = 50;

    [Header("Setup")]
    // [SerializeField]
    // private GameObject impactEffect;
    [SerializeField]
    private AudioClip soundWhenTuch = default;
    [Range(0.05f, 1f)]
    [SerializeField]
    private float volume = 0.5f;
    [SerializeField]
    private GameObject audioPlayer = default;

    // ---- INTERN ----
    private Transform target;


    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 direction = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            // if we hit the target
            if (direction.magnitude <= distanceThisFrame)
            {
                HitTarget();
            }
            else
            {
                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                transform.LookAt(target);
            }
        }
    }

    private void HitTarget()
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

    private void MakeDamage(Transform enemyGameObject)
    {
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void Explode()
    {
        // get all what is hit
        Collider[] colliderHitObjectArray = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliderHitObjectArray)
        {
            if (collider.tag == "Enemy")
            {
                MakeDamage(collider.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
