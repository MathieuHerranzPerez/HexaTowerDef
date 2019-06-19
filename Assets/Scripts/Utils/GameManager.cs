using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField]
    private SpawnerManager spawnerManager = default;
    [SerializeField]
    private BuildingMapManager buildingMapManager = default;

    // ---- ITERN ----

    void Awake()
    {
        if (Instance != null)
            throw new Exception("Multiple GameManager in the scene");

        Instance = this;
    }

    void Start()
    {
        // spawnerManager.Spawn();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            buildingMapManager.StartPhase(0);
        }
    }


    public void NotifyEndWave(int waveNum)
    {
        Debug.Log("Wave end ! " + waveNum); // todo
        spawnerManager.Spawn();
    }
}
