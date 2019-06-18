using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string monsterName = "Basic Enemy";
    [Range(1f, 50f)]
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100f;
    private float health;

    [Header("going to change")]
    public int worth = 50;

    [SerializeField]
    private GameObject[] spawnWhenDieArray;

    // ---- INTERN ----
    private EnemySpawner enemySpawner;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    void Update()
    {
        
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
        speed = startSpeed * (1f - percent);
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
}
