using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour
{
    public TargetPoint EnemyTarget { get { return enemyGoal; } }

    [SerializeField]
    private AnimationCurve priceSingleEnemyByRoundCurve = default;
    [SerializeField]
    private AnimationCurve nbEnemySingleSpawnByRoundCurve = default;

    [SerializeField]
    private List<GameObject> listEnemyPrefab = new List<GameObject>();

    [SerializeField]
    private float timeToBuild = 60f;

    [Header("Setup")]
    [SerializeField]
    private Text waveCountdownText = default;
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
    private float countdown;
    private bool needToSpawn = false;


    void Start()
    {
        gameManager = GameManager.Instance;

        listEnemyPrefab.Sort((a, b) => a.GetComponent<Enemy>().GetNbEnemies().CompareTo(b.GetComponent<Enemy>().GetNbEnemies()));
        listEnemy = new Enemy[listEnemyPrefab.Count];
        int i = 0;
        foreach(GameObject enemyGO in listEnemyPrefab)
        {
            listEnemy[i] = enemyGO.GetComponent<Enemy>();
            ++i;
        }

        foreach(EnemySpawner es in listEnemySpawner)
        {
            es.Init(enemyGoal, this, enemyContainer);
        }

        countdown = timeToBuild;
    }

    void Update()
    {
        if (!areEnemiesDead)
        {
            if (nbSpawnerActive <= 0 && enemyContainer.childCount == 0)
            {
                areEnemiesDead = true;
                gameManager.NotifyEndWave();
                ++waveNum;
            }
        }
        else if(needToSpawn)
        {
            if (countdown <= 0f)
            {
                Spawn();
                countdown = timeToBuild;
                needToSpawn = false;
            }
            else
            {
                // if the player cancel the countdown
                if (Input.GetKeyDown(KeyCode.P))
                {
                    countdown = Mathf.Min(1f, countdown);
                }
                else
                {
                    countdown -= Time.deltaTime;
                    countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
                }
                waveCountdownText.text = string.Format("{0:00.0}", countdown);      // update the countdown UI text
            }
        }
    }

    public List<Tile> GetListTileOfSpawners()
    {
        List<Tile> res = new List<Tile>();
        foreach(EnemySpawner es in listEnemySpawner)
        {
            res.Add(es.tile);
        }
        return res;
    }

    public void SetReadyToSpawn()
    {
        needToSpawn = true;
    }

    public void NotifySpawnerDown()
    {
        --nbSpawnerActive;
    }

    public void AddSpawner(EnemySpawner spawner)
    {
        listEnemySpawner.Add(spawner);
        spawner.Init(enemyGoal, this, enemyContainer);
    }

    private void Spawn()
    {
        Debug.Log("Sapwn");
        areEnemiesDead = false;
        foreach(EnemySpawner enemySpawner in listEnemySpawner)
        {
            // build an order
            List<EnemyOrder> listOrder = new List<EnemyOrder>();
            int nbEnemyToSpawn = (int) nbEnemySingleSpawnByRoundCurve.Evaluate(waveNum);
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

            enemySpawner.Spawn(listOrder, nbEnemyToSpawn, priceSingleEnemyByRoundCurve.Evaluate(waveNum));
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
