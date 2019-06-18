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

    void Awake()
    {
        if (Instance != null)
            throw new Exception("Multiple GameManager in the scene");

        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartSpawn());
    }

    public void NotifyEndWave(int waveNum)
    {
        Debug.Log("Wave end ! " + waveNum); // todo
        spawnerManager.Spawn();
    }


    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        spawnerManager.Spawn(); // todo
    }
}
