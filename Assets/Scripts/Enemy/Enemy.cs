using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health { get { return health; } }

    public string monsterName = "Basic Enemy";
    [Range(0.05f, 5f)]
    public float startSpeed = 1f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100f;
    private float health;

    [Header("going to change")]
    public int worth = 50;

    [SerializeField]
    private GameObject[] spawnWhenDieArray = new GameObject[0];

    // ---- INTERN ----
    private EnemySpawner enemySpawner;

    // tuple<amount, duration>
    private Dictionary<DebuffName, List<Debuff>> dictionaryDebuff = new Dictionary<DebuffName, List<Debuff>>();

    void Start()
    {
        speed = startSpeed;
        health = startHealth;

        foreach (DebuffName debuffName in (DebuffName[])Enum.GetValues(typeof(DebuffName)))
        {
            dictionaryDebuff.Add(debuffName, new List<Debuff>());
        }
    }

    private void FixedUpdate()
    {
        foreach (List<Debuff> listDebuff in dictionaryDebuff.Values)
        {            // reduce duration
            for (int i = 0; i < listDebuff.Count; ++i)
            {
                listDebuff[i].duration -= Time.deltaTime;
                if (listDebuff[i].duration <= 0f)
                    listDebuff.RemoveAt(i);
            }

            // do effect
            if (listDebuff.Count > 0)
            {
                Debuff current = listDebuff[0];
                for (int i = 1; i < listDebuff.Count; ++i)
                {
                    if (listDebuff[i] > current)
                    {
                        current = listDebuff[i];
                    }
                }

                current.DoEffect();
            }
        }
    }

    void LateUpdate()
    {
        speed = startSpeed;
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount, Vector3 pos, Vector3 normal)
    {
        HitEffect(pos, normal);
        TakeDamage(amount);
    }

    public void Slow(float percent)
    {
        float newSpeed = startSpeed - ((percent * startSpeed) / 100);
        speed = newSpeed < speed ? newSpeed : speed;
    }

    public void Slow(float percent, float duration)
    {
        dictionaryDebuff[DebuffName.Slow].Add(new SlowDebuff(percent, duration, this));        
    }

    private void Die()
    {
        PlayerStats.Money += worth;

        if (spawnWhenDieArray != null)
        {
            foreach (GameObject go in spawnWhenDieArray)
            {
                GameObject gTmp = (GameObject)Instantiate(go, transform.position, Quaternion.identity);
                // put it in the enemy container
                gTmp.transform.parent = transform.parent;
                // give it the next waypoint
                EnemyMovement emChild = gTmp.GetComponent<EnemyMovement>();
                emChild.SetTarget(this.GetComponent<EnemyMovement>().Target);
                // and the enemySpawner
                Enemy enemy = gTmp.GetComponent<Enemy>();
                enemy.SetEnemySpawner(enemySpawner);
            }
        }

        // destroy the enemy
        Destroy(gameObject);
    }

    public void Explode()
    {
        // invoke explosion effect

        PlayerStats.Lives -= GetNbEnemies();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemySpawner.NotifyDeath();
    }

    private void HitEffect(Vector3 pos, Vector3 normal)
    {
        //GameObject effect = Instantiate(hitEffect, pos, transform.rotation);
        //Destroy(effect, 2f);
    }

    public int GetNbEnemies()
    {
        int nbEnemies = 1;
        if (spawnWhenDieArray.Length != 0)
        {
            foreach (GameObject goChild in spawnWhenDieArray)
            {
                Enemy child = goChild.GetComponent<Enemy>();
                nbEnemies += child.GetNbEnemies();
            }
        }

        return nbEnemies;
    }

    private enum DebuffName
    {
        Slow,
    }
}
