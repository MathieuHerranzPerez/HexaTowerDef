using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GuidedMissile : Projectile
{
    [SerializeField]
    private float rotateSpeed = 180f;

    [Header("Setup")]
    [SerializeField]
    private GameObject EffectGO = default;
    [SerializeField]
    private AudioClip SoundAtMissileLaunch = default;
    [Range(0f, 1f)]
    [SerializeField]
    private float volumeLaucnh = 0.5f;

    // ---- INERN ----
    private bool isChasing = false;

    private Quaternion rocketTargetRotation;

    void Start()
    {
        if (explosionRadius == 0)
            explosionRadius = 0.1f;
    }

    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);
    }

    protected override void UpdateCall()
    {
        if (target != null)
        {
            if (isChasing || rb.velocity.y <= 1)
            {
                if (!isChasing)
                {
                    isChasing = true;
                    EffectGO.SetActive(true);
                    GameObject soundGO = (GameObject)Instantiate(audioPlayer, transform.position, transform.rotation);
                    AudioPlayer _audioPlayer = soundGO.GetComponent<AudioPlayer>();
                    _audioPlayer.Play(SoundAtMissileLaunch, volumeLaucnh);
                    Destroy(soundGO, 1.5f);
                }

                // direction
                Vector3 direction = target.position - transform.position;
                direction.Normalize();

                // rotation
                rocketTargetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rocketTargetRotation, Time.deltaTime * rotateSpeed);

                rb.velocity = transform.forward * speed;
            }
            else
            {
                // rotate the missile to follow its gravity curve
                transform.LookAt(transform.position + rb.velocity);
            }
        }
        else
        {
            // rotate the missile to follow its gravity curve
            transform.LookAt(transform.position + rb.velocity);
        }
    }

    public void Impulse(float force)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
    }

    protected override void ActOnTriggerEnter(Collider other)
    {
        HitTarget(target);
    }
}
