using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GameObject turretToBuildPrefab { get; private set; }
    public bool CanBuild { get { return turretToBuildPrefab != null; } }

    void Awake()
    {
        if(Instance != null)
        {
            throw new System.Exception("More that one buildManager in the scene");
        }
        Instance = this;
    }

    public void SetWall(Wall wall)
    {
        // todo
    }


    public void SetTurretToBuild(GameObject turretToBuild)
    {
        this.turretToBuildPrefab = turretToBuild;
    }

    // check if we have enought money to build the selected turret
    public bool HasMoney()
    {
        return PlayerStats.Money >= turretToBuildPrefab.GetComponent<Turret>().stats.cost;
    }

    public void DeselectWall()
    {
        // todo
    }

    public void BuildTurretOn(Wall wall)
    {
        if (HasMoney())
        {
            PlayerStats.Money -= turretToBuildPrefab.GetComponent<Turret>().stats.cost;

            GameObject turretCloneGO = (GameObject)Instantiate(turretToBuildPrefab, wall.turretSpawnPoint.position, Quaternion.identity, wall.transform);
            wall.SetTurretGO(turretCloneGO);
        }
    }
}
