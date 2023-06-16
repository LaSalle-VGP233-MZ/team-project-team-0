using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Zombie : MonoBehaviour
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float damage;
    [SerializeField] private float structureDamage;
    [SerializeField] private float attackDelay;

    AIDestinationSetter pathfinder;
    private float attackDelayTimer = 0;

    // Start is called before the first frame update
    private void Start()
    {
        pathfinder = GetComponent<AIDestinationSetter>();
        pathfinder.target = GameObject.Find("Player").transform;
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
                attackDelayTimer = attackDelay;
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
        if (collider.CompareTag("Breakable"))
        {
            collider.gameObject.GetComponent<Health>().ReduceHealth(structureDamage);
            attackDelayTimer = attackDelay;
        }
    }
}
