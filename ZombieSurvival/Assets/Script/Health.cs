using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Panel")]
    [SerializeField]
    private GameObject deathPanel;

    private GameObject objPool;
    public delegate void ZombieDied();
    public static event ZombieDied OnZombieDied;

    // Start is called before the first frame update
    void Start()
    {
        objPool = GameObject.Find("ZombiePool");
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
                deathPanel.SetActive(true);
                Time.timeScale = 0;
                gameObject.SetActive(false);
            }
            else if (gameObject.CompareTag("Zombie"))
            {
                gameObject.GetComponent<Health>().ResetHealth();
                objPool.GetComponent<ObjPool>().ReturnToQueue(gameObject);
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
