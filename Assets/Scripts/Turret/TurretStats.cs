using System;
using UnityEngine;

[Serializable]
public class TurretStats
{
    public int cost;
    public float range = 15f;
    public float turnSpeed = 10f;
    [Header("Use Bullets (default)")]
    [Range(0.05f, 20f)]
    public float fireRate = 1f;
    public GameObject bulletPrefab;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowPercent = 0.5f;

    [Header("Upgrade")]
    public int upgradeCost = 100;
    public GameObject upgradedPrefab;
    [HideInInspector]
    public float fireRateUP;
    [HideInInspector]
    public float damageUP;
    [HideInInspector]
    public float rangeUP;
    [HideInInspector]
    public float slowUP;
}
