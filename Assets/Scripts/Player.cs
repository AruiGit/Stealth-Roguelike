using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int healthPoints = 5;
    private int damage = 1;
    private float attackRange = 0.1f;
    private float attackSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints <= 0)
        {
            //Lost Screen Load
           // Destroy(gameObject);
        }

    }

    public void TakeDamage(int value)
    {
        healthPoints = -value;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }


}
