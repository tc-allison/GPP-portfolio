using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform respawnPoint;

    public int maxHealth = 10;
    public int health;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.SetHealth(health);
        if(health <= 0)
        {
            health = maxHealth;
            Player.transform.position = respawnPoint.transform.position;
            healthBar.SetHealth(maxHealth);
        }
    }

   
}
