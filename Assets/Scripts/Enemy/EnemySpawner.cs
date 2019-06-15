using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject enemyPrefab = default;
    [SerializeField]
    private Transform target = default;     // todo get the nearest door


    void Start()
    {
        StartSpawning();
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        while(true)
        {
            GameObject enemyCloneGO = (GameObject)Instantiate(enemyPrefab, transform.position, transform.rotation, transform);
            EnemyMovement enemyMovementClone = enemyCloneGO.GetComponent<EnemyMovement>();

            enemyMovementClone.SetTarget(target);

            yield return new WaitForSeconds(1f);
        }
    }
}
