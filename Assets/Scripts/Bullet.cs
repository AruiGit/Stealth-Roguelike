using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Player player;
    public GameObject sprite, capsule;
    int damage;

    void Start()
    {
        player = GameObject.Find("Player_Character").GetComponent<Player>();
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            DestroyAll();
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            DestroyAll();
        }
        
    }

    void DestroyAll()
    {
        Destroy(capsule);
        Destroy(sprite);
        Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
