using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : WalkableTileContent
{
    public Tile tile;

    // ---- INTERN ----
    private TargetPoint target;
    private int nbEnemyAlive = 0;
    private SpawnerManager spawnManager;
    private Transform enemyContainer;

    private bool isActive = false;

    void Update()
    {
        if(isActive && nbEnemyAlive <= 0)
        {
            isActive = false;
            nbEnemyAlive = 0;
            spawnManager.NotifySpawnerDown();
        }
    }

    public void Init(TargetPoint target, SpawnerManager spawnManager, Transform enemyContainer)
    {
        this.target = target;
        this.spawnManager = spawnManager;
        this.enemyContainer = enemyContainer;
    }

    public void Spawn(List<EnemyOrder> listToSpawn, int nbEnemies, float percentageGoldEarnt)
    {
        nbEnemyAlive = nbEnemies;
        StartCoroutine(SpawnWithDelay(listToSpawn, percentageGoldEarnt));
        isActive = true;
    }

    public void NotifyDeath()
    {
        --nbEnemyAlive;
    }

    private IEnumerator SpawnWithDelay(List<EnemyOrder> listToSpawn, float percentageGoldEarnt)
    {
        // foreach (EnemyOrder enemyOrder in listToSpawn)
        // {
           // Enemy enemy = enemyOrder.enemyToSpawn.GetComponent<Enemy>();
           // nbEnemyAlive += enemy.GetNbEnemies() * enemyOrder.nbEnemyToSpawn;
        // }

        int nbEnemyToSpawn = 0;
        foreach (EnemyOrder enemyOrder in listToSpawn)
        {
            nbEnemyToSpawn += enemyOrder.nbEnemyToSpawn;
        }


        int nbEnemySpawn = 0;
        while (nbEnemySpawn < nbEnemyToSpawn)
        {
            // select a random enemy
            int index = Random.Range(0, listToSpawn.Count);
            GameObject EnemyToSpawnGO = listToSpawn[index].enemyToSpawn;

            GameObject enemyCloneGO = (GameObject)Instantiate(EnemyToSpawnGO, transform.position, transform.rotation, enemyContainer);
            EnemyMovement enemyMovementClone = enemyCloneGO.GetComponent<EnemyMovement>();
            enemyMovementClone.SetTarget(target);
            Enemy enemyClone = enemyCloneGO.GetComponent<Enemy>();
            enemyClone.SetEnemySpawner(this);
            enemyClone.worth = (int) ((enemyClone.worth * percentageGoldEarnt) / 100);

            --listToSpawn[index].nbEnemyToSpawn;
            if(listToSpawn[index].nbEnemyToSpawn == 0)
            {
                listToSpawn.RemoveAt(index);
            }

            ++nbEnemySpawn;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
