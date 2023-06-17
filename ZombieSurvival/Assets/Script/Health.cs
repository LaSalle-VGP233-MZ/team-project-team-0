using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthRegenAmount;
    [SerializeField] private AudioClip deathSound;


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
        if (gameObject.CompareTag("Player"))
        {
            if (currentHealth / maxHealth <= 0)
            {
                PlayDeathAudio();
                deathPanel.SetActive(true);
                Time.timeScale = 0;
                gameObject.SetActive(false);
            }
            else if (currentHealth / maxHealth < 0.5)
            {

                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (currentHealth + Time.deltaTime * healthRegenAmount < maxHealth) 
            {
                currentHealth += Time.deltaTime * healthRegenAmount;
            }
            else
            {
                currentHealth = maxHealth;
            }
        }
        else if (gameObject.CompareTag("Breakable"))
        {
            if (currentHealth / maxHealth <= 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (currentHealth / maxHealth < 0.5)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else if (gameObject.CompareTag("Zombie"))
        {
            if (currentHealth / maxHealth <= 0)
            {
                ResetHealth();
                transform.GetComponentInChildren<SpriteRenderer>().color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
                transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                gameObject.GetComponent<Zombie>().enabled = false;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.GetComponent<Pathfinding.AIPath>().enabled = false;
                Invoke(nameof(Death), 3f);
                Invoke(nameof(PlayDeathAudio), 0.1f);
                TriggerZombieDied();
            }
            else if (currentHealth / maxHealth < 0.5)
            {
                transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
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
    public void Death()
    {
        gameObject.SetActive(false);
        objPool.GetComponent<ObjPool>().ReturnToQueue(gameObject);
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        gameObject.GetComponent<Zombie>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<Pathfinding.AIPath>().enabled = true;
    }
    public void PlayDeathAudio()
    {
        AudioSource.PlayClipAtPoint(deathSound, new Vector3(0, 0, 0));
    }
}
