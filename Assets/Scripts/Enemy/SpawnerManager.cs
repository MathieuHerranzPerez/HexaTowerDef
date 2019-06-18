using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve priceSingleEnemyByRoundCurve;
    [SerializeField]
    private AnimationCurve nbEnemySingleSpawnByRoundCurve;

    [SerializeField]
    private List<GameObject> listEnemyPrefab = new List<GameObject>();

    [Header("Setup")]
    [SerializeField]
    private Transform enemyContainer = default;
    [SerializeField]
    private TargetPoint enemyGoal = default;
    [SerializeField]
    private List<EnemySpawner> listEnemySpawner = new List<EnemySpawner>();

    // ---- INTERN ----
    private GameManager gameManager;
    private Enemy[] listEnemy;
    
    private bool areEnemiesDead = true;
    private int nbSpawnerActive = 0;

    private int waveNum = 0;


    void Start()
    {
        gameManager = GameManager.Instance;

        listEnemyPrefab.Sort((a, b) => a.GetComponent<Enemy>().GetNbEnemies().CompareTo(b.GetComponent<Enemy>().GetNbEnemies()));
        listEnemy = new Enemy[listEnemyPrefab.Count];
        int i = 0;
        foreach(GameObject enemyGO in listEnemyPrefab)
        {
            Debug.Log("Enemy : " + enemyGO.GetComponent<Enemy>());
            listEnemy[i] = enemyGO.GetComponent<Enemy>();
            ++i;
        }

        foreach(EnemySpawner es in listEnemySpawner)
        {
            es.Init(enemyGoal, this, enemyContainer);
        }
    }

    void Update()
    {
        if (!areEnemiesDead)
        {
            if (nbSpawnerActive <= 0 && enemyContainer.childCount == 0)
            {
                areEnemiesDead = true;
                gameManager.NotifyEndWave(waveNum);
                ++waveNum;
            }
        }
    }

    public void NotifySpawnerDown()
    {
        --nbSpawnerActive;
    }

    public void AddSpawner(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        listEnemySpawner.Add(enemySpawner);

        //Vector3 pos = spawner.transform.position;
        spawner.transform.parent = transform;
        //spawner.transform.position = pos;

        enemySpawner.Init(enemyGoal, this, enemyContainer);
    }

    public void Spawn()
    {
        Debug.Log("Sapwn");
        areEnemiesDead = false;
        foreach(EnemySpawner enemySpawner in listEnemySpawner)
        {
            // build an order
            List<EnemyOrder> listOrder = new List<EnemyOrder>();
            int nbEnemyToSpawn = (int) nbEnemySingleSpawnByRoundCurve.Evaluate(waveNum) + 40;
            int nbCurrentEnemy = 0;
             
            Dictionary<GameObject, int> enemySelectedDico = new Dictionary<GameObject, int>();

            int maxIndex = listEnemyPrefab.Count - 1;
            while (nbCurrentEnemy < nbEnemyToSpawn)
            {
                maxIndex = GetMaxIndex(maxIndex, nbEnemyToSpawn - nbCurrentEnemy, listEnemy);

                int index = Random.Range(0, maxIndex + 1);
                if(enemySelectedDico.ContainsKey(listEnemyPrefab[index]))
                {
                    ++enemySelectedDico[listEnemyPrefab[index]];
                }
                else
                {
                    enemySelectedDico.Add(listEnemyPrefab[index], 1);
                }

                nbCurrentEnemy += listEnemy[index].GetNbEnemies();
            }

            foreach(GameObject enemyGO in enemySelectedDico.Keys)
            {
                listOrder.Add(new EnemyOrder(enemyGO, enemySelectedDico[enemyGO]));
            }

            enemySpawner.Spawn(listOrder, nbEnemyToSpawn);
        }
    }

    private int GetMaxIndex(int previousIndex, int nbEnemyToSpawn, Enemy[] listEnemy)
    {
        bool stop = false;
        int res = previousIndex;
        while (!stop)
        {
            if (listEnemy[res].GetNbEnemies() <= nbEnemyToSpawn)
                stop = true;
            else
                --res;       
        }

        return res;
    }
}
