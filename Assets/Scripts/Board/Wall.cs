using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
public class Wall : TileContent
{
    public Transform turretSpawnPoint = default;
    public Turret Turret { get { return turret; } }

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
            turret.SetWall(this);
        }
    }

    protected void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (AreConditionsOK() && buildManager.HasMoney)
        {
            rend.material.color = hoverCanPurchaseColor;
        }
        else
        {
            rend.material.color = hoverColor;
        }
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turretGO != null)
        {
            buildManager.SelectWall(this);
            return;
        }
        else
        {
            if (!AreConditionsOK())
                return;

            buildManager.BuildTurretOn(this);
        }
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
        turret.SetWall(this);

        turretGO.transform.parent = transform;
    }

    public void SellTurret()
    {
        // give money to the user
        PlayerStats.Money += turret.GetSellAmount();

        // effect animation on delete
        // GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
       //  Destroy(effect, 5f);

        // destroy the turret
        Destroy(turretGO);
        turret = null;
        turretGO = null;

        //isUpgraded = false;
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turret.stats.upgradeCost)
        {
            Debug.Log("Not enought money to upgrade it");
        }
        else
        {
            PlayerStats.Money -= turret.stats.upgradeCost;

            // for the sell option
            int tmpCost = this.turret.stats.upgradeCost;

            // destroy the old turret
            Destroy(this.turretGO);

            // build the new turret
            GameObject turretCloneGO = (GameObject)Instantiate(turret.stats.upgradedPrefab, turretSpawnPoint.position, Quaternion.identity);
            SetTurretGO(turretCloneGO);

            this.turretGO = turretCloneGO;
            this.turret = turret.GetComponent<Turret>();
            this.turret.stats.cost = tmpCost;
            turret.GetComponent<Turret>().SetWall(this);

            // effect animation on spawn
            // GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
            // Destroy(effect, 5f);

            //isUpgraded = true;
        }
    }
}
