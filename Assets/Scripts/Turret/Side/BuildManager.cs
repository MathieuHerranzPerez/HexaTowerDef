using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GameObject turretToBuildPrefab { get; private set; }
    public bool CanBuild { get { return turretToBuildPrefab != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuildPrefab.GetComponent<Turret>().stats.cost; } }

    [SerializeField]
    private TurretUI turretUI = default;
    // ---- INTERN ----
    private Wall selectedWall;

    void Awake()
    {
        if(Instance != null)
        {
            throw new System.Exception("More that one buildManager in the scene");
        }
        Instance = this;
    }

    public void SetTurretToBuild(GameObject turretToBuild)
    {
        this.turretToBuildPrefab = turretToBuild;

        DeselectWall();
    }

    public void SelectWall(Wall wall)
    {
        if(selectedWall == wall)
        {
            DeselectWall();
        }
        else
        {
            selectedWall = wall;
            turretToBuildPrefab = null;

            turretUI.SetTarget(wall);
        }
    }

    public void DeselectWall()
    {
        selectedWall = null;
        turretUI.Hide();
    }

    public void BuildTurretOn(Wall wall)
    {
        if (HasMoney)
        {
            PlayerStats.Money -= turretToBuildPrefab.GetComponent<Turret>().stats.cost;

            GameObject turretCloneGO = (GameObject)Instantiate(turretToBuildPrefab, wall.turretSpawnPoint.position, Quaternion.identity, wall.transform);
            wall.SetTurretGO(turretCloneGO);
        }
    }
}
