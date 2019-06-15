using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public Turret turretToBuild { get; private set; }

    // ---- INTERN ----


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

    public void SetTurretToBuild(Turret turretToBuild)
    {
        this.turretToBuild = turretToBuild;
    }

    // check if we have enought money to build the selected turret
    public bool HasMoney()
    {
        return PlayerStats.Money >= turretToBuild.stats.cost;
    }

    public void DeselectWall()
    {
        // todo
    }
}
