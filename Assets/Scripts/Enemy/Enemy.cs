using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(1f, 50f)]
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100f;
    private float health;
    public int worth = 50;
    public bool isBoss = false;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    void Update()
    {
        
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
        // destroy the enemy
        Destroy(gameObject);
    }

    private void HitEffect(Vector3 pos, Vector3 normal)
    {
        //GameObject effect = Instantiate(hitEffect, pos, transform.rotation);
        //Destroy(effect, 2f);
    }
}
