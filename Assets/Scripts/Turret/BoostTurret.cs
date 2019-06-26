using System.Collections.Generic;
using UnityEngine;

public abstract class BoostTurret : SupportTurret
{
    [SerializeField]
    [Range(0f, 100f)]
    protected float boost = 20f;

    // ---- INTERN ----
    protected float boostUp;
    protected List<BoostableTurret> listTarget = new List<BoostableTurret>();
    protected List<BoostableTurret> listTargetToBoost = new List<BoostableTurret>();

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
                if (t != null && t is BoostableTurret)
                {
                    BoostableTurret st = (BoostableTurret)t;
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
