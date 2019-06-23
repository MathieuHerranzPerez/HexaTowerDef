﻿using System;
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
    private int numWave = 0;

    void Awake()
    {
        if (Instance != null)
            throw new Exception("Multiple GameManager in the scene");

        Instance = this;
    }

    void Start()
    {
        buildingMapManager.StartPhase(numWave);
    }

    void Update()
    {

    }


    public void NotifyEndWave(int waveNum)
    {
        Debug.Log("Wave end ! " + waveNum); // todo
        spawnerManager.Spawn();
    }

    public void EndByWin()
    {
        Debug.Log("WIIIIN !");
        // todo
    }
}
