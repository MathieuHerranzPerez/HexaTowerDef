using System;
using UnityEngine;

[Serializable]
public class TurretStats
{
    public int cost;
    public float range = 15f;
    public float turnSpeed = 10f;

    [Header("Upgrade")]
    public int upgradeCost = 100;
    public GameObject upgradedPrefab;

    [HideInInspector]
    public float rangeUp;
    [HideInInspector]
    public float baseRange;
}
