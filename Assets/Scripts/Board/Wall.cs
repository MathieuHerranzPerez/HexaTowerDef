using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class Wall : TileContent
{
    public Transform turretSpawnPoint = default;
    [SerializeField]
    private Color hoverColor = default;
    [SerializeField]
    private Color hoverCanPurchaseColor = default;
    
    [Header("Optional")]
    [SerializeField]
    private GameObject turretGO = null;

    // ---- INTERN ----
    private Turret turret;
    private BuildManager buildManager;

    private Renderer rend;
    private Color startColor;


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

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.CanBuild && buildManager.HasMoney())
        {
            rend.material.color = hoverCanPurchaseColor;
        }
        else
        {
            rend.material.color = hoverColor;
        }
    }

    private void OnMouseDown()
    {
        if(turretGO != null)
        {
            Debug.Log("Can't build turret here");
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!buildManager.CanBuild)
            return;

        buildManager.BuildTurretOn(this);
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public void SetTurretGO(GameObject turretGO)
    {
        this.turretGO = turretGO;
        turret = turretGO.GetComponent<Turret>();
    }
}
