using UnityEngine;

public class GuidedMissile : Projectile
{
    [SerializeField]
    private float rotateSpeed = 180f;
    [SerializeField]
    private float timeBeforeLaunch = 1.2f;

    [Header("Setup")]
    [SerializeField]
    private GameObject effectGO = default;
    [SerializeField]
    private AudioClip soundAtMissileLaunch = default;
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
            if(!isChasing)
            {
                timeBeforeLaunch -= Time.deltaTime;
                if(timeBeforeLaunch <= 0)
                {
                    isChasing = true;
                    effectGO.SetActive(true);
                    GameObject soundGO = (GameObject)Instantiate(audioPlayer, transform.position, transform.rotation);
                    AudioPlayer _audioPlayer = soundGO.GetComponent<AudioPlayer>();
                    _audioPlayer.Play(soundAtMissileLaunch, volumeLaucnh);
                    Destroy(soundGO, 1.5f);
                }
            }

            if (isChasing /*|| rb.velocity.y <= 0*/)
            {
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

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    protected override void ActOnCollisionEnter(Collision other)
    {
        HitTarget(target);
    }
}
