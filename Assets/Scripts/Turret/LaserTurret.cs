using UnityEngine;

public class LaserTurret : ShootingTurret
{
    [SerializeField]
    private LineRenderer lineRenderer = default;
    [SerializeField]
    private  ParticleSystem impactEffect = default;
    [SerializeField]
    private Light impactLight = default;

    // ---- INTERN ----
    private bool isPlayingSound = false;

    public override int GetDamage()
    {
        return (int) stats.damageOverTime;
    }

    protected override void UpdateCall()
    {
        base.UpdateCall();

        if(target != null)
        {
            RaycastHit hit;
            // check if there is no obstacle between the fire point and the target
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, stats.range))
            {
                if (hit.collider.gameObject.layer == enemyLayer)
                {
                    targetEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if(targetEnemy != null)
                        Laser();
                }
                else
                {
                    StopLaser();
                }
            }
            else
            {
                StopLaser();
            }
        }
        else
        {
            StopLaser();
        }
    }

    protected void Laser()
    {
        targetEnemy.TakeDamage(stats.damageOverTime * Time.deltaTime);
        targetEnemy.Slow(stats.slowPercent);

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
        }
    }

    private void StopLaser()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }
    }
}
