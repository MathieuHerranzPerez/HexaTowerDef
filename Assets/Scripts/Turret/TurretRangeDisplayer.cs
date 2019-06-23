using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRangeDisplayer : MonoBehaviour
{
    public static TurretRangeDisplayer Instance { get; private set; }

    [Header("Setup")]
    [SerializeField]
    private GameObject sphereRangeGO = default;

    void Awake()
    {
        Instance = this;
    }

    public void DisplayRange(Turret turret)
    {
        this.transform.position = turret.transform.position;
        sphereRangeGO.transform.localScale = new Vector3(turret.stats.range * 2, turret.stats.range * 2, turret.stats.range * 2);
        sphereRangeGO.SetActive(true);
    }

    public void HideRange()
    {
        sphereRangeGO.SetActive(false);
    }
}
