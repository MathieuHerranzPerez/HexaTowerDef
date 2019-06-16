﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class Wall : TileContent
{
    public Transform turretSpawnPoint = default;

    [SerializeField]
    protected Color hoverColor = default;
    [SerializeField]
    protected Color hoverCanPurchaseColor = default;
    
    [Header("Optional")]
    [SerializeField]
    protected GameObject turretGO = null;

    // ---- INTERN ----
    protected Turret turret;
    protected BuildManager buildManager;

    protected Renderer rend;
    protected Color startColor;


    void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void Start()
    {
        buildManager = BuildManager.Instance;
        if(turretGO != null)
        {
            turret = turretGO.GetComponent<Turret>();
        }
    }

    protected void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (AreConditionsOK() && buildManager.HasMoney())
        {
            rend.material.color = hoverCanPurchaseColor;
        }
        else
        {
            rend.material.color = hoverColor;
        }
    }

    protected void OnMouseDown()
    {
        if (turretGO != null)
        {
            Debug.Log("Can't build turret here");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!AreConditionsOK())
            return;

        buildManager.BuildTurretOn(this);
    }

    protected virtual bool AreConditionsOK()
    {
        return buildManager.CanBuild;
    }

    protected void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public void SetTurretGO(GameObject turretGO)
    {
        this.turretGO = turretGO;
        turret = turretGO.GetComponent<Turret>();
    }
}
