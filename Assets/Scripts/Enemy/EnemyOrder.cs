using System;
using UnityEngine;

[Serializable]
public class EnemyOrder
{
    public GameObject enemyToSpawn;
    public int nbEnemyToSpawn;

    public EnemyOrder(GameObject enemyToSpawn, int nbEnemyToSpawn)
    {
        this.enemyToSpawn = enemyToSpawn;
        this.nbEnemyToSpawn = nbEnemyToSpawn;
    }
}
