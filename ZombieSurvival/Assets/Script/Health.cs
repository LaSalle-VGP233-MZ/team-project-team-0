using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public delegate void ZombieDied();
    public static event ZombieDied OnZombieDied;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            if (gameObject.CompareTag("Breakable"))
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (gameObject.CompareTag("Player"))
            {
                gameObject.SetActive(false);
            }
            else if (gameObject.CompareTag("Zombie"))
            {
                gameObject.GetComponent<Health>().ResetHealth();
                gameObject.SetActive(false);
                TriggerZombieDied();
            }
        }
    }
    public void ReduceHealth(float amount)
    {
        currentHealth -= amount;
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public void TriggerZombieDied()
    {
        if (OnZombieDied != null)
        {
            OnZombieDied();
        }
    }
}
