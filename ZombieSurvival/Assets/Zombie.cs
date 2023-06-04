using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float damage;
    [SerializeField] private float structureDamage;
    [SerializeField] private float attackDelay;
    private float attackDelayTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach(var collider in colliders)
        {
            if(attackDelayTimer <= 0)
            {
                Attack(collider);
            }
            else
            {
                attackDelayTimer -= Time.deltaTime;
            }
        }
    }
    void Attack(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Health>().ReduceHealth(damage);
            attackDelayTimer = attackDelay;
        }
        if (collider.CompareTag("Structure"))
        {
            collider.gameObject.GetComponent<Health>().ReduceHealth(structureDamage);
            attackDelayTimer = attackDelay;
        }
    }
}
