using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Stats
    private int healthPoints = 5;
    private int damage = 1;
    private float attackRange = 0.1f;
    private float attackSpeed = 1;
    private float attackTime = 0.767f;
    private float movementSpeed = 1f;

    //Items
    [SerializeField] private List<GameObject> passiveItems = new List<GameObject>();
    private GameObject activeItem;


    //UI
    [SerializeField] Text damageText, attackSpeedText, attackTimeText, movementspeedText;
    //img to hp


    //Difficulty parameters
    private int killedEnemies = 0;

    void Update()
    {
        if (healthPoints <= 0)
        {
            //Lost Screen Load
           // Destroy(gameObject);
        }

        UpdateUI();

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

    public float GetAttackTime()
    {
        return attackTime;
    }

    public void UseActiveItem()
    {
        //item.active();
    }

    public void ChangeHP(int value)
    {
        healthPoints = healthPoints + value;
    }

    public void ChangeDamage(int value, bool type)
    {
        if (type == true)
        {
            damage = damage + value;
        }
        else if(value!=0)
        {
            damage = damage * value;
        }
    }
    public void ChangeAttackSpeed(float value)
    {
        attackSpeed =attackSpeed + attackSpeed * value/100;
        attackTime = attackTime * 1 / attackSpeed;
        if (attackTime < 0.1f)
        {
            attackTime = 0.1f;
        }
    }

    public void ChangeMovementSpeed(float value)
    {
        movementSpeed *= value;
    }

    public void AddPassiveItem(GameObject item)
    {
        passiveItems.Add(item);
    }

    public void UpdateUI()
    {
        damageText.text = "Damage: " + damage;
        attackSpeedText.text = "Attack Speed: " + attackSpeed + " /s";
        attackTimeText.text = "Attack Time: " + attackTime + " s";
        movementspeedText.text = "Movement Speed " + movementSpeed;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void EnemyKilled()
    {
        killedEnemies++;
    }
}
