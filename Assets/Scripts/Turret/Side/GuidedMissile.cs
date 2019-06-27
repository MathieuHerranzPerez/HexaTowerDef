using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GuidedMissile : Projectile
{

    // ---- INERN ----
    private Rigidbody rb;

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
            if (rb.velocity.y <= 1)
            {
                Vector3 direction = target.position - transform.position;
                rb.AddForce(direction * speed);
            }
        }

        // rotate the missile to follow its gravity curve
        transform.LookAt(transform.position + rb.velocity);
    }

    void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    public void Impulse(float force)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
