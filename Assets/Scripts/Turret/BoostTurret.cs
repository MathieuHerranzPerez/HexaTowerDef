using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoostTurret : SupportTurret
{
    [SerializeField]
    protected float boost = 2f;

    // ---- INTERN ----
    protected float boostUp;
    protected List<ShootingTurret> listTarget = new List<ShootingTurret>();
    protected List<ShootingTurret> listTargetToBoost = new List<ShootingTurret>();

    protected override void UpdateCall()
    {
        GetTargets();
        BoostAll();
    }

    protected void GetTargets()
    {
        listTargetToBoost.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.range, LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)));
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == gameObject.tag)
            {
                Turret t = collider.gameObject.GetComponent<Turret>();
                if (t != null && t is ShootingTurret)
                {
                    ShootingTurret st = (ShootingTurret)t;
                    if (!listTarget.Contains(st))
                    {
                        listTargetToBoost.Add(st);
                        listTarget.Add(st);
                    }
                }
            }
        }
    }

    protected abstract void BoostAll();
    protected abstract void UnboostAll();

    protected override void InitUpStats()
    {
        base.InitUpStats();

        BoostTurret bt = (BoostTurret)turretUp;
        boostUp = bt.boost;
    }

    public float GetBoost()
    {
        return boost;
    }

    public float GetBoostUp()
    {
        return boostUp;
    }

    void OnDestroy()
    {
        UnboostAll();
    }
}
