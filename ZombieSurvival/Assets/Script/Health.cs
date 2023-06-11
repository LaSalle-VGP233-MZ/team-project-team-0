using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
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
            else
            {
                gameObject.SetActive(false);
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
}
